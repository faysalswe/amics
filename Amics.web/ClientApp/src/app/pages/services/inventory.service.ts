import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { InventoryStatus } from '../components/InventoryStatus/inventory.status.component';
import {
  IncreaseInventoryInt,
  SerialLotInt,
  TransLogInt,
  TransNumberRecInt,
} from '../../shared/models/rest.api.interface.model';
import {
  concatMap,
  map,
  Observable,
  tap,
  mergeMap,
  concatAll,
  of,
  pipe,
} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InventoryService {
  private readonly api = '{apiUrl}/api/Inventory';

  constructor(private readonly httpClient: HttpClient) {}

  getInventoryStatus(val: string, secUserId: String) {
    return this.httpClient.get<InventoryStatus>(
      `${this.api}/getInventoryStatus?ItemsId=${val}&SecUserId=${secUserId}`
    );
  }

  getTransLog(fromDate: string, toDate: string, reason: string = 'MISC REC') {
    var param = {
      FromDate: fromDate,
      ToDate: toDate,
    };
    return this.httpClient.get<TransLogInt[]>(`${this.api}/getTransLog`, {
      params: param,
    }); 

    /*  return this.httpClient.get<TransLogInt[]>("http://localhost:3000/getTransLog", {
      params: param,
    });  */
  }

  extractTransNum(): Observable<TransNumberRecInt> {
    return this.httpClient.get<TransNumberRecInt>(
      `${this.api}/getTransNumberRec`
    );
  }

  invSerLotApi_1(num: TransNumberRecInt, serLotArray: SerialLotInt[]) {
    serLotArray.forEach((a) => {
      a.transnum = Number(num.sp_rec);
    });
    return this.httpClient.post(`${this.api}/InsertInvSerLot`, serLotArray);
  }

  updateReceiptApi_2(num: TransNumberRecInt, obj: IncreaseInventoryInt) {
    obj.transNum = Number(num.sp_rec);
    return this.httpClient.post(`${this.api}/UpdateReceipt`, obj);
  }

  insertInvSerLot(serLotArray: SerialLotInt[], val: IncreaseInventoryInt) {
    return this.extractTransNum().pipe(
      concatMap((obj: any) =>
        this.invSerLotApi_1(obj, serLotArray).pipe(
          concatMap((res) => this.updateReceiptApi_2(obj, val))
        )
      )
    );
  }

  insertReceipt(val: IncreaseInventoryInt) {
    return this.extractTransNum().pipe(
      concatMap((obj) => this.updateReceiptApi_2(obj, val))
    );
  }
}
