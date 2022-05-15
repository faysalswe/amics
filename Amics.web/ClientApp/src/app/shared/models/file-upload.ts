import { SafeScript, SafeUrl } from "@angular/platform-browser";
import { Subscription } from "rxjs/Subscription";
import { logging } from "protractor";
export class FileUploadModel {
  id: number;
  fileName: string;
  data: File;
  url: SafeUrl;
  state: string;
  inProgress: boolean;
  progress: number;
  canRetry: boolean;
  canCancel: boolean;
  sub?: Subscription;
  location: string;
  createdOn: Date;
  constructor(
    id,
    fileName,
    data,
    url,
    state,
    inprogres,
    progress,
    canRetry,
    canCancel,
    sub,
    location,
    createdOn = null
  ) {
    this.id = id;
    this.fileName = fileName;
    this.data = data;
    this.url = url;
    this.state = state;
    this.inProgress = inprogres;
    this.progress = progress;
    this.canRetry = canRetry;
    this.canCancel = canCancel;
    this.sub = sub;
    this.location = location;
    this.createdOn = createdOn === null ? new Date() : createdOn;
  }
}
