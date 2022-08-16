import { Guid } from "guid-typescript";

export class MdatSearch {
    mdatNum: string = '';
    er: string = '';
    packlistNum: string = '';
    status: string = '';

    constructor() { }
}

export class mdatItemSearchResult {
    id: Guid = Guid.createEmpty();
    actionFlag: string = '';
    mdatNum: string = '';
    somain: string = '';
    description: string = '';
    status: string = '';
    packlistnum: string = '';
    submitted_date: string = '';
    approved_date: number = 0;
    shipped_date: string = '';
    cancelled_date: string = '';
    createdby: string = '';
    shippingId: Guid = Guid.createEmpty();
}
