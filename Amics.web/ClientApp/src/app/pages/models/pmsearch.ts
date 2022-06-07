export class pmSearch {
    itemnumber: string = '';
    description: string = '';
    itemtype: string = '';
    itemclass: string = '';
    itemcode: string = '';

    constructor() { }

}

export interface pmItemSearchResult {
    id: string;
    itemNumber: string;
    rev: string;
    description: string;
    itemType: string;
    itemCode: string;
    itemClass: string;
    uomref: string;
    cost: number;
    dwgNo: string;
    conversion: number;
}