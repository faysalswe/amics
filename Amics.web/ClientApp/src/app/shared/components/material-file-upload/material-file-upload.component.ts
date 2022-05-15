import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import {
  trigger,
  state,
  style,
  animate,
  transition,
} from "@angular/animations";
import {
  HttpClient,
  HttpResponse,
  HttpRequest,
  HttpEventType,
  HttpErrorResponse,
  HttpHeaders,
} from "@angular/common/http";
import { Subscription } from "rxjs/Subscription";
import { of } from "rxjs/observable/of";
import { catchError, last, map, tap } from "rxjs/operators";
import { FileHandle } from "../../directives/drag-drop.directive";
import { SafeScript, SafeUrl } from "@angular/platform-browser";
import { DeleteFileDialogComponent } from "../delete-file-dialog/delete-file-dialog.component";
import { MatDialog } from "@angular/material/dialog";
import { FileUploadModel } from "../../models/file-upload";
import { THIS_EXPR } from "@angular/compiler/src/output/output_ast";
import { FileService } from "../../services/file.service";
import { ToastrService } from "ngx-toastr"; 

@Component({
  selector: "app-material-file-upload",
  templateUrl: "./material-file-upload.component.html",
  styleUrls: ["./material-file-upload.component.scss"],
  animations: [
    trigger("fadeInOut", [
      state("in", style({ opacity: 100 })),
      transition("* => void", [animate(300, style({ opacity: 0 }))]),
    ]),
  ],
})
export class MaterialFileUploadComponent implements OnInit {
  /** Link text */
  @Input() text = "Browse";
  /** Name used in form which will be sent in HTTP request. */
  @Input() param = "file";
  /** Target URL for file uploading. */
  @Input() target = "api/File";
  /** File extension that accepted, same as 'accept' of <input type="f ile" />.
          By the default, it's set to 'image/*'. */
  @Input() accept = "*";

  @Input() existingFiles = null;
  @Input() disableDropping = false;
  @Input() orderId: number = null;
  @Input() productId: number = 0; 
  @Input() isDraft = true;

  @Input() isExternalUser = false;
  @Input() authToken = "";
  /** Allow you to add handler after its completion. Bubble up response text from remote. */
  @Output() complete = new EventEmitter<{
    name: string;
    location: string;
    id: number;
    fileAlreadyExists: boolean;
  }>();
  @Output() delete = new EventEmitter<string>();

  files: Array<FileUploadModel> = [];
  fileHandles: FileHandle[] = [];
  fileDownloadInitiated: boolean = false;

  constructor(
    private _http: HttpClient,
    private fileService: FileService,
    private readonly toastr: ToastrService,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    if (this.existingFiles !== null && this.existingFiles !== undefined) {
      this.existingFiles.forEach((f) => {
        this.files.push({
          id: f.id,
          fileName: f.fileName,
          data: null,
          url: "./assets/img/file.png",
          state: "in",
          inProgress: false,
          progress: 100,
          canRetry: false,
          canCancel: true,
          location: f.location,
          createdOn: f.createdOn,
        });
      });
    }
  }

  onClick() {
    const fileUpload = document.getElementById(
      "fileUpload"
    ) as HTMLInputElement;
    fileUpload.onchange = () => {
      for (let index = 0; index < fileUpload.files.length; index++) {
        const file = fileUpload.files[index];
        this.files.push({
          id: -1,
          fileName: file.name,
          data: file,
          url: "./assets/img/file.png",
          state: "in",
          inProgress: false,
          progress: 0,
          canRetry: false,
          canCancel: true,
          location: "",
          createdOn: new Date(),
        });
      }
      this.uploadFiles();
    };
    fileUpload.click();
  }

  cancelFile(file: FileUploadModel) {
    this.openDialog(file);
  }

  handleDeleteFile(file) {
    this.deleteFile(file.fileName, file.location);
    // if (file.data !== null) {
    //   file.sub.unsubscribe();
    // }
    this.removeFileFromArray(file);
  }
  retryFile(file: FileUploadModel) {
    this.uploadFile(file);
    file.canRetry = false;
  }
  deleteFile(fileName: string, location: string) {
    if (this.isDraft && (this.orderId === null || this.orderId === 0)) {
      this.fileService.DeleteFile(fileName, location).subscribe(
        (result) => {
          if (result != null) {
            // this.showBlobs();
            this.delete.emit(fileName);
          }
        },
        (error) => console.error(error)
      );
    } else {
      this.fileService
        .DeleteFileFromOrder(
          fileName,
          location, 
          this.orderId,
          this.productId,
          this.isExternalUser,
          this.authToken
        )
        .subscribe(
          (result) => {
            if (result != null) {
              // this.showBlobs();
              this.delete.emit(fileName);
              if (!this.isDraft) {
                this.toastr.success("Removed the file " + fileName);
              }
            }
          },
          (error) => console.error(error)
        );
    }
  }

  public downloadFile(filename: string, location: string) {
    this.fileDownloadInitiated = true;
    return this._http
      .post(
        this.target + "/download/",
        { fileName: filename, location: location },
        {
          responseType: "blob",
        }
      )
      .subscribe((result: any) => {
        if (result.type != "text/plain") {
          var blob = new Blob([result]);
          let saveAs = require("file-saver");
          let file = filename;
          saveAs(blob, file);
          this.fileDownloadInitiated = false;
        } else {
          this.fileDownloadInitiated = false;
          alert("File not found in Blob!");
        }
      });
  }
  private uploadToTempFolder(file: FileUploadModel) {
    this.fileService.uploadToTempFolder(file).subscribe((event: any) => {
      if (typeof event === "object") {
        //  this.removeFileFromArray(file);
        //  console.log(this.files);
        file.location = event.body.dbPath;
        file.id = event.body.newId;
        if (
          event.body.fileAlreadyExists &&
          this.files.filter((f) => f.fileName === file.fileName).length > 1
        ) {
          this.files.some((f, index) => {
            if (f.fileName === file.fileName) {
              this.files.splice(index, 1);
              return true;
            }
          });
        }
        this.complete.emit({
          name: file.data.name,
          location: event.body.dbPath,
          id: 0,
          fileAlreadyExists: false,
        });
      } else {
        this.toastr.error(event);
      }
    });
  }
  private uploadToFolder(file: FileUploadModel) {
    this.fileService
      .UploadFileToOrder( 
        this.orderId,
        this.productId,
        file,
        this.isExternalUser,
        this.authToken
      )
      .subscribe(
        (event: any) => {
          if (typeof event === "object") {
            file.location = event.body.destFile;
            file.id = event.body.newId;

            if (
              event.body.fileAlreadyExists &&
              this.files.filter((f) => f.fileName === file.fileName).length > 1
            ) {
              this.files.some((f, index) => {
                if (f.fileName === file.fileName) {
                  this.files.splice(index, 1);
                  return true;
                }
              });
            }
            // console.log(this.files);
            this.complete.emit({
              name: file.data.name,
              location: event.body.destFile,
              id: event.body.newId,
              fileAlreadyExists: event.body.fileAlreadyExists,
            });
            if (!this.isDraft) {
              this.toastr.success(`Successfully uploaded  ${file.data.name} .`);
            }
          } else {
            this.toastr.error(event);
          }
        },
        (err) => {
          this.toastr.error("Error while uploading ..");
        }
      );
  }
  private uploadFile(file: FileUploadModel) {
    if (file.id >= 0 || file.location !== "") {
      return;
    }

    //only upload new files
    if (this.orderId !== null && this.orderId !== 0) {     
          this.uploadToTempFolder(file);
    }
         
  }

  private uploadFiles() {
    const fileUpload = document.getElementById(
      "fileUpload"
    ) as HTMLInputElement;
    fileUpload.value = "";

    this.files.forEach((file) => {
      //not for existing files
      if (file.data !== null) {
        this.uploadFile(file);
      }
    });
  }

  private removeFileFromArray(file: FileUploadModel) {
    try {
      if (this.files !== null && this.files.length !== 0) {
        const index = this.files.indexOf(file);
        if (index > -1) {
          this.files.splice(index, 1);
        }
      }
    } catch (e) {
      console.log(
        file.data.name + " something bad happened while removing file!"
      );
      // console.log(e);
    }
  }

  filesDropped(files: FileHandle[]): void {
    this.fileHandles = files;
    this.fileHandles.forEach((f) => {
      const justDragged = {
        id: -1,
        fileName: f.file.name,
        data: f.file,
        // url: f.url,
        url: "assets/img/file.png",
        state: "in",
        inProgress: false,
        progress: 0,
        canRetry: false,
        canCancel: true,
        location: "",
        createdOn: new Date(),
      };

      this.files.push(justDragged);
      this.uploadFile(justDragged);
    });
  }

  openDialog(file): void {
    const dialogRef = this.dialog.open(DeleteFileDialogComponent, {
      width: "400px",
      data: { id: 0, name: file.fileName },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== undefined) {
        this.handleDeleteFile(file);
      }
    });
  }
}
