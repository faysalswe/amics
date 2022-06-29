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
  }

  updateReceipt(obj: IncreaseInventoryInt) {
    // var transNum$ = this.extractTransNum();
    // var updateReceipt$ = this.httpClient.post(`${this.api}/UpdateReceipt`, obj);

    return this.extractTransNum().pipe(
      map((num: TransNumberRecInt) => {
        obj.transNum = num.sp_rec;
        return obj;
      }),
      tap(console.log),
      concatMap((val) => this.updateReceiptApi(val))
    );
  }

  extractTransNum() {
    return this.httpClient.get<TransNumberRecInt>(
      `${this.api}/getTransNumberRec`
    );
  }

  invSerLotApi(serLotArray: SerialLotInt[]) {
    return this.httpClient.post(`${this.api}/InsertInvSerLot`, serLotArray);
  }

  updateReceiptApi(obj: IncreaseInventoryInt) {
    return this.httpClient.post(`${this.api}/UpdateReceipt`, obj);
  }

  insertInvSerLot(serLotArray: SerialLotInt[], obj: IncreaseInventoryInt) {
    var dataArray: any[] = [];
    this.extractTransNum().subscribe((num: TransNumberRecInt) => {
      var seq = num.sp_rec;

      serLotArray.forEach((obj) => {
        obj.transnum = Number(seq);
      });

      obj.transNum = Number(seq);

      this.invSerLotApi(serLotArray).subscribe((obj1) => {
        this.updateReceiptApi(obj).subscribe((obj2) => {
          console.log('Completed');
        });
      });
    });

    /*return this.extractTransNum().pipe(
      map((num: TransNumberRecInt) => {
        // obj.transNum = num.sp_rec;
        return num.sp_rec
      }),
      tap(console.log),
      of((val : string) => {
        console.log("testing 69....");
        serLotArray.forEach(obj => {
          obj.transnum = Number(val);
        })
        console.log(serLotArray);
        this.invSerLotApi(serLotArray);
        return val
      }),
      of((val : string) => {
        console.log("testing 77....");
        obj.transNum = Number(val);
        return this.updateReceiptApi(obj)
      }),
      concatAll()
    )*/
  }
}

