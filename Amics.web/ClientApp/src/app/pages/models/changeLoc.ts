export class ChangeLocRequest {
    projectId: string = '';
    projectName: string = '';
    er: string = '';
    budget: string = '';
    user: string = '';
}

export class ChangeLocProjDetails {
    projectName: string = '';
    warehouse: string = '';
    location: string = '';
}
export class ChangeLocSearchResult {
    project: string = '';
    name: string = '';
    soMain: string = "";
    lineNum: number = 0;
    soLinesId: string = "";
    itemnumber: string = "";
    description: string = "";
    itemType: string = "";
    quantity: number = 0;
    itemsId: string = "";
    invType: string = "";
    user: string = "";
    warehouse: string = "";
    location: string = "";
    serNo: string = "";
    tagNo: string = "";
    invSerialId: string = "";
    invBasicId: string = ""; 
}

export class ChgLocTransItem {
    id: string = '';
    action: number = 1;
    soLinesId: string = '';
    availQuantity: number = 0;
    transQuantity: number = 0;
    invSerialId: string = '';
    invBasicId: string = '';
    createdBy: string = '';

}