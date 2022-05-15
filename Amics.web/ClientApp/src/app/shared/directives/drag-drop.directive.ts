import {
  Directive,
  HostBinding,
  HostListener,
  Output,
  EventEmitter,
  Input,
  ElementRef,
} from "@angular/core";
import { DomSanitizer, SafeUrl } from "@angular/platform-browser";

export interface FileHandle {
  file: File;
  url: SafeUrl;
}

@Directive({
  selector: "[appDrag]",
})
export class DragDirective {
  @Output() files: EventEmitter<FileHandle[]> = new EventEmitter();
  @Input() disableDropping: boolean = false;

  @HostBinding("style.background") private background = "#eee";

  constructor(private sanitizer: DomSanitizer, private el: ElementRef) {}

  @HostListener("dragover", ["$event"]) public onDragOver(evt: DragEvent) {
    evt.preventDefault();
    evt.stopPropagation();
    this.background = "#999";
    this.disableDragDrop(this.disableDropping);
  }

  @HostListener("dragleave", ["$event"]) public onDragLeave(evt: DragEvent) {
    evt.preventDefault();
    evt.stopPropagation();
    this.background = "#eee";
    this.disableDragDrop(this.disableDropping);
  }

  @HostListener("drop", ["$event"]) public onDrop(evt: DragEvent) {
    evt.preventDefault();
    evt.stopPropagation();
    this.background = "#eee";

    this.disableDragDrop(this.disableDropping);
    if (this.disableDropping) {
      return;
    }
    let files: FileHandle[] = [];
    for (let i = 0; i < evt.dataTransfer.files.length; i++) {
      const file = evt.dataTransfer.files[i];
      const url = this.sanitizer.bypassSecurityTrustUrl(
        window.URL.createObjectURL(file)
      );
      files.push({ file, url });
    }
    if (files.length > 0) {
      this.files.emit(files);
    }
  }
  private disableDragDrop(disable: boolean) {
    this.el.nativeElement.disable = disable;
  }
}
