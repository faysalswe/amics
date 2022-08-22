import { Component, Inject,OnInit,  ViewEncapsulation, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DxReportViewerComponent } from 'devexpress-reporting-angular';

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

  @ViewChild(DxReportViewerComponent, { static: false }) viewer: DxReportViewerComponent;

  @ViewChild('paramValue', { static: false }) public paramValue: ElementRef;

  reportUrl: string;
  invokeAction: string = '/DXXRDV';
  Param: string;

  //submitParameter() {
  //  var parameterValue = this.paramValue.nativeElement.value;
  //   this.viewer.bindingSender.OpenReport("ListItemsRpt" + "?parameter1=" + parameterValue + "&parameter2=et" + "&header1=H Item Number" + "&header2=H Description");

  //}

  ngOnInit(): void {

     

        //this.Param = "?item=009";
        //this.Param += "&rev=-";
        //this.Param += "&description=";
        //this.Param += "&itemtype=";
        //this.Param += "&itemclass=";
        //this.Param += "&itemcode=";
        //this.Param += "&warehouse=";
        //this.Param += "&location=";
        //this.Param += "&user1=";
        //this.Param += "&user2=";
        //this.Param += "&user3=";
        //this.Param += "&user4=";
        //this.Param += "&user5=";
        //this.Param += "&user6=";
        //this.Param += "&user7=";
        //this.Param += "&user8=";
        //this.reportUrl = "ListItemsCS" + this.Param;


    this.Param = "?somain=ertm02262-1"; //itemCode_num  "labelMap.partNumber_num"
    this.Param += "&header_somain=ER : ertm02262-1";
    this.reportUrl = "erinv" + this.Param;
   
  }

 

  constructor(@Inject('BASE_URL') public hostUrl: string) {
    //this.viewer.bindingSender.OpenReport(this.reportUrl + "?itemnumber=" + "0092");
  }


  OnParametersInitialized(event) {

   //this.viewer.bindingSender.OpenReport(this.reportUrl + "?itemnumber=" + "0092");

   // event.args.Submit();

    // Specify an invisible integer parameter's value on viewer initialization.
    //var invisibleIntParamValue = 42;
    //var intParam = event.args.ActualParametersInfo.filter(
    //  x => x.parameterDescriptor.name == "intParam")[0];
    //intParam.value(invisibleIntParamValue);

    //// Specify a visible Boolean parameter's value on viewer initialization.
    //var visibleBooleanParamValue = true;
    //var booleanParam = event.args.ActualParametersInfo.filter(
    //  x => x.parameterDescriptor.name == "booleanParam")[0];
    //booleanParam.value(visibleBooleanParamValue);

    //// Update a string parameter value when a user changes the Boolean parameter value.
    //var strParam = event.args.ActualParametersInfo.filter(
    //  x => x.parameterDescriptor.name == "itemnumber")[0];

    //booleanParam && booleanParam.value.subscribe(function (newVal) {
    //  strParam.value(newVal.toString());
    //});

    //intParam & booleanParam & strParam &&

  }

   
}

//export class ReportItemslistComponent implements OnInit {

//  reportUrl: string = "ListItems";
//  invokeAction: string = '/DXXRDV';

//  constructor(@Inject('BASE_URL') public hostUrl: string) {
//    console.log(hostUrl);
//  }

//  ngOnInit(): void {

//    console.log('ngOnInit ngOnInit');

//  }

//  OnParametersInitialized(event) {

//    // Specify an invisible integer parameter's value on viewer initialization.
//    var invisibleIntParamValue = 42;
//    var intParam = event.args.ActualParametersInfo.filter(
//      x => x.parameterDescriptor.name == "intParam")[0];
//    intParam.value(invisibleIntParamValue);

//    // Specify a visible Boolean parameter's value on viewer initialization.
//    var visibleBooleanParamValue = true;
//    var booleanParam = event.args.ActualParametersInfo.filter(
//      x => x.parameterDescriptor.name == "booleanParam")[0];
//    booleanParam.value(visibleBooleanParamValue);

//    // Update a string parameter value when a user changes the Boolean parameter value.
//    var strParam = event.args.ActualParametersInfo.filter(
//      x => x.parameterDescriptor.name == "strParam")[0];

//    booleanParam && booleanParam.value.subscribe(function (newVal) {
//      strParam.value(newVal.toString());
//    });

//    intParam & booleanParam & strParam && event.args.Submit();
//  }


//}
