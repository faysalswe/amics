import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import {
  SerTagValidateInt
} from '../../shared/models/rest.api.interface.model';

@Injectable({
  providedIn: 'root',
})
export class ValidationService {
  private readonly api = '{apiUrl}/api/Inventory';

  constructor(private readonly httpClient: HttpClient) {}

  validateSerTag(itemsid: string, sertag: string, option: string) {
    var param = {
      itemsid: itemsid,
      sertag: sertag,
      option: option,
    };
    return this.httpClient.get<SerTagValidateInt>(
      `${this.api}/ValidateSerTag`,
      {
        params: param,
      }
    );
  }
}
