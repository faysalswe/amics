
export class InquiryRequest {
    action : InquiryActionType = InquiryActionType.PartMaster;
    searchText :string='';
    user :string='';
}

export class InquiryResponse {

    itemnumber: string = '';
    description: string = '';
    location: string = '';
    quantity: string = '';
    serial: string = '';
    lotno: string = '';
    color: string = '';
    lic_Plate: number = 0;
    source: string = '';
    ref: string = '';
    cost: number = 0;
    itemtype: string = '';
    er: string = '';
    mdatin: string = '';
}

export enum InquiryActionType {
    PartMaster = 1,
    ER = 2,
    Location = 3,
    Description = 4,
    Serial = 5,
    Tag = 6,
    MdatIn = 7,
}
