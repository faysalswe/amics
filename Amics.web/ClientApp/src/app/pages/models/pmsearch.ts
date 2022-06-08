export class pmSearch {
    itemnumber: string = '';
    description: string = '';
    itemtype: string = '';
    itemclass: string = '';
    itemcode: string = '';

    constructor() { }

}

export class pmItemSearchResult {
    id: string = '';
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