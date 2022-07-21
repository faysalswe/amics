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
  TransData,
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
import { DecreaseRequestModel } from '../../models/rest.api.interface.model';

@Component({
  selector: 'app-decrease-inventory',
  templateUrl: 'decrease.inventory.component.html',
  styleUrls: ['./decrease.inventory.component.scss'],
  providers: [IncreaseInventoryService, DatePipe],
  //,changeDetection: ChangeDetectionStrategy.OnPush
})
export class DecreaseInventoryComponent implements AfterViewInit {
  @ViewChild('varReasonCode', { static: false })
  varReasonCode!: DxSelectBoxComponent;

  /* @ViewChild(DxFormComponent, { static: false }) form!: DxFormComponent;
  @ViewChild('quantityVar', { static: false }) quantityVar!: DxTextBoxComponent;
  @ViewChild('varWarehouse', { static: false })
  varWarehouse!: DxSelectBoxComponent;
  @ViewChild('varLocation', { static: false })
  varLocation!: DxSelectBoxComponent;
  @ViewChild('varPmSearch', { static: false }) varPmSearch!: PMSearchComponent;
  @ViewChild('varInvStatus', { static: false })
  varInvStatus!: PMSearchComponent; */
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

  miscReasonCntl!: AbstractControl;
  transDateCntl!: AbstractControl;
  miscRefCntl!: AbstractControl;
  miscSourceCntl!: AbstractControl;
  notesCntl!: AbstractControl;
  transNumCntl!: AbstractControl;

  tmpSourceVal = '';
  tmpRefVal = '';

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

  closeSerialPopUp: any;
  saveSerialPopUp: any;
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

    this.saveSerialPopUp = {
      text: 'Save and Exit',
      onClick(e: any) {
        that.loadingVisible = true;
        that.serialPopupVisible = false;

        var serialLst: SerBasicFormArrayModel[] = that.serialInvDetForms.value;
        let tmpObjLst: TransData[] = [];
        serialLst
          .filter((d) => d.isQuantitySelectedForDecrease)
          .forEach((obj) => {
            let tmpObj = <TransData>{};
            tmpObj.invSerialId = obj.serialId;
            tmpObj.itemsId = that.itemsId;
            tmpObj.source = '';
            tmpObj.transQty = Number(1);
            tmpObj.itemNumber = that.pmDetails.itemNumber;
            tmpObj.rev = '-';
            tmpObj.boxNum = 0;
            tmpObj.lineWeight = '';
            tmpObj.createdBy = that.user;
            tmpObjLst.push(tmpObj);
          });
        console.log(tmpObjLst);

        const adjstedQuantity = serialLst.filter(
          (d) => d.isQuantitySelectedForDecrease
        ).length;

        const decModel = <DecreaseRequestModel>{};

        decModel.pickTransdate = that.transDateCntl.value;
        decModel.pickMiscReason = that.miscReasonCntl.value;
        decModel.pickMiscRef = that.miscRefCntl.value;
        decModel.pickMiscSource = that.miscSourceCntl.value;
        decModel.pickSource = 'MISC PICK';
        decModel.pickNotes = that.notesCntl.value;
        decModel.pickUser = that.user;
        decModel.pickItemId = that.itemsId;
        decModel.pickQty = adjstedQuantity;

        that.decInvService
          .decreaseBasicInventory(tmpObjLst, decModel)
          .subscribe(() => {
            setTimeout(() => {
              that.refreshLog();
              that.formDirective.resetForm();
              that.myForm.reset();
              that.initializeFormData();
              that.pmDetails = new pmDetails();
              that.loadingVisible = false;
              notify('Successfully Saved', 'info', 1000);
            }, 1000);
          });
      },
    };

    this.closeSerialPopUp = {
      text: 'Cancel and Exit',
      onClick(e: any) {
        //that.initializeFormData();
        //that.pmDetails = new pmDetails();
        that.removeSerialInvDet();
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
          tmpObj.transQty = Number(obj.selectedQuantity);
          tmpObj.itemNumber = that.pmDetails.itemNumber;
          tmpObj.rev = '-';
          tmpObj.boxNum = 0;
          tmpObj.lineWeight = '';
          tmpObj.createdBy = that.user;
          tmpObjLst.push(tmpObj);
        });
        console.log(tmpObjLst);

        const adjstedQuantity = tmpObjLst.reduce(
          (a, obj) => a + obj.transQty,
          0
        );

        const decModel = <DecreaseRequestModel>{};

        decModel.pickTransdate = that.transDateCntl.value;
        decModel.pickMiscReason = that.miscReasonCntl.value;
        decModel.pickMiscRef = that.miscRefCntl.value;
        decModel.pickMiscSource = that.miscSourceCntl.value;
        decModel.pickSource = 'MISC PICK';
        decModel.pickNotes = that.notesCntl.value;
        decModel.pickUser = that.user;
        decModel.pickItemId = that.itemsId;
        decModel.pickQty = adjstedQuantity;

        that.decInvService
          .decreaseBasicInventory(tmpObjLst, decModel)
          .subscribe(() => {
            setTimeout(() => {
              that.refreshLog();
              that.formDirective.resetForm();
              that.myForm.reset();
              that.initializeFormData();
              that.pmDetails = new pmDetails();
              that.loadingVisible = false;
              notify('Successfully Saved', 'info', 1000);
            }, 1000);
          });
      },
    };

    this.closeBasicPopUp = {
      text: 'Cancel and Exit',
      onClick(e: any) {
        //that.initializeFormData();
        //that.pmDetails = new pmDetails();
        that.removeSerialInvDet();
        console.log(that.serialInvDetForms.length);
        that.basicPopupVisible = false;
      },
    };
  }

  private initializeFormData() {
    this.myForm = this.fb.group({
      miscReason: [this.defaultReason, [Validators.required]],
      transDate: [this.todayDate, [Validators.required]],
      miscRef: [this.tmpRefVal != '' ? this.tmpRefVal : this.defaultRef],
      miscSource: [
        this.tmpSourceVal != '' ? this.tmpSourceVal : this.defaultSource,
      ],
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
    this.miscReasonCntl = this.myForm.controls['miscReason'];
    this.miscRefCntl = this.myForm.controls['miscRef'];
    this.miscSourceCntl = this.myForm.controls['miscSource'];
    this.notesCntl = this.myForm.controls['notes'];
    this.transDateCntl = this.myForm.controls['transDate'];
    this.transNumCntl = this.myForm.controls['transNum'];
  }

  get serialInvDetForms() {
    return this.myForm?.get('serialInvDet') as FormArray;
  }

  addAllSerialInvDet(obj: pmWHLocation[]) {
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
    obj.forEach((ele, i) => {
      this.addBasic(ele, i);
    });
  }

  addBasic(ele: pmWHLocation, i: number) {
    const serialInvDet = this.fb.group(
      {
        elementId: ['id_' + i],
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

    this.serialInvDetForms.push(serialInvDet);
  }

  removeSerialInvDet() {
    this.serialInvDetForms.clear();
  }

  getSerElementIdValue(i: number) {
    return this.serialInvDetForms.value[i].serElementId;
  }
  getTagElementIdValue(i: number) {
    return this.serialInvDetForms.value[i].tagElementId;
  }

  ngOnInit(): void {
    console.log(this.pmDetails);
    this.loadingVisible = true;
    this.initializeFormData();
    this.fromDate.setDate(this.fromDate.getDate() - 1);

    var initData$ = forkJoin([
      this.incInvService.getDefaultValues(),
      this.searchService.getWarehouseInfo(''),
      this.searchService.getReasonCode('decrease'),
      this.inventoryService.getTransLog(
        this.fromDateStr(),
        this.toDateStr(),
        'MISC PICK'
      ),
    ]).pipe(
      tap((obj) => {
        console.log(obj);
        this.defaultVals = obj[0];
        this.warehouses = obj[1];
        this.reasons = obj[2];
        this.trasLogArray = obj[3];

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
      //this.focusAdjustQuantity();
      this.er = '';
      // this.initializeFormData();
      // console.log(JSON.stringify(item));
      this.pmDetails = item;
      this.defaultWarehouse = item.warehouse;
      this.defaultLocation = item.location;
      this.itemsId = item.id.toString();
      console.log(this.itemsId);

      // Initialize the form values
      //this.quantityCntl?.setValue(null);
      // this.costCntl?.setValue(item.cost);
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
    this.focusOnItemNumber();
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
      .getTransLog(this.fromDateStr(), this.toDateStr(), 'MISC PICK')
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
    if (this.myForm.valid) {
      // alert(this.pmDetails.invType);
      if (this.pmDetails.invType === 'SERIAL') {
        this.viewSerial$ = this.partmasterService
          .getViewSerial(this.itemsId, this.secUserId)
          .subscribe((res: pmSerial[]) => {
            if (res.length > 0) {
              this.removeSerialInvDet();
              this.addAllSerial(res);
              this.serialPopupVisible = true;
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
            } else {
              alert('There is no quantity to decrease');
            }
          });
      }
    } else {
      this.myForm.markAllAsTouched();
    }
  }

  increaseInvObj() {
    const body = <IncreaseInventoryInt>this.myForm?.value;
    body.transDate = this.datePipe
      .transform(body.transDate, 'MM/dd/yy')
      ?.toString();
    body.user1 = this.user?.toString();
    //body.sourcesRefId = new Guid(this.sourcesRefIdCntl?.value).toString();
    body.cost = Number(body.cost);
    body.itemNumber = this.pmDetails.itemNumber;
    body.licPlatFlage = true;
    body.receiverNum = 0;
    return body;
  }

  openReasonCodeBox() {
    this.varReasonCode?.instance.open();
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

  initPopUp() {
    setTimeout(() => {
      document.getElementById('id_0')?.focus();
    }, 500);
  }

  getDynamicElementId(i: number) {
    return this.serialInvDetForms.value[i].elementId;
  }

  handleSourceValueChange(e: any) {
    const newValue = e.value;
    if (newValue) {
      this.tmpSourceVal = this.miscSourceCntl.value;
    } else {
      this.tmpSourceVal = '';
    }
  }

  handleRefValueChange(e: any) {
    const newValue = e.value;
    if (newValue) {
      alert(this.miscRefCntl.value);
      this.tmpRefVal = this.miscRefCntl.value;
    } else {
      this.tmpRefVal = '';
    }
  }
}
