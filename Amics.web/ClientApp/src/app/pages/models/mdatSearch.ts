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
    submitted_date: Date;
    approved_date: Date;
    shipped_date: Date = new Date();
    cancelled_date: Date;
    createdby: string = '';
    shippingId: string = Guid.EMPTY;
}

export interface SomainLookUp {
    id: string;
    somain: string;
}

export interface StatusLookUp {
    id: string;
    status: string;
}
