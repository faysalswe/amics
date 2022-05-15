import { Injectable } from "@angular/core";
import {
  HttpClient,
  HttpRequest,
  HttpEventType,
  HttpErrorResponse,
  HttpHeaders,
} from "@angular/common/http";
import { Observable, of } from "rxjs";
import { map, tap, last, catchError } from "rxjs/operators"; 
import { logging } from "protractor";
import { DeleteFileFromOrderVM } from "../models/delete-file";

@Injectable({
  providedIn: "root",
})
export class FileService {
  private readonly api = `api/File/`;
  constructor(private readonly httpClient: HttpClient) {}

  uploadToTempFolder(file): Observable<any> {
    const fd = new FormData();
    fd.append(file.data.name, file.data);
    const req = new HttpRequest("POST", this.api + "Upload", fd, {
      reportProgress: true,
    });

    file.inProgress = true;
    return this.httpClient.request(req).pipe(
      map((event) => {
        switch (event.type) {
          case HttpEventType.UploadProgress:
            file.progress = Math.round((event.loaded * 100) / event.total);
            break;
          case HttpEventType.Response:
            file.canCancel = true;
            file.inProgress = false;

            return event;
        }
      }),
      tap((message) => {}),
      last(),
      catchError((error: HttpErrorResponse) => {
        file.inProgress = false;
        file.canRetry = true;
        file.canCancel = false;
        return of(`${file.data.name} upload failed.`);
      })
    );
  }
  getJwtHttpOptions(token) {
    var headers_object = new HttpHeaders().set(
      "Authorization",
      `Bearer ${token}`
    );

    const httpOptions = {
      headers: headers_object,
    };
    return httpOptions;
  }
  UploadFileToOrder( 
    orderId: number,
    productId: number,
    file,
    isExternalUser = false,
    token = ""
  ): Observable<any> {
    const fd = new FormData();
    fd.append(file.data.name, file.data);

    let url =
      this.api +
      "UploadFileToOrder/" + 
      "/" +
      orderId +
      "/" +
      productId;
    let headerOptions = {};
    if (isExternalUser) {
      url =
        this.api +
        "UploadFileToOrderForExternalUser/" + 
        "/" +
        orderId +
        "/" +
        productId;
    }
    let req = new HttpRequest("POST", url, fd, {
      reportProgress: true,
    });
    if (isExternalUser) {
      req = req.clone({
        headers: req.headers.set("Authorization", `Bearer ${token}`),
      });
    }
    file.inProgress = true;
    return this.httpClient.request(req).pipe(
      map((event) => {
        switch (event.type) {
          case HttpEventType.UploadProgress:
            file.progress = Math.round((event.loaded * 100) / event.total);
            break;
          case HttpEventType.Response:
            file.canCancel = true;
            file.inProgress = false;
            return event;
        }
      }),
      tap((message) => {}),
      last(),
      catchError((error: HttpErrorResponse) => {
        file.inProgress = false;
        file.canRetry = true;
        file.canCancel = false;
        return of(`${file.data.name} upload failed.`);
      })
    );
  }

  DeleteFile(fileName: string, location: string) {
    // if (!del) return;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" }),
      body: { fileName: fileName, location: location },
    };

    return this.httpClient.delete(this.api + "Delete/", httpOptions);
  }
  DeleteFileFromOrder(
    fileName: string,
    location: string, 
    orderId: number,
    productId: number,
    isExternalUser = false,
    token = ""
  ) {
    // if (!del) return;
    let httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" }),
      body: new DeleteFileFromOrderVM(
        fileName,
        location,
        orderId,
        productId
      ),
    };
    let url = this.api + "DeleteFileFromOrder/";
    if (isExternalUser) {
      httpOptions = {
        headers: new HttpHeaders({ Authorization: `Bearer ${token}` }),
        body: new DeleteFileFromOrderVM(
          fileName,
          location, 
          orderId,
          productId
        ),
      };
      url = this.api + "DeleteFileFromOrderForExternalUser/";
    }
    return this.httpClient.delete(url, httpOptions);
  }
}
