export class pmSerial {
    id!: string;
    pomain!: string;
    serlot!: string;
    tagcol!: string;
    warehouse!: string;
    location !: string;
    quantity!: number;
    cost!: number;
    expdate !: string;
    color_model !: string;
    invtype !: string;
    actualSo !: string;
    currentSo !: string;
}


export class changeSerial {
    // itemNumber: string = "";
    // fromSerial: string = "";
    // toSerial: string = "";
    // fromTagNo: string = "";
    // toTagNo: string = "";
    // fromModel: string = "";
    // toModel: string = "";
    // fromCost: string = "";
    // toCost: string = "";

    itemNumber: string = "";
    serNoFm: string = "";
    tagNoFm: string = ""; 
    serNoTo: string = ""; 
    tagNoTo: string = ""; 
    notes: string = "";
    serialId: string = "";
    user: string = "";
    modelFm: string = "";
    modelTo: string = "";
    costFm: string = "";
    costTo: string = "";

}