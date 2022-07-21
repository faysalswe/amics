import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ChangeLocSearch, ChangeLocSearchDto } from '../models/changeLocSearch';

@Injectable({
  providedIn: 'root'
})
export class ChangeLocationService {

  private readonly api = '{apiUrl}/api/ChangeLocation';

  constructor(private readonly httpClient: HttpClient) { }

  getChangeLocSearch(changeLocSearch: ChangeLocSearchDto){
    return this.httpClient.get<ChangeLocSearch[]>
    (`${this.api}/ChangeLocSearch?projectId=${changeLocSearch.projectId}&projectName=${changeLocSearch.projectName}&er=${changeLocSearch.er}&budget=${changeLocSearch.budget}&user=${changeLocSearch.user}`);
  }
}
