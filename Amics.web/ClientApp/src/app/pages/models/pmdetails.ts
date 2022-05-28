export interface pmDetails {
    partNumber: string;
    description: string;
    itemType: string;
    itemClass: string;
    itemCode: string;
    cost: number;
    markup: number;
    price: number;
    uom: string;
    maintenance: string;
    defaultWarehouse:string;
    defaultLocation:string;
    invType: string;
    obsolete:boolean;
    taa: boolean;
    createPO: boolean;
}
