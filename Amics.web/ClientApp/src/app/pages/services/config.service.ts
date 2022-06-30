import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { pmSearch, pmItemSearchResult } from '../models/pmsearch';
import { ItemClass, ItemCode, ItemType, Uom } from '../models/searchModels';
import { Warehouse, WarehouseLocation } from '../models/warehouse';
import {
  CompanyOptionsInt,
  LabelInt,
  ReasonInt,
} from 'src/app/shared/models/rest.api.interface.model';

@Injectable({
  providedIn: 'root',
})
export class ConfigService {
  private readonly api = '{apiUrl}/api/Config';

  constructor(private readonly httpClient: HttpClient) {}

  getLabels() {
    return this.httpClient.get<LabelInt[]>(`${this.api}/Label`);
  }

  getCompanyOptions() {
    return this.httpClient.get<CompanyOptionsInt[]>(
      `${this.api}/GetCompanyOptions`
    );
  }
}
