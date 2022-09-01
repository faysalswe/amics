import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Observable } from 'rxjs';
import { mdatItemSearchResult, MdatSearch, SomainLookUp, StatusLookUp } from '../models/mdatSearch';

@Injectable({
  providedIn: 'root'
})
export class MdatService {

private readonly api = '{apiUrl}/api/mdat';

constructor(private readonly httpClient: HttpClient) { }

getMdatSearchResults(mdatSearch: MdatSearch): Observable<mdatItemSearchResult[]> {

  let url = `${this.api}/GetMdatOutSearchDetails?mdatNum=${mdatSearch.mdatNum}&er=${mdatSearch.er}&packlistNum=${mdatSearch.packlistNum}&status=${mdatSearch.status}`;
  return this.httpClient.get<mdatItemSearchResult[]>(url);
}

getMdatViewDetails(itemId: string) {
  return this.httpClient.get<mdatItemSearchResult>(`${this.api}/GetMdatOutViewDetails?mdatNum=${itemId}`);
}

addorUpdateMDATDetails(item: mdatItemSearchResult): Observable<any> {
  return this.httpClient.post<string>(`${this.api}/UpdateMdatOutDetails`,item);
}

getSomainLookUp(somain: string, somainId: string, statusId: string): Observable<SomainLookUp[]> {
  let url = `${this.api}/Somain?searchSomain=${somain}&somainId=${somainId}&statusId=${statusId}`;
  return this.httpClient.get<SomainLookUp[]>(url);
}

getStatusLookUp(status: string, statusId: string): Observable<StatusLookUp[]> {
  let url = `${this.api}/Status?searchStatus=${status}&statusId=${statusId}`;
  return this.httpClient.get<StatusLookUp[]>(url);
}

}
