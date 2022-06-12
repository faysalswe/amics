import { Guid } from "guid-typescript";

export class pmSearch {
    itemnumber: string = '';
    description: string = '';
    itemtype: string = '';
    itemclass: string = '';
    itemcode: string = '';

    constructor() { }

}

export class pmItemSearchResult {
    id: Guid = Guid.createEmpty();
    itemNumber: string = '';
    rev: string = '';
    description: string = '';
    itemType: string = '';
    itemCode: string = '';
    itemClass: string = '';
    uomref: string = '';
    cost: number = 0;
    dwgNo: string = '';
    conversion: number = 0;
}