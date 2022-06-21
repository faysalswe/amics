import { Guid } from "guid-typescript";

export interface ItemClass {
    id: string;
    itemclass: string;
}

export interface ItemType {
    id: string;
    itemtype: string;
}

export interface ItemCode {
    id: string;
    itemcode: string;
}

export interface Uom {
    id: string;
    uom: string;
    purchasingUom: string;
    factor: number;
}

export interface PartNumber {
    id: number;
    partnumber: string;
}
export interface WareHouse {
    id: Guid;
    warehouse: string;
}
export interface ReportLocation {
    id: Guid;
    location: string;
}