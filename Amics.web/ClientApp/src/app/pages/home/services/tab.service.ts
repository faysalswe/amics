import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { TabInfo } from "../models/tabInfo";

@Injectable({
    providedIn: "root",
  })
  export class TabService {
    tabs: TabInfo[] = [];

    private addTabSource = new Subject();
    addTabObservable$ = this.addTabSource.asObservable();

    private removeTabSource = new Subject();
    removeTabObservable$ = this.removeTabSource.asObservable();
    
    addTab(title:string, component:string, selector:string)
    {
        const tab = new TabInfo(title, component, selector);    
        this.tabs.push(tab);
        console.log(`added Tab with title ${title}`)
        this.addTabSource.next(tab);
    }

    removeTab(tabToRemove : TabInfo)
    {    
        const index = this.tabs.indexOf(tabToRemove);
        this.tabs.splice(index, 1);
        console.log(`removed Tab with title ${tabToRemove.title}`) 
        this.removeTabSource.next(tabToRemove);
    }
  }