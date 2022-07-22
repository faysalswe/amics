
export class BulkTransferItem {    
    warehouse: string = '';
    location: string = '';
}

export class UpdateBulkTransfer {    
    WarehouseFrom: string = '';
    LocationFrom: string = '';
    WarehouseTo: string='';
    LocationTo: string = '';
    UserName : string = '';
}

export class BulkTransferItemResult {    
    id: string ='';
    itemNumber: string = '';
    rev: string = '';
    description: string='';
    quantity: number = 0;

}

export class LstMessage{
    Message: string ='';
}
