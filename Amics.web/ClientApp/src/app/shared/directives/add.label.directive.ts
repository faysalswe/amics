import {AfterViewInit, ChangeDetectorRef, Directive, ElementRef, Input} from "@angular/core";
import {AppInitialDataService} from "../services/app.initial.data.service"; 
import SelectBox from "devextreme/ui/select_box";
import TextBox from "devextreme/ui/text_box";
import NumberBox from "devextreme/ui/number_box";
import DateBox from "devextreme/ui/date_box";

@Directive({
  selector: "[ngAddLabel]",
})
export class AddLabelDirective implements AfterViewInit {
  @Input("ngAddLabel") labelNumber: number = 0;

  constructor(private elementRef: ElementRef,
              private appInitDataService: AppInitialDataService,
              private ref: ChangeDetectorRef) {

  }

  ngAfterViewInit() {
    var data = this.appInitDataService.getData();
    var result = new Map(data.map(i => [i.labelNumber, i.myLabel]));
    var myLabel = result.get(this.labelNumber);

    if (myLabel !== null && myLabel !== 'undefined') {
      console.log(this.elementRef.nativeElement.tagName);
      if (this.elementRef.nativeElement.tagName === "DX-SELECT-BOX") {
        let selectBox = SelectBox.getInstance(this.elementRef.nativeElement) as SelectBox;
        selectBox.option("label", myLabel);
      }

      if (this.elementRef.nativeElement.tagName === "DX-TEXT-BOX") {
        let textBox = TextBox.getInstance(this.elementRef.nativeElement) as TextBox;
        textBox.option("label", myLabel);
      }

      if (this.elementRef.nativeElement.tagName === "DX-NUMBER-BOX") {
        let numBox = NumberBox.getInstance(this.elementRef.nativeElement) as NumberBox;
        numBox.option("label", myLabel);
      }

      if (this.elementRef.nativeElement.tagName === "DX-DATE-BOX") {
        let dateBox = DateBox.getInstance(this.elementRef.nativeElement) as DateBox;
        dateBox.option("label", myLabel);
      }

    }

  }
}
