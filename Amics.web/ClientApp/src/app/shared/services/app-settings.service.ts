import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ApplicationSettings } from "../models/application-settings";
import { Observable, BehaviorSubject } from "rxjs";


@Injectable({
  providedIn: "root",
})
export class AppSettingsService { 
  private _appSettings = new BehaviorSubject<ApplicationSettings>(
    new ApplicationSettings(ApplicationSettings.ApiUrl)
  );
  appSettings$: Observable<
    ApplicationSettings
  > = this._appSettings.asObservable();
  constructor(private readonly httpClient: HttpClient) {}
  getAppSettings(): Promise<ApplicationSettings> {
    let settings = new ApplicationSettings(ApplicationSettings.ApiUrl);
    return this.httpClient
      .get<ApplicationSettings>("Config")
      .toPromise()
      .then((x) => {
        if (x) {
          settings = new ApplicationSettings(x.publicApiUrl);
        }
        this._appSettings.next(settings); 
        return settings;
      })
      .catch(() => {
        this._appSettings.next(settings);
        return settings;
      });
  }
}
