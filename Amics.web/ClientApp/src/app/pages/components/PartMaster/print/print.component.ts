import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-print',
  templateUrl: './print.component.html',
  styleUrls: ['./print.component.scss']
})
export class PrintComponent implements OnInit {

  submitButtonOptions = {
    text: "Partmaster and BOM",
    useSubmitBehavior: true,
    width: "100%",
    type: "default",
};


  @Output() viewPrintPopup: EventEmitter<any> = new EventEmitter<any>();
  @Output() viewReport: EventEmitter<any> = new EventEmitter<any>();

  printPopup: boolean = true;

  constructor() { }

  ngOnInit() {
  }

  HidingPrint(){
    this.viewPrintPopup.emit();
  }

  populateReport(key:string){
    this.viewReport.emit(key);
  }
}
