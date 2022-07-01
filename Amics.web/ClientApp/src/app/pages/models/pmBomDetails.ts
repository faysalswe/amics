import { Guid } from "guid-typescript";

export class pmBomDetails {
    id: string = "00000000-0000-0000-0000-000000000000";
    itemsid_Parent: Guid = Guid.createEmpty();
    itemsid_Child: Guid = Guid.createEmpty();
    itemNumber: string = '';
    rev: string = '';
    description: string = '';
    quantity: number = 0;
    uomref: string = '';
    ref:string='';
    comments: string = '';
    findNo: string = '';
    lineNum: number = 0;
    cost?: number;
    extCost?: number;
    itemtype: string = '';
}