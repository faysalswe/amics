import { Guid } from "guid-typescript";

export interface ItemClass {
    id: Guid;
    itemclass: string;
}

export interface ItemType{
    id:Guid;
    itemtype:string;
}

export interface ItemCode{
    id:Guid;
    itemcode:string;
}

export interface Uom{
    id:Guid;
    uom:string;
    purchasingUom:string;
    factor:number;
}