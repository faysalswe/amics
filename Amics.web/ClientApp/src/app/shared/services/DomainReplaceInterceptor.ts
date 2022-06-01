import {
    HttpHandler,
    HttpInterceptor,
    HttpRequest,
    HttpEvent
} from '@angular/common/http';
import { first, map, mergeMap, Observable } from 'rxjs';
import { Injectable } from '@angular/core';

import { ApplicationSettings } from '../models/application-settings';
import { AppSettingsService } from './app-settings.service';
import { environment } from 'src/environments/environment';

function parseReplaceDomain(request: HttpRequest<any>) {
    var apiUrl = !ApplicationSettings.ApiUrl?environment.publicApiUrl:ApplicationSettings.ApiUrl;
    return request.clone({ url: request.url.replace('{apiUrl}', apiUrl) });
}

@Injectable()
export class DomainReplaceIterceptor implements HttpInterceptor {
   
    constructor(private readonly environmentService: AppSettingsService) {
    }
    intercept(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>
    > {
        return this.environmentService.appSettings$.pipe(
            first(),
            map(config => ({ config, hasUrlParam: request.url.includes('{apiUrl}') })),
            map(({ config, hasUrlParam }) => (!config || !hasUrlParam ? request : parseReplaceDomain(request))),
            mergeMap(req => next.handle(req))
        )
    }
}
