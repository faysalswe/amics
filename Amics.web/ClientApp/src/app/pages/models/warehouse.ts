import { Guid } from "guid-typescript";

export interface Warehouse {
    id: Guid;
    warehouse: string;
}

export interface WarehouseLocation {
    warehouseId: Guid;
    id: Guid; 
    location: string;
    invalid:boolean;
    sequenceNo:number;
    route:number;
}