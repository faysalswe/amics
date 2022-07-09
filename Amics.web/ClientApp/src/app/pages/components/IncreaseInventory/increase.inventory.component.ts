import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { PMPOView } from '../../models/pmpoview';
import { HttpClient } from '@angular/common/http';
import { IncreaseInventoryService } from '../../services/increase.inventory.service';
import { ComponentType } from '../../models/componentType';
import { pmDetails } from '../../models/pmdetails';
import { Warehouse, WarehouseLocation } from '../../models/warehouse';
import {
  DefaultValInt,
  ERInt,
  IncreaseInventoryInt,
  ReasonInt,
  SerialLotInt,
  TransLogInt,
} from 'src/app/shared/models/rest.api.interface.model';
import { forkJoin, Subscription, tap } from 'rxjs';
import { PartMasterDataTransService } from '../../services/pmdatatransfer.service';
import { SearchService } from '../../services/search.service';
import { LabelMap } from '../../models/Label';
import { OptionIdMap } from '../../models/optionIdMap';
import { InventoryService } from '../../services/inventory.service';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormGroup,
  FormGroupDirective,
  Validators,
} from '@angular/forms';
import { DatePipe } from '@angular/common';
import { AuthService } from '../../../shared/services';
import Guid from 'devextreme/core/guid';
import {
  DxFormComponent,
  DxSelectBoxComponent,
  DxTextBoxComponent,
} from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import { PMSearchComponent } from '../PartMaster/search/pmsearch.component';
import { Employee, HomeService } from '../../services/home.service';
import { TransNumberRecInt } from '../../../shared/models/rest.api.interface.model';
import { DuplicateSerTagCheck } from 'src/app/shared/validator/duplicate.sertag.validator';
import { DuplicateSerTagErrorMsgService } from 'src/app/shared/validator/duplicate.sertag.msg.service';
import { ValidationService } from 'src/app/shared/services/validation.service';

@Component({
  selector: 'app-increase-inventory',
  templateUrl: 'increase.inventory.component.html',
  styleUrls: ['./increase.inventory.component.scss'],
  providers: [IncreaseInventoryService, DatePipe],
  //,changeDetection: ChangeDetectionStrategy.OnPush
})
export class IncreaseInventoryComponent implements AfterViewInit {
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

  componentType: ComponentType = ComponentType.IncreaseInventory;

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
  popupVisible = false;
  positionOf: string = 'window';
  employees!: Employee[];

  currentIncreaseInventoryIntObj!: IncreaseInventoryInt;

  loadingVisible = false;

  @ViewChild('formDirective', {static: false}) formDirective!: FormGroupDirective;

  constructor(
    private pmdataTransfer: PartMasterDataTransService,
    private incInvService: IncreaseInventoryService,
    private searchService: SearchService,
    private inventoryService: InventoryService,
    private fb: FormBuilder,
    private datePipe: DatePipe,
    private authService: AuthService,
    public dupsertagerrormsg: DuplicateSerTagErrorMsgService,
    public validationService: ValidationService
  ) {
    this.labelMap = LabelMap;
    this.optionIdMap = OptionIdMap;

    const that = this;

    this.closeButtonOptions = {
      text: 'Save and Exit',
      onClick(e: any) {
        that.loadingVisible = true;
        that.popupVisible = false;
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
        that.popupVisible = false;
      },
    };
  }

  private initializeFormData() {
    this.myForm = this.fb.group({
      sourcesRefId: [null],
      source: ['MISC REC', [Validators.required]],
      extendedId: [
        '00000000-0000-0000-0000-000000000000',
        [Validators.required],
      ],
      warehouse: ['', [Validators.required]],
      location: ['', [Validators.required]],
      itemNumber: [null],
      rev: ['-', [Validators.required]],
      cost: [null, [Validators.required]],
      quantity: [null, [Validators.required]],
      miscReason: [this.defaultReason, [Validators.required]],
      miscRef: [this.defaultRef, [Validators.required]],
      miscSource: [this.defaultSource, [Validators.required]],
      notes: [null],
      transDate: [this.todayDate, [Validators.required]],
      transNum: [null],
      poType: [null],
      recAccount: [null],
      recPackList: [null],
      licPlatFlage: [true, [Validators.required]],
      receiverNum: [0, [Validators.required]],
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

  addSerialInvDet(i: number) {
    const serialInvDet = this.fb.group(
      {
        serElementId: ['ser' + i],
        tagElementId: ['tag' + i],
        transnum: [],
        serNo: [
          '',
          DuplicateSerTagCheck.validate(
            this.serialInvDetForms,
            this.dupsertagerrormsg,
            this.validationService,
            this.itemsId,
            'SERIAL'
          ),
        ],
        tagNo: [
          '',
          DuplicateSerTagCheck.validate(
            this.serialInvDetForms,
            this.dupsertagerrormsg,
            this.validationService,
            this.itemsId,
            'TAG'
          ),
        ],
        model: [],
        lotNo: [],
        color: [],
        qty: [],
        createdBy: [],
        expDate: [],
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
      this.searchService.getReasonCode(),
      this.inventoryService.getTransLog(this.fromDateStr(), this.toDateStr()),
    ]).pipe(
      tap((obj) => {
        console.log(obj);
        this.defaultVals = obj[0];
        this.warehouses = obj[1];
        this.reasons = obj[2];
        this.trasLogArray = obj[3];

        console.log(JSON.stringify(this.trasLogArray));

        this.defaultSource = this.defaultVals.find(
          (x) => x.formName === 'ADJ-IN' && x.textFields === 'Source'
        )?.value as string;
        this.defaultReason = this.defaultVals.find(
          (x) => x.formName === 'ADJ-IN' && x.textFields === 'Reason Code'
        )?.value as string;
        this.defaultRef = this.defaultVals.find(
          (x) => x.formName === 'ADJ-IN' && x.textFields === 'Ref'
        )?.value as string;

        this.warehousesStr = this.warehouses.map((x) => x.warehouse);
        this.reasonsStr = this.reasons.map((x) => x.reason.trim());
      })
    );

    initData$.subscribe(() => {
      this.loadingVisible = false;
    });

    this.pmdataTransfer.selectedItemForInvDetails$.subscribe((item) => {
      this.focusAdjustQuantity();
      this.er = '';
      // this.initializeFormData();
      console.log(JSON.stringify(item));
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
    this.myForm.updateValueAndValidity();
    this.myForm.valueChanges.subscribe((val) => {
      console.log(this.quantityCntl);
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
    this.loadingVisible = true;
    const body = this.increaseInvObj();
    //body.quantity = Number(body.quantity);
    this.removeSerialInvDet();
    for (var i = 0; i < body.quantity; i++) {
      this.addSerialInvDet(i);
    }

    //this.pmDetails.invType = 'SERIAL';

    if (this.pmDetails.invType == 'SERIAL') {
      this.dupsertagerrormsg.add('');
      this.popupVisible = true;
      this.currentIncreaseInventoryIntObj = body;
      this.loadingVisible = false;
    } else {
      this.inventoryService.insertReceipt(body).subscribe((res: any) => {
        this.refreshLogs();
        this.isVisible = true;
        this.formDirective.resetForm();
        this.myForm.reset();
        this.initializeFormData();
        // this.itemsId = this.itemsId;
        this.pmDetails = new pmDetails();
        setTimeout(() => {
          this.loadingVisible = false;
          notify(res['message'], 'info', 500);
        }, 500);
      });
    }
    this.focusOnItemNumber();
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

  ngOnDestroy() {}

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
