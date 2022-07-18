export interface LabelInt {
  id: string
  labelNumber: number
  myLabel: string
}

export interface DefaultValInt {
  id: string;
  formName: string;
  textFields: string;
  value: string;
}


export interface ReasonInt {
  id: string;
  reason: string;
}

export interface ERInt {
  id: string;
  soMain: string;
}

export interface CompanyOptionsInt {
  id: string;
  optionId: number;
  description: string;
  yesOrNo: boolean;
  optionValue: number;
}

export interface TransLogInt {
  id: string
  invtransid: string
  itemnumber: string
  description: string
  source: string
  ref: string
  quantity: number
  transDate: string
  createdDate: string
  createdBy: string
  serNo: string
  tagNo: string
  lotNo: string
  warehouse: string
  location: string
  cost: number
  notes: string
}

export interface IncreaseInventoryInt {
  sourcesRefId: string
  source: string
  extendedId: string
  warehouse: string
  location: string
  itemNumber: string
  rev: string
  cost: number
  quantity: number
  miscReason: string
  miscRef: string
  miscSource: string
  notes: string
  transDate?: string
  transNum: number
  poType: string
  recAccount: string
  recPackList: string
  licPlatFlage: boolean
  receiverNum: number
  user1?: string
  user2: string
}

export interface TransNumberRecInt {
  sp_rec: number
}

export interface SerialLotInt {
  transnum: number
  serNo: string
  tagNo: string
  lotNo: string
  model: string
  color: string
  qty: number
  createdBy: string
  expDate: string
}

export interface SerTagValidateInt {
  itemsid: string
  serialid: string
  locationsid: string
  itemnumber: string
  rev: string
  serno: string
  tagno: string
}

export interface SerBasicFormArrayModel {
  line: number;
  basicId: string;
  serialId: string;
  warehouse: string;
  location: string;
  er: string;
  quantity: number;
  cost: number;
  selectedQuantity: number;
  serNo: string;
  tagNo: string;
  model: string;
  isQuantitySelectedForDecrease: boolean;
}

export interface TransData {
  transNum: number;
  invBasicId: string;
  invSerialId: string;
  itemsId: string;
  source: string;
  refId: string;
  fromLocationId: string;
  toLocationId: string;
  transQty: number;
  itemNumber: string;
  rev: string;
  boxNum: number;
  lineWeight: string;
  createdBy?: string;
}


