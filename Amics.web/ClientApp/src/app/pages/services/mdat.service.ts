import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { mdatItemSearchResult, MdatSearch } from '../models/mdatSearch';

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

}
