import { Component, enableProdMode, OnInit } from '@angular/core';
import { TabInfo } from '../../models/tabInfo';
import { AppTask, Employee, HomeService } from '../../services/home.service'; 
import { TabService } from '../../services/tab.service';
 
@Component({
  templateUrl: 'home.component.html',
  styleUrls: [ './home.component.scss' ],
  preserveWhitespaces: true,
})

export class HomeComponent implements OnInit{
  allEmployees: Employee[];

  employees: Employee[];

  selectedIndex: number;

  tasks: AppTask[];

  tasksDataSourceStorage: any;

  tabs: TabInfo[] = [];
  
  constructor(private homeService: HomeService, private tabService: TabService) {
    this.allEmployees = homeService.getEmployees();
    this.employees = homeService.getEmployees().slice(0, 3);
    this.selectedIndex = 0;
    this.tasks = homeService.getTasks();
    this.tasksDataSourceStorage = [];
    this.tabs = [];
  }

  ngOnInit(): void{
    this.tabService.addTabObservable$.subscribe((tab: any) => { 
      this.tabs.push(tab);
    });
    this.tabService.removeTabObservable$.subscribe((tabToRemove: any) => { 
      const index = this.tabs.indexOf(tabToRemove);
      this.tabs.splice(index, 1);
    });
  }

  onTabDragStart(e:any) {
    e.itemData = e.fromData[e.fromIndex];
  }

  onTabDrop(e:any) {
    e.fromData.splice(e.fromIndex, 1);
    e.toData.splice(e.toIndex, 0, e.itemData);
  }

  addButtonHandler() {
    const newItem = this.allEmployees.filter((employee) => this.employees.indexOf(employee) === -1)[0];

    this.selectedIndex = this.employees.length;
    this.employees.push(newItem);
  }

  closeButtonHandler(itemData:any) {
    // const index = this.employees.indexOf(itemData);

    // this.employees.splice(index, 1);
    // if (index >= this.employees.length && index > 0) this.selectedIndex = index - 1;
    
    this.tabService.removeTab(itemData);
  }

  showCloseButton() {
    return this.employees.length > 1;
  }

  disableButton() {
    return this.employees.length === this.allEmployees.length;
  }

  getTasks(id:any) {
    let item = this.tasksDataSourceStorage.find((i:any) => i.key === id);
    if (!item) {
      item = {
        key: id,
        dataSourceInstance: this.tasks.filter((task) => task.EmployeeID === id),
      };
      this.tasksDataSourceStorage.push(item);
    }

    return item.dataSourceInstance;
  }

  getCompletedTasks(id:any) {
    return this.tasks.filter((task) => task.EmployeeID === id).filter((task) => task.Status === 'Completed');
  }
}
  