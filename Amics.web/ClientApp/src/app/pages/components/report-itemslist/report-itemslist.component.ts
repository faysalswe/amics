import { Component, OnInit, Inject, ViewEncapsulation } from '@angular/core';

 
@Component({
  selector: 'app-report-itemslist',
  encapsulation: ViewEncapsulation.None,
  templateUrl: './report-itemslist.component.html',
  styleUrls: [
    "../../../../../node_modules/jquery-ui/themes/base/all.css",
    "../../../../../node_modules/devextreme/dist/css/dx.common.css",
    "../../../../../node_modules/devextreme/dist/css/dx.light.css",
    "../../../../../node_modules/@devexpress/analytics-core/dist/css/dx-analytics.common.css",
    "../../../../../node_modules/@devexpress/analytics-core/dist/css/dx-analytics.light.css",
    "../../../../../node_modules/devexpress-reporting/dist/css/dx-webdocumentviewer.css"


  ]
})

export class ReportItemslistComponent implements OnInit {

  reportUrl: string = "Translog";
  invokeAction: string = '/DXXRDV';

  constructor(@Inject('BASE_URL') public hostUrl: string) {
    console.log(hostUrl);
  }

  ngOnInit(): void {

    console.log('ngOnInit ngOnInit');

  }

}
