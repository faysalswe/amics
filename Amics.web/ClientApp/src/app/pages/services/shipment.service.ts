import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { LstMessage } from "../models/bulktransfer";
import { ShipmentSearch, ShipmentSearchResult, ShipmentViewResult, ShipmentPickShipItem } from "../models/shipment";


@Injectable({
    providedIn: "root",
})
export class ShipmentService {
    private readonly api = '{apiUrl}/api/Shipment';

    constructor(private readonly httpClient: HttpClient) { }

    getShipmentSearchResult(req: ShipmentSearch) {
        if (req.projectId == "null") { req.projectId = ''; }
        if (req.er == "null") { req.er = ''; }
        if (req.projectName == "null") { req.projectName = ''; }
        if (req.budget == "null") { req.budget = ''; }
        if (req.task == "null") { req.task = ''; }
        let url = `${this.api}/ShipmentSearch?projectId=${req.projectId}&projectName=${req.projectName}&er=${req.er}&budget=${req.budget}`;
        return this.httpClient.get<ShipmentSearchResult[]>(url);
    }

    getShipmentView(req: ShipmentSearchResult) {
        if (req.project == "null") { req.project = ''; }
        if (req.soMain == "null") { req.soMain = ''; }        
        let url = `${this.api}/ShipmentViewByERProject?projectId=${req.project}&somain=${req.soMain}`;
        return this.httpClient.get<ShipmentViewResult[]>(url);
    }

    getShipmentViewDetails(req: ShipmentViewResult){         
        if (req.soMain == "null") { req.soMain = ''; }
        if (req.itemnumber == "null") { req.itemnumber = ''; }   
        if (req.user == "null") { req.user = ''; }   
        if (req.soLinesId == "null") { req.soLinesId = ''; }   
        if (req.invType == "null") { req.invType = ''; }  
        let username = "admin";   
        console.log(" service 38 so " + req.soMain + " itm " + req.itemnumber + " type " + req.invType + " user " +  req.user);   
        let url = `${this.api}/ShipmentViewDetails?somain=${req.soMain}&itemnumber=${req.itemnumber}&userId=${username}&soLinesId=${req.soLinesId}&invType=${req.invType}`;
        return this.httpClient.get<ShipmentViewResult[]>(url);
    }

    getShipmentSelectedItemDetails(req: ShipmentViewResult){         
        if (req.itemsId == "null") { req.itemsId = ''; }               
        if (req.soLinesId == "null") { req.soLinesId = ''; }   
        if (req.invType == "null") { req.invType = ''; }    
        let username = "admin";
        console.log(req.itemsId + " user " + req.user + " solines " +req.soLinesId + " invtype " + req.invType );
        let url = `${this.api}/ShipmentSelectedItems?itemsId=${req.itemsId}&username=${username}&solinesId=${req.soLinesId}&invType=${req.invType}`;
        return this.httpClient.get<ShipmentViewResult[]>(url);
    }

    DeleteInvPickShip(userName: string) {
        let url = `${this.api}/DeleteInvPickShip`;
        //return this.httpClient.post<string>(url, { 'User': userName });
        return this.httpClient.post<string>(url, { 'userName': userName });
    }

    UpdateInvPickShip(req: ShipmentPickShipItem[]) {
        let url = `${this.api}/UpdateInvPickShip`;
        return this.httpClient.post<string>(url, req);
    }

    UpdateShipment(userName: string, mdatout: string) {
        let url = `${this.api}/UpdateShipment?userName=${userName}&mdatout=${mdatout}`;
        return this.httpClient.get<string>(url);
    }
}