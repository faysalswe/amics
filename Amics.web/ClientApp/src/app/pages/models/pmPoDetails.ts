import { Guid } from "guid-typescript";

export class pmPoDetails {
    id: Guid = Guid.createEmpty();;
    pomainId: Guid = Guid.createEmpty();
    pomain: string = '';
    linenum: number =0;
    poLine:number=0;
    quantity?: number = 0;
    received: number = 0;
    somain:string='';
    trans_date: string ='';
    supplier: string = '';
    p10:string = '';
    p11:string = '';
    p12:string = '';
    p13:string = '';
    p14:string = '';
    p15:string = '';
    p16:string = '';
    p17:string = '';
    p18:string = '';
    p19:string = '';
    p20:string = '';

}