import { Guid } from "guid-typescript";
import { BomAction } from "./pmBomGridDetails";

export class MdatSearch {
    mdatNum: string = '';
    er: string = '';
    packlistNum: string = '';
    status: string = '';

    constructor() { }
}

export class mdatItemSearchResult {
    id: string = Guid.EMPTY;
    actionFlag: BomAction = BomAction.Add;
    mdatNum: string = '';
    somain: string = '';
    description: string = '';
    status: string = '';
    packlistnum: number = 0;
    submitted_date: Date = new Date();
    approved_date: Date = new Date();
    shipped_date: Date = new Date();
    cancelled_date: Date = new Date();
    createdby: string = '';
    shippingId: string = Guid.EMPTY;
}
