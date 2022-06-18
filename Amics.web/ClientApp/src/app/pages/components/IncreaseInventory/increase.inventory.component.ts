import {Component} from '@angular/core';
import {PMPOView} from "../../models/pmpoview";
import {HttpClient} from "@angular/common/http";
import ArrayStore from 'devextreme/data/array_store';
import {Employee, IncreaseInventoryService} from "../../services/increase.inventory.service";
import { ComponentType } from '../../models/componentType';
import { pmDetails } from '../../models/pmdetails';
import { Warehouse, WarehouseLocation } from '../../models/warehouse';
import { DefaultValInt, ERInt, ReasonInt } from 'src/app/shared/models/rest.api.interface.model';
import { Subscription } from 'rxjs';
import { PartMasterDataTransService } from '../../services/pmdatatransfer.service';
import { SearchService } from '../../services/search.service';
import { LabelMap } from '../../models/Label';
import { OptionIdMap } from '../../models/optionIdMap';

@Component({
  selector: 'app-increase-inventory',
  templateUrl: 'increase.inventory.component.html',
  styleUrls: ['./increase.inventory.component.scss'],
  providers: [IncreaseInventoryService],
})

export class IncreaseInventoryComponent {

  // employee: Employee;

  labelMap: typeof LabelMap;
  optionIdMap: typeof OptionIdMap;

  componentType: ComponentType = ComponentType.IncreaseInventory;

  todayDate = new Date();
  pmpoviewArray: PMPOView[] = [];
  passwordButton: any;
  prevDateButton: any;
  afterDateButton: any;
  currencyIcon: any;

  pmDetails: pmDetails = new pmDetails();
  defaultWarehouse: string = '';
  defaultLocation: string = '';

  warehouses: Warehouse[] = [];
  warehousesStr: string[] = [];

  reasons: ReasonInt[] = [];
  reasonsStr: string[] = [];

  locations: WarehouseLocation[] = [];
  locationsStr: string[] = [];

  er: string = '';
  ers: ERInt[] = [];
  ersStr: string[] = [];

  defaultVals: DefaultValInt[] = [];
  defaultSource: string = '';
  defaultReason: string = '';
  defaultRef: string = '';

  itemsId: string = '';
  secUserId = 'E02310D5-227F-4DB8-8B42-C6AE3A3CB60B';

  defaultValue$: Subscription = new Subscription();

  constructor(
    private service: IncreaseInventoryService,
    private http: HttpClient,
    private pmdataTransfer: PartMasterDataTransService,
    private incInvService: IncreaseInventoryService,
    private searchService: SearchService
  ) {
    this.labelMap = LabelMap;
    this.optionIdMap = OptionIdMap;
  }


  ngOnInit(): void {
    this.defaultValue$ = this.incInvService
      .getDefaultValues()
      .subscribe((obj: DefaultValInt[]) => {
        this.defaultVals = obj;
        this.defaultSource = obj.find(
          (x) => x.formName === 'ADJ-IN' && x.textFields === 'Source'
        )?.value as string;
        this.defaultReason = obj.find(
          (x) => x.formName === 'ADJ-IN' && x.textFields === 'Reason Code'
        )?.value as string;
        this.defaultRef = obj.find(
          (x) => x.formName === 'ADJ-IN' && x.textFields === 'Ref'
        )?.value as string;
      });

    this.searchService.getWarehouseInfo('').subscribe((obj: Warehouse[]) => {
      this.warehouses = obj;
      this.warehousesStr = obj.map((x) => x.warehouse);
    });

    this.searchService.getReasonCode().subscribe((obj: ReasonInt[]) => {
      this.reasons = obj;
      this.reasonsStr = obj.map((x) => x.reason.trim());
    });

    this.pmdataTransfer.selectedItemForInvDetails$.subscribe((item) => {
      this.er = '';
      console.log(item);
      this.pmDetails = item;
      this.defaultWarehouse = item.warehouse;
      this.defaultLocation = item.location;
      this.itemsId = item.id.toString();

      this.incInvService.getER(this.itemsId).subscribe((obj: ERInt[]) => {
        this.ers = obj;
        this.ersStr = obj.map((x) => x.soMain);
        if (obj.length == 1) this.er = obj[0].soMain;
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

    this.passwordButton = {
      icon: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAADhAJiYAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAAAsTAAALEwEAmpwYAAAB7klEQVRYw+2YP0tcQRTFz65xFVJZpBBS2O2qVSrRUkwqYfUDpBbWQu3ELt/HLRQ/Q8RCGxVJrRDEwj9sTATxZ/Hugo4zL/NmV1xhD9xi59177pl9986fVwLUSyi/tYC+oL6gbuNDYtyUpLqkaUmfJY3a+G9JZ5J2JW1J2ivMDBSxeWCfeBxYTHSOWMcRYLOAEBebxtEVQWPASQdi2jgxro4E1YDTQIJjYM18hszGbew4EHNq/kmCvgDnHtI7YBko58SWgSXg1hN/btyFBM0AlwExczG1YDZrMS4uLUeUoDmgFfjLGwXEtG05wNXyTc4NXgzMCOAIGHD8q0ATuDZrempkwGJ9+AfUQ4K+A/eEseqZ/UbgdUw4fqs5vPeW+5mgBvBAPkLd8cPju+341P7D/WAaJGCdOFQI14kr6o/zvBKZYz11L5Okv5KGA89Kzu9K0b0s5ZXt5PjuOL6TRV5ZalFP4F+rrnhZ1Cs5vN6ijmn7Q162/ThZq9+YNW3MbfvDAOed5cxdGL+RFaUPKQtjI8DVAr66/u9i6+jJzTXm+HFEVqxVYBD4SNZNKzk109HxoycPaG0bIeugVDTp4hH2qdXJDu6xOAAWiuQoQdLHhvY1aEZSVdInG7+Q9EvSz9RrUKqgV0PP3Vz7gvqCOsUj+CxC9LB1Dc8AAAASdEVYdEVYSUY6T3JpZW50YXRpb24AMYRY7O8AAAAASUVORK5CYII=',
      type: 'default',
      onClick: () => {
        // this.passwordMode = this.passwordMode === 'text' ? 'password' : 'text';
      },
    };
    // throw new Error('Method not implemented.');
  }

  updateSelectedWarehouse(event$: any) {
    console.log(event$);
    var warehouseStr = event$['value'];
    var obj: Warehouse = this.warehouses.find(
      (x) => x.warehouse === warehouseStr
    ) as Warehouse;
    var warehouseId = obj?.id;

    this.searchService
      .getLocationInfo(warehouseId.toString(), '')
      .subscribe((obj: WarehouseLocation[]) => {
        this.locations = obj;
        this.locationsStr = obj.map((x) => x.location);
      });
  }

  ngOnDestroy() {
    this.defaultValue$.unsubscribe();
  }
}
