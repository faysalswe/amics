import { Guid } from "guid-typescript";

export class pmDetails {
    id: Guid = Guid.createEmpty();
    itemNumber: string = '';
    rev: string = '';
    dwgNo: string = '';

    description: string = '';
    salesDescription: string = '';
    purchaseDescription: string = '';

    itemType: string = '';
    itemCode: string = '';
    itemClass: string = '';

    cost: number = 0;
    markup: number = 0;
    price: number = 0;
    price2: number = 0;
    price3: number = 0;
    weight: number = 0;


    conversion: number = 0;
    leadTime: number = 0;
    minimum: number = 0;
    maximum: number = 0;

    uomref: string = '';
    purchasing_Uom: string = '';

    gLSales: string = '';
    gLInv: string = '';
    gLCOGS: string = '';

    userBit: boolean = false;
    userBit2: boolean = false;
    notes: string = '';
    warehouse: string = '';
    location: string = '';
    buyItem: boolean = false;
    obsolete: boolean = false;
    invType: string = '';
    child_ItemNumber: string = '';
    child_Rev: string = '';
    child_Description: string = '';
    child_Uom: string = '';
    child_Cost: number = 0;
    quantity: number = 0;
    lineNum: number = 0;
    user1: string = '';
    user2: string = '';
    user3: string = '';
    user4: string = '';
    user5: string = '';
    user6: string = '';
    user7: string = '';
    user8: string = '';
    user9: string = '';
    user10: string = '';
    user11: string = '';
    user12: string = '';
    user13: string = '';
    user14: string = '';
    user15: string = '';
    constructor() { }
}
