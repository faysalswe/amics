import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  DecreaseRequestModel,
  TransNumberRecInt,
} from '../models/rest.api.interface.model';
import { concatMap, Observable } from 'rxjs';
import { TransData } from 'src/app/shared/models/rest.api.interface.model';
import { resolveAny, resolveMx } from 'dns';

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

  updateReceiptApi_3(num: TransNumberRecInt, obj: DecreaseRequestModel) {
    obj.pickTransnum = Number(num.sp_rec);

    return this.httpClient.post(`${this.api}/ExecuteSpPick`, obj);
  }

  public decreaseBasicInventory(lst: TransData[], decModel: DecreaseRequestModel) {
    console.log('calling dec service');
    return this.extractTransNum().pipe(
      concatMap((obj) => this.updateReceiptApi_2(obj, lst).pipe(
        concatMap((res) => this.updateReceiptApi_3(obj, decModel))))
    )
  }
}
