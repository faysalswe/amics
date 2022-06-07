import {Component} from '@angular/core';
import {PMPOView} from "../../models/pmpoview";
import {HttpClient} from "@angular/common/http";
import ArrayStore from 'devextreme/data/array_store';
import {Employee, IncreaseInventoryService} from "../../services/increase.inventory.service";


export interface WarehouseInt {
  id: string;
  warehouse: string;
}

export interface LocationInt {
  id: string;
  location: string;
  invalid: boolean;
  sequenceNo: number;
  route: number;
}


export interface ReasonInt {
  id: string;
  reason: string;
}

@Component({
  selector: "app-increase-inventory",
  templateUrl: 'increase.inventory.component.html',
  styleUrls: ['./increase.inventory.component.scss'],
  providers: [IncreaseInventoryService]
})

export class IncreaseInventoryComponent {

  employee: Employee;

  positions: string[];

  rules: Object;
  birthDate = new Date(1981, 5, 3);
  pmpoviewArray: PMPOView[] = [];
  passwordButton: any;
  prevDateButton: any;
  afterDateButton: any;
  currencyIcon: any;

  locations: LocationInt[] = [];
  wareHouses: WarehouseInt[] = [];
  reasons: ReasonInt[] = [];
  now: Date = new Date();
  selectedWarehouseId = '';
  data: any;
  dataReason: any;
  dataLoc: any;

  constructor(service: IncreaseInventoryService, private http: HttpClient) {

    this.reasons = [{
      id: '123',
      reason: 'Demand'
    },
      {
        id: '123',
        reason: 'Test'
      }];

    this.http.get<WarehouseInt[]>('https://localhost:44327/api/Search/Warehouse').subscribe((obj: WarehouseInt[]) => {
      this.wareHouses = obj;
      console.log(this.wareHouses);

      this.data = new ArrayStore({
        data: this.wareHouses,
        key: 'id',
      });

      this.dataReason = new ArrayStore({
        data: this.reasons,
        key: 'id',
      });

    });

    this.prevDateButton = {
      icon: 'minus',
      stylingMode: 'text',
      onClick: () => {
        //this.dateValue -= this.millisecondsInDay;
      },
    };

    this.afterDateButton = {
      icon: 'plus',
      stylingMode: 'text',
      onClick: () => {
        //this.dateValue -= this.millisecondsInDay;
      },
    };

    this.currencyIcon = {
      text: 'USD',
      stylingMode: 'text',
      disabled: true,
      onClick: () => {
        //this.dateValue -= this.millisecondsInDay;
      },
    };

    this.employee = service.getEmployee();
    this.positions = service.getPositions();
    this.rules = {X: /[02-9]/};

    this.passwordButton = {
      icon: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAADhAJiYAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAAAsTAAALEwEAmpwYAAAB7klEQVRYw+2YP0tcQRTFz65xFVJZpBBS2O2qVSrRUkwqYfUDpBbWQu3ELt/HLRQ/Q8RCGxVJrRDEwj9sTATxZ/Hugo4zL/NmV1xhD9xi59177pl9986fVwLUSyi/tYC+oL6gbuNDYtyUpLqkaUmfJY3a+G9JZ5J2JW1J2ivMDBSxeWCfeBxYTHSOWMcRYLOAEBebxtEVQWPASQdi2jgxro4E1YDTQIJjYM18hszGbew4EHNq/kmCvgDnHtI7YBko58SWgSXg1hN/btyFBM0AlwExczG1YDZrMS4uLUeUoDmgFfjLGwXEtG05wNXyTc4NXgzMCOAIGHD8q0ATuDZrempkwGJ9+AfUQ4K+A/eEseqZ/UbgdUw4fqs5vPeW+5mgBvBAPkLd8cPju+341P7D/WAaJGCdOFQI14kr6o/zvBKZYz11L5Okv5KGA89Kzu9K0b0s5ZXt5PjuOL6TRV5ZalFP4F+rrnhZ1Cs5vN6ijmn7Q162/ThZq9+YNW3MbfvDAOed5cxdGL+RFaUPKQtjI8DVAr66/u9i6+jJzTXm+HFEVqxVYBD4SNZNKzk109HxoycPaG0bIeugVDTp4hH2qdXJDu6xOAAWiuQoQdLHhvY1aEZSVdInG7+Q9EvSz9RrUKqgV0PP3Vz7gvqCOsUj+CxC9LB1Dc8AAAASdEVYdEVYSUY6T3JpZW50YXRpb24AMYRY7O8AAAAASUVORK5CYII=',
      type: 'default',
      onClick: () => {
        // this.passwordMode = this.passwordMode === 'text' ? 'password' : 'text';
      },
    };
  }

  updateSelectedWarehouse(event$: any) {
    console.log(event$);
    this.selectedWarehouseId = event$["value"];

    var param = {
      warehouseId: this.selectedWarehouseId
    };

    this.http.get<LocationInt[]>('https://localhost:44327/api/Search/Location', {params: param}
    ).subscribe((obj: LocationInt[]) => {
      this.locations = obj;
      console.log(this.locations);
      this.dataLoc = new ArrayStore({
        data: this.locations,
        key: 'id',
      });
    });

  }
}
