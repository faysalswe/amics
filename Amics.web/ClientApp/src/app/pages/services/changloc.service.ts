import { registerLocaleData } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { ChangeLocRequest, ChangeLocSearchResult, ChgLocTransItem } from "../models/changeLoc";




@Injectable({
    providedIn: "root",
})
export class ChangeLocService {
    private readonly api = '{apiUrl}/api/ChangeLocation';

    constructor(private readonly httpClient: HttpClient) { }



    getChangeLocSearchResults(req: ChangeLocRequest) {
        if (req.projectId == "null") { req.projectId = ''; }
        if (req.er == "null") { req.er = ''; }
        if (req.projectName == "null") { req.projectName = ''; }
        if (req.budget == "null") { req.budget = ''; }
        let url = `${this.api}/ChangeLocSearch?projectId=${req.projectId}&projectName=${req.projectName}&er=${req.er}&budget=${req.budget}`;
        return this.httpClient.get<ChangeLocSearchResult[]>(url);
    }

    getChangeLocView(req: ChangeLocSearchResult) {
        if (req.project == "null") { req.project = ''; }
        if (req.soMain == "null") { req.soMain = ''; }
        if (req.name == "null") { req.name = ''; }

        let url = `${this.api}/ChangeLocView?projectId=${req.project}&somain=${req.soMain}`;
        return this.httpClient.get<ChangeLocSearchResult[]>(url);
    }

    getChangeLocItemsDetailsView(req: ChangeLocSearchResult) {
        if (req.itemnumber == "null") { req.itemnumber = ''; }
        if (req.soMain == "null") { req.soMain = ''; }
        if (req.soLinesId == "null") { req.soLinesId = ''; }

        let url = `${this.api}/ChangeLocViewDetails?somain=${req.soMain}&itemnumber=${req.itemnumber}&userId="admin"&soLinesId=${req.soLinesId}&invType=${req.invType}`;
        return this.httpClient.get<ChangeLocSearchResult[]>(url);
    }

    UpdateInvTransLoc(req: ChgLocTransItem[]) {
      
        let url = `${this.api}/UpdateInvTransLoc`;
        return this.httpClient.post<string>(url,req);
    }

    UpdateChangeLoc(req: ChangeLocSearchResult) {
        if (req.itemnumber == "null") { req.itemnumber = ''; }
        if (req.soMain == "null") { req.soMain = ''; }
        if (req.soLinesId == "null") { req.soLinesId = ''; }

        let url = `${this.api}/UpdateChangeLoc?somain=${req.soMain}&itemnumber=${req.itemnumber}&userId="admin"&soLinesId=${req.soLinesId}&invType=${req.invType}`;
        return this.httpClient.get<ChangeLocSearchResult[]>(url);
    }

    UpdateInvTransLoc(req: changeLocSearchResult) {
        if (req.itemnumber == "null") { req.itemnumber = ''; }
        if (req.soMain == "null") { req.soMain = ''; }
        if (req.soLinesId == "null") { req.soLinesId = ''; }

        let url = `${this.api}/UpdateInvTransLoc`;
        return this.httpClient.get<changeLocSearchResult[]>(url);
    }

    UpdateChangeLoc(req: changeLocSearchResult) {
        if (req.itemnumber == "null") { req.itemnumber = ''; }
        if (req.soMain == "null") { req.soMain = ''; }
        if (req.soLinesId == "null") { req.soLinesId = ''; }

        let url = `${this.api}/UpdateChangeLoc?somain=${req.soMain}&itemnumber=${req.itemnumber}&userId="admin"&soLinesId=${req.soLinesId}&invType=${req.invType}`;
        return this.httpClient.get<changeLocSearchResult[]>(url);
    }

}