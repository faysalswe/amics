export class ShipmentSearch {
    projectId: string = '';
    projectName: string = '';
    er: string = '';
    budget: string = '';
    task: string = '';
}

export class ShipmentSearchResult {     
    project: string = '';
    name: string = '';
    soMain: string = "";
}

export class ShipmentSelectedItem {
    projectName: string = '';
    mdatOut: string = '';    
}

export class ShipmentViewResult {
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
    pickQty:number=0;
}


export class ShipmentPickShipItem {
    id: string = '';
    action: number = 1;
    soLinesId: string = '';
    availQuantity: number = 0;
    transQuantity: number = 0;
    invSerialId: string = '';
    invBasicId: string = '';
    createdBy: string = '';

}