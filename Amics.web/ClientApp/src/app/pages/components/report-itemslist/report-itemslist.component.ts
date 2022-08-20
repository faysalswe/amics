import { Component, OnInit, Inject, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

 
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

  constructor(@Inject('BASE_URL') public hostUrl: string, private activatedRoute: ActivatedRoute) {
    console.log(hostUrl);
  }

  ngOnInit(): void {
    let reportName = localStorage.getItem("reportUrl");
    if(reportName != undefined && reportName != ""){
      this.reportUrl = reportName;
    }
    console.log('ngOnInit ngOnInit');

  }

}
