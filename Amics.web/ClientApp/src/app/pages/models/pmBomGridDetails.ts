import { Guid } from "guid-typescript";

export class pmBomGridDetails {
    actionFlag: BomAction = BomAction.Add;
    id: string = Guid.EMPTY;
    parent_ItemsId: string = Guid.EMPTY;
    child_ItemsId: string = Guid.EMPTY;
    lineNum: number = 0;
    quantity: string = '';
    ref: string = '';
    comments: string = '';
    createdby: string = '';
    findNo: string = '';
}

export enum BomAction {
    Add = 1,
    Update = 2,
    Delete = 3
}