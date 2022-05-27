import { Component, enableProdMode, OnInit } from '@angular/core';
import { TabInfo } from '../../models/tabInfo';
import { TabService } from '../../services/tab.service';

@Component({
  templateUrl: 'home.component.html',
  styleUrls: ['./home.component.scss'],
  preserveWhitespaces: true,
})

export class HomeComponent implements OnInit {

  selectedIndex: number;
  tabs: TabInfo[] = [];

  constructor(private tabService: TabService) {
    this.selectedIndex = 0;
    this.tabs = [];
  }

  ngOnInit(): void {
    this.tabService.addTabObservable$.subscribe((tab: any) => {
      this.tabs.push(tab);
      this.selectedIndex = this.tabs.length - 1;
    });
    this.tabService.removeTabObservable$.subscribe((tabToRemove: any) => {
      const index = this.tabs.indexOf(tabToRemove);
      this.tabs.splice(index, 1);
    });
  }

  onTabDragStart(e: any) {
    e.itemData = e.fromData[e.fromIndex];
  }

  onTabDrop(e: any) {
    e.fromData.splice(e.fromIndex, 1);
    e.toData.splice(e.toIndex, 0, e.itemData);
  }

  closeButtonHandler(itemData: any) { 
    this.tabService.removeTab(itemData);
  }

  showCloseButton() {
    return this.tabs.length > 1;
  }


}
