import { Guid } from "guid-typescript";

export interface warehouse {
    id: Guid;
    warehouse: string;
}

export interface Location {
    warehouseId: Guid;
    id: Guid; 
    location: string;
    invalid:boolean;
    sequenceNo:number;
    route:number;
}