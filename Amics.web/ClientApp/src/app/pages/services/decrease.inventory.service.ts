import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TransNumberRecInt } from '../models/rest.api.interface.model';
import { concatMap, Observable } from 'rxjs';
import { TransData } from 'src/app/shared/models/rest.api.interface.model';

@Injectable({ providedIn: 'root' })
export class DecreaseInventoryService {
  private readonly api = '{apiUrl}/api/Inventory';
  constructor(private httpClient: HttpClient) {}

  extractTransNum(): Observable<TransNumberRecInt> {
    return this.httpClient.get<TransNumberRecInt>(
      `${this.api}/getTransNumberRec`
    );
  }

  updateReceiptApi_2(num: TransNumberRecInt, lst: TransData[]) {
    lst.forEach((trasData) => {
      trasData.transNum = Number(num.sp_rec);
    });
    return this.httpClient.post(`${this.api}/InsertInvTrans`, lst);
  }

  public decreaseBasicInventory(lst: TransData[]) {
    console.log('calling dec service');
    return this.extractTransNum().pipe(
      concatMap((obj) => this.updateReceiptApi_2(obj, lst))
    );
  }
}
