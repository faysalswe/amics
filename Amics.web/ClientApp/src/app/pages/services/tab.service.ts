import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { ComponentType } from "../models/componentType";
import { TabInfo } from "../models/tabInfo";

@Injectable({
  providedIn: "root",
})
export class TabService {
  tabs: TabInfo[] = [];

  private addTabSource = new Subject();
  addTabObservable$ = this.addTabSource.asObservable();

  private showTabSource = new Subject();
  showTabObservable$ = this.showTabSource.asObservable();

  private removeTabSource = new Subject();
  removeTabObservable$ = this.removeTabSource.asObservable();

  addTab(title: string, component: string, selector: string, type: ComponentType) {

    const index = this.tabs.findIndex(t => t.title == title);
    if (index === -1) {
      const tab = new TabInfo(title, type);
      this.tabs.push(tab);
      console.log(`added Tab with title ${title}`)
      this.addTabSource.next(tab);
    } else {
      this.showTabSource.next(title);
    }
  }

  removeTab(tabToRemove: TabInfo) {
    const index = this.tabs.indexOf(tabToRemove);
    this.tabs.splice(index, 1);
    console.log(`removed Tab with title ${tabToRemove.title}`)
    this.removeTabSource.next(tabToRemove);
  } 

}