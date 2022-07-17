import { DatePipe } from '@angular/common';
import { AfterViewInit, Component, ViewChild } from '@angular/core';
import {
  FormGroup,
  AbstractControl,
  FormGroupDirective,
  FormBuilder,
  Validators,
  FormArray,
} from '@angular/forms';
import {
  DxFormComponent,
  DxTextBoxComponent,
  DxSelectBoxComponent,
} from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import Guid from 'devextreme/core/guid';
import { Subscription, forkJoin, tap } from 'rxjs';
import { AuthService } from 'src/app/shared/services';
import { ValidationService } from 'src/app/shared/services/validation.service';
import { BasicQuantityValidator } from 'src/app/shared/validator/basic.quantity.validator';
import { DuplicateSerTagErrorMsgService } from 'src/app/shared/validator/duplicate.sertag.msg.service';
import { ComponentType } from '../../models/componentType';
import { LabelMap } from '../../models/Label';
import { OptionIdMap } from '../../models/optionIdMap';
import { pmDetails } from '../../models/pmdetails';
import { PMPOView } from '../../models/pmpoview';
import { pmSerial } from '../../models/pmSerial';
import { pmWHLocation } from '../../models/pmWHLocation';

import {
  DefaultValInt,
  ERInt,
  IncreaseInventoryInt,
  ReasonInt,
  SerBasicFormArrayModel,
  SerialLotInt,
  TransLogInt,
  TransData
} from 'src/app/shared/models/rest.api.interface.model';

//import { ReasonInt, ERInt, DefaultValInt, TransLogInt, IncreaseInventoryInt, SerialLotInt } from "../../models/rest.api.interface.model";
import { Warehouse, WarehouseLocation } from '../../models/warehouse';
import {
  IncreaseInventoryService,
  Employee,
} from '../../services/increase.inventory.service';
import { InventoryService } from '../../services/inventory.service';
import { PartMasterService } from '../../services/partmaster.service';
import { PartMasterDataTransService } from '../../services/pmdatatransfer.service';
import { SearchService } from '../../services/search.service';
import { PMSearchComponent } from '../PartMaster/search/pmsearch.component';
import { DecreaseInventoryService } from '../../services/decrease.inventory.service';

@Component({
  selector: 'app-decrease-inventory',
  templateUrl: 'decrease.inventory.component.html',
  styleUrls: ['./decrease.inventory.component.scss'],
  providers: [IncreaseInventoryService, DatePipe],
  //,changeDetection: ChangeDetectionStrategy.OnPush
})
export class DecreaseInventoryComponent implements AfterViewInit {
  @ViewChild(DxFormComponent, { static: false }) form!: DxFormComponent;
  @ViewChild('quantityVar', { static: false }) quantityVar!: DxTextBoxComponent;
  @ViewChild('varReasonCode', { static: false })
  varReasonCode!: DxSelectBoxComponent;
  @ViewChild('varWarehouse', { static: false })
  varWarehouse!: DxSelectBoxComponent;
  @ViewChild('varLocation', { static: false })
  varLocation!: DxSelectBoxComponent;
  @ViewChild('varPmSearch', { static: false }) varPmSearch!: PMSearchComponent;
  @ViewChild('varInvStatus', { static: false })
  varInvStatus!: PMSearchComponent;
  labelMap: typeof LabelMap;
  optionIdMap: typeof OptionIdMap;

  componentType: ComponentType = ComponentType.DecreaseInventory;

  todayDate = new Date();
  pmpoviewArray: PMPOView[] = [];
  passwordButton: any;
  triggerAdd: any;
  triggerSub: any;
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

  trasLogArray: TransLogInt[] = [];

  fromDate: Date = new Date();
  toDate: Date = new Date();

  now: Date = new Date();

  myForm!: FormGroup;
  sourcesRefIdCntl!: AbstractControl;
  sourceCntl!: AbstractControl;
  extendedIdCntl!: AbstractControl;
  warehouseCntl!: AbstractControl;
  locationCntl!: AbstractControl;
  itemNumberCntl!: AbstractControl;
  revCntl!: AbstractControl;
  costCntl!: AbstractControl;
  quantityCntl!: AbstractControl;
  miscReasonCntl!: AbstractControl;
  miscRefCntl!: AbstractControl;
  miscSourceCntl!: AbstractControl;
  notesCntl!: AbstractControl;
  transDateCntl!: AbstractControl;
  transNumCntl!: AbstractControl;
  poTypeCntl!: AbstractControl;
  recAccountCntl!: AbstractControl;
  recPackListCntl!: AbstractControl;
  licPlatFlageCntl!: AbstractControl;
  receiverNumCntl!: AbstractControl;
  user1Cntl!: AbstractControl;
  user2Cntl!: AbstractControl;

  user?: string = '';

  isVisible = false;
  toastMessage = '';
  type = 'info';

  emailButtonOptions: any;
  closeButtonOptions: any;
  currentEmployee!: Employee;
  serialPopupVisible = false;
  basicPopupVisible = false;
  positionOf: string = 'window';
  employees!: Employee[];

  currentdecreaseInventoryIntObj!: IncreaseInventoryInt;

  loadingVisible = false;

  @ViewChild('formDirective', { static: false })
  formDirective!: FormGroupDirective;

  viewWarehouseLocation$!: Subscription;
  viewSerial$!: Subscription;

  closeBasicPopUp: any;
  saveBasicPopUp: any;
  constructor(
    private pmdataTransfer: PartMasterDataTransService,
    private incInvService: IncreaseInventoryService,
    private searchService: SearchService,
    private inventoryService: InventoryService,
    private decInvService: DecreaseInventoryService,
    private fb: FormBuilder,
    private datePipe: DatePipe,
    private authService: AuthService,
    public dupsertagerrormsg: DuplicateSerTagErrorMsgService,
    public validationService: ValidationService,
    public partmasterService: PartMasterService
  ) {
    this.labelMap = LabelMap;
    this.optionIdMap = OptionIdMap;

    const that = this;

    this.closeButtonOptions = {
      text: 'Save and Exit',
      onClick(e: any) {
        that.loadingVisible = true;
        that.serialPopupVisible = false;
        var serialLst: SerialLotInt[] = that.serialInvDetForms.value;
        serialLst.forEach((obj) => {
          obj.qty = 1;
          obj.createdBy = String(that.user);
        });
        const body = that.increaseInvObj();
        that.loadingVisible = false;
        that.inventoryService.insertInvSerLot(serialLst, body).subscribe(() => {
          setTimeout(() => {
            that.refreshLog();
            that.formDirective.resetForm();
            that.myForm.reset();
            that.initializeFormData();
            that.pmDetails = new pmDetails();
            that.loadingVisible = false;
            notify('Successfully Saved', 'info', 500);
          }, 500);
        });
      },
    };

    this.emailButtonOptions = {
      text: 'Cancel and Exit',
      onClick(e: any) {
        //that.initializeFormData();
        //that.pmDetails = new pmDetails();
        that.serialPopupVisible = false;
      },
    };

    this.saveBasicPopUp = {
      text: 'Save and Exit',
      onClick(e: any) {
        that.loadingVisible = true;
        that.basicPopupVisible = false;

        var serialLst: SerBasicFormArrayModel[] = that.serialInvDetForms.value;
        let tmpObjLst: TransData[] = [];
        serialLst.forEach((obj) => {
          let tmpObj = <TransData>{};
          tmpObj.invBasicId = obj.basicId;
          tmpObj.itemsId = that.itemsId;
          tmpObj.source = '';
          tmpObj.transQty = obj.selectedQuantity;
          tmpObj.itemNumber = that.pmDetails.itemNumber;
          tmpObj.rev = 'rev';
          tmpObj.boxNum = 0;
          tmpObj.lineWeight = 0;
          tmpObj.createdBy = that.user;
          tmpObjLst.push(tmpObj);
        });
        console.log(tmpObjLst);

        that.decInvService.decreaseBasicInventory(tmpObjLst).subscribe(() => {
          that.loadingVisible = false;
        });
      },
    };

    this.closeBasicPopUp = {
      text: 'Cancel and Exit',
      onClick(e: any) {
        //that.initializeFormData();
        //that.pmDetails = new pmDetails();
        that.basicPopupVisible = false;
      },
    };
  }

  private initializeFormData() {
    this.myForm = this.fb.group({
      miscReason: [this.defaultReason, [Validators.required]],
      transDate: [this.todayDate, [Validators.required]],
      miscSource: [this.defaultSource, [Validators.required]],
      miscRef: [this.defaultRef, [Validators.required]],
      notes: [null],
      transNum: [null],
      rev: ['-', [Validators.required]],
      receiverNum: [0, [Validators.required]],
      sourcesRefId: [null],
      source: ['MISC REC', [Validators.required]],
      extendedId: [
        '00000000-0000-0000-0000-000000000000',
        [Validators.required],
      ],
      warehouse: [''],
      location: [''],
      itemNumber: [null],
      cost: [null],
      quantity: [null],
      poType: [null],
      recAccount: [null],
      recPackList: [null],
      licPlatFlage: [true],
      user1: [null],
      user2: [null],
      serialInvDet: this.fb.array([]),
    });

    this.sourcesRefIdCntl = this.myForm.controls['sourcesRefId'];
    this.sourceCntl = this.myForm.controls['source'];
    this.extendedIdCntl = this.myForm.controls['extendedId'];
    this.warehouseCntl = this.myForm.controls['warehouse'];
    this.locationCntl = this.myForm.controls['location'];
    this.itemNumberCntl = this.myForm.controls['itemNumber'];
    this.revCntl = this.myForm.controls['rev'];
    this.costCntl = this.myForm.controls['cost'];
    this.quantityCntl = this.myForm.controls['quantity'];
    this.miscReasonCntl = this.myForm.controls['miscReason'];
    this.miscRefCntl = this.myForm.controls['miscRef'];
    this.miscSourceCntl = this.myForm.controls['miscSource'];
    this.notesCntl = this.myForm.controls['notes'];
    this.transDateCntl = this.myForm.controls['transDate'];
    this.transNumCntl = this.myForm.controls['transNum'];
    this.poTypeCntl = this.myForm.controls['poType'];
    this.recAccountCntl = this.myForm.controls['recAccount'];
    this.recPackListCntl = this.myForm.controls['recPackList'];
    this.licPlatFlageCntl = this.myForm.controls['licPlatFlage'];
    this.receiverNumCntl = this.myForm.controls['receiverNum'];
    this.user1Cntl = this.myForm.controls['user1'];
    this.user2Cntl = this.myForm.controls['user2'];
  }

  get serialInvDetForms() {
    return this.myForm?.get('serialInvDet') as FormArray;
  }

  addAllSerialInvDet(obj: pmWHLocation[]) {
    // console.log(obj);
    obj.forEach((ele, i) => {
      this.addSerialInvDet(ele, i);
    });
  }

  addSerialInvDet(ele: pmWHLocation, i: number) {
    const serialInvDet = this.fb.group({
      line: [i + 1],
      warehouse: [ele.warehouse],
      location: [ele.location],
      er: [],
      quantity: [],
      cost: [],
      selectedQuantity: [],
      serNo: [],
      tagNo: [],
      model: [],
      isQuantitySelectedForDecrease: [],
    });
    this.serialInvDetForms.push(serialInvDet);
  }

  addAllSerial(obj: pmSerial[]) {
    // console.log(obj);
    obj.forEach((ele, i) => {
      this.addSerial(ele, i);
    });
  }

  addSerial(ele: pmSerial, i: number) {
    const serialInvDet = this.fb.group({
      line: [i + 1],
      basicId: [],
      serialId: [ele.id],
      warehouse: [ele.warehouse],
      location: [ele.location],
      er: [],
      quantity: [],
      cost: [ele.cost],
      selectedQuantity: [],
      serNo: [ele.serlot],
      tagNo: [ele.tagcol],
      model: [ele.color_model],
      isQuantitySelectedForDecrease: [],
    });
    this.serialInvDetForms.push(serialInvDet);
  }

  addAllBasic(obj: pmWHLocation[]) {
    // console.log(obj);
    obj.forEach((ele, i) => {
      this.addBasic(ele, i);
    });
  }

  addBasic(ele: pmWHLocation, i: number) {
    const serialInvDet = this.fb.group(
      {
        line: [i + 1],
        basicId: [ele.id],
        serialId: [],
        warehouse: [ele.warehouse],
        location: [ele.location],
        er: [ele.somain],
        quantity: [ele.quantity],
        cost: [''],
        selectedQuantity: [
          '',
          BasicQuantityValidator.validate(this.dupsertagerrormsg),
        ],
        serNo: [''],
        tagNo: [''],
        model: [''],
        isQuantitySelectedForDecrease: [],
      },
      { updateOn: 'blur' }
    );

    //serialInvDet.markAsTouched();
    //serialInvDet.updateValueAndValidity();
    //serialInvDet.markAsPending();
    // serialInvDet.markAsPristine();

    this.serialInvDetForms.push(serialInvDet);
    // this.myForm.markAllAsTouched();
  }

  removeSerialInvDet() {
    for (var i = 0; i < this.serialInvDetForms.length; i++) {
      this.serialInvDetForms.removeAt(i);
    }
  }

  getSerElementIdValue(i: number) {
    return this.serialInvDetForms.value[i].serElementId;
  }
  getTagElementIdValue(i: number) {
    return this.serialInvDetForms.value[i].tagElementId;
  }

  ngOnInit(): void {
    this.loadingVisible = true;
    this.initializeFormData();
    this.fromDate.setMonth(this.fromDate.getMonth() - 1);

    var initData$ = forkJoin([
      this.incInvService.getDefaultValues(),
      this.searchService.getWarehouseInfo(''),
      this.searchService.getReasonCode('decrease'),
      this.inventoryService.getTransLog(this.fromDateStr(), this.toDateStr()),
    ]).pipe(
      tap((obj) => {
        console.log(obj);
        this.defaultVals = obj[0];
        this.warehouses = obj[1];
        this.reasons = obj[2];
        this.trasLogArray = obj[3];

        // console.log(JSON.stringify(this.trasLogArray));

        this.defaultSource = this.defaultVals.find(
          (x) => x.formName === 'ADJ-OUT' && x.textFields === 'Source'
        )?.value as string;
        this.defaultReason = this.defaultVals.find(
          (x) => x.formName === 'ADJ-OUT' && x.textFields === 'Reason Code'
        )?.value as string;
        this.defaultRef = this.defaultVals.find(
          (x) => x.formName === 'ADJ-OUT' && x.textFields === 'Ref'
        )?.value as string;

        this.warehousesStr = this.warehouses.map((x) => x.warehouse);
        this.reasonsStr = this.reasons.map((x) => x.reason.trim());
      })
    );

    initData$.subscribe(() => {
      this.loadingVisible = false;
    });

    this.pmdataTransfer.selectedItemForDecInvDetails$.subscribe((item) => {
      console.log(item);
      this.focusAdjustQuantity();
      this.er = '';
      // this.initializeFormData();
      // console.log(JSON.stringify(item));
      this.pmDetails = item;
      this.defaultWarehouse = item.warehouse;
      this.defaultLocation = item.location;
      this.itemsId = item.id.toString();
      console.log(this.itemsId);

      // Initialize the form values
      this.quantityCntl?.setValue(null);
      this.costCntl?.setValue(item.cost);
      this.notesCntl?.setValue('');

      this.incInvService.getER(this.itemsId).subscribe((obj: ERInt[]) => {
        this.ers = obj;
        this.ersStr = obj.map((x) => x.soMain);
        if (obj.length == 1) this.er = obj[0].soMain;
      });
    });

    this.authService.getUser().then((e) => (this.user = e.userId));
    // this.myForm.updateValueAndValidity();
    this.myForm.valueChanges.subscribe((val) => {
      // // console.log(this.quantityCntl);
    });
  }

  ngAfterViewInit() {
    // this.focusAdjustQuantity();
    this.focusOnItemNumber();
  }

  private focusAdjustQuantity() {
    setTimeout(() => {
      this.quantityVar?.instance.focus();
    }, 0);
  }

  private refreshLogs() {
    this.inventoryService
      .getTransLog(this.fromDateStr(), this.toDateStr())
      .subscribe((obj: TransLogInt[]) => {
        this.trasLogArray = obj;
      });
  }

  updateSelectedWarehouse(event$: any) {
    if (event$ != null) {
      var warehouseStr = event$['value'];
      var obj: Warehouse = this.warehouses.find(
        (x) => x.warehouse === warehouseStr
      ) as Warehouse;
      var warehouseId = obj?.id;
      if (warehouseId != null) {
        this.searchService
          .getLocationInfo(warehouseId?.toString(), '')
          .subscribe((obj: WarehouseLocation[]) => {
            this.locations = obj;
            this.locationsStr = obj.map((x) => x.location);
          });
      }
    }
  }

  refreshLog() {
    this.loadingVisible = true;
    this.inventoryService
      .getTransLog(this.fromDateStr(), this.toDateStr())
      .subscribe((obj: TransLogInt[]) => {
        this.trasLogArray = obj;
        this.loadingVisible = false;
      });
  }

  fromDateStr(): string {
    return (
      this.fromDate.getFullYear() +
      '-' +
      (this.fromDate.getMonth() + 1) +
      '-' +
      this.fromDate.getDate()
    );
  }

  toDateStr(): string {
    return (
      this.toDate.getFullYear() +
      '-' +
      (this.toDate.getMonth() + 1) +
      '-' +
      this.toDate.getDate()
    );
  }

  fromDatehandler(e: any) {
    const previousValue = e.previousValue;
    const newValue = e.value;
    this.fromDate = e.value;
  }

  toDatehandler(e: any) {
    const previousValue = e.previousValue;
    const newValue = e.value;
    this.toDate = e.value;
  }

  update() {
    alert(this.pmDetails.invType);
    if (this.pmDetails.invType === 'SERIAL') {
      this.viewSerial$ = this.partmasterService
        .getViewSerial(this.itemsId, this.secUserId)
        .subscribe((res: pmSerial[]) => {
          // console.log(res);
          if (res.length > 0) {
            this.removeSerialInvDet();
            this.addAllSerial(res);
          } else {
            alert('There is no quantity to decrease');
          }
        });
    } else {
      this.viewWarehouseLocation$ = this.partmasterService
        .getViewWHLocation(this.itemsId, this.secUserId, '')
        .subscribe((res: pmWHLocation[]) => {
          console.log(res);

          if (res.length > 0) {
            this.removeSerialInvDet();
            this.addAllBasic(res);
            this.basicPopupVisible = true;
            const body = this.increaseInvObj();
            this.currentdecreaseInventoryIntObj = body;
            //this.loadingVisible = false;
          } else {
            alert('There is no quantity to decrease');
          }
        });
    }

    //this.loadingVisible = true;
    //const body = this.increaseInvObj();
    //body.quantity = Number(body.quantity);
    /* this.removeSerialInvDet();
    for (var i = 0; i < body.quantity; i++) {
      this.addSerialInvDet(i);
    } */

    //this.pmDetails.invType = 'SERIAL';

    /* if (this.pmDetails.invType == 'SERIAL') {
      this.dupsertagerrormsg.add('');
      this.serialPopupVisible = true;
      this.currentdecreaseInventoryIntObj = body;
      this.loadingVisible = false;
    } else {
      this.basicPopupVisible = true;
      this.currentdecreaseInventoryIntObj = body;
      this.loadingVisible = false;
    } */
    //this.focusOnItemNumber();
  }

  increaseInvObj() {
    const body = <IncreaseInventoryInt>this.myForm?.value;
    body.transDate = this.datePipe
      .transform(body.transDate, 'MM/dd/yy')
      ?.toString();
    body.user1 = this.user?.toString();
    body.sourcesRefId = new Guid(this.sourcesRefIdCntl?.value).toString();
    body.cost = Number(body.cost);
    body.itemNumber = this.pmDetails.itemNumber;
    body.licPlatFlage = true;
    body.receiverNum = 0;
    return body;
  }

  openReasonCodeBox() {
    this.varReasonCode?.instance.open();
  }

  openWarehouseBox() {
    this.varWarehouse?.instance.open();
  }

  openLocationBox() {
    this.varLocation?.instance.open();
  }

  ngOnDestroy() {
    this.viewWarehouseLocation$.unsubscribe();
  }

  private focusOnItemNumber() {
    setTimeout(() => {
      (<HTMLInputElement>document.getElementsByName('itemnumber')[0]).value =
        '';
      document.getElementsByName('itemnumber')[0].focus();
    }, 0);
  }

  isValid(c: AbstractControl) {
    return !(c.invalid && (c.dirty || c.touched));
  }
}
