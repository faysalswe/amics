import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import notify from 'devextreme/ui/notify';
import { LabelMap } from 'src/app/pages/models/Label';
import { changeSerial, pmSerial } from 'src/app/pages/models/pmSerial';
import { PartMasterService } from 'src/app/pages/services/partmaster.service';
import { TextboxStyle } from '../../textbox-style/textbox-style';

@Component({
  selector: 'app-viewSerial',
  templateUrl: './viewSerial.component.html',
  styleUrls: ['./viewSerial.component.scss']
})
export class ViewSerialComponent implements OnInit {

  @Output() viewSerialPopup: EventEmitter<any> = new EventEmitter<any>();
  @Input() PMDetailId:string = "";

  secUserId = 'E02310D5-227F-4DB8-8B42-C6AE3A3CB60B';
  StylingMode: string = TextboxStyle.StylingMode;
  LabelMode: string = TextboxStyle.LabelMode;
  pmSerials: pmSerial[] = [];
  labelMap: typeof LabelMap;
  changeSerialSearchInfo: changeSerial = new changeSerial();
  updateSerialPopupVisible: boolean = false;
  submitSerialPopupButtonOptions = {
    text: "Save and exit",
    useSubmitBehavior: true,
    type: "default"
  };
  vsVisibility:boolean = true;

  constructor(private pmService: PartMasterService) 
  {
    this.labelMap = LabelMap;
   }
  

  HidingVS(){
    this.viewSerialPopup.emit();
  }

  ngOnInit() {
    this.getSerial();
  }

  getSerial() {
    debugger
    this.pmService
      .getViewSerial(this.PMDetailId, this.secUserId)
      .subscribe((x) => (this.pmSerials = x));
  }

  onRowSelection(e: any) {
    let selectedRow = e.data;
    this.changeSerialSearchInfo.serNoFm = selectedRow?.serlot;
    this.changeSerialSearchInfo.serNoTo = selectedRow?.serlot;
    this.changeSerialSearchInfo.tagNoFm = selectedRow?.tagcol;
    this.changeSerialSearchInfo.tagNoTo = selectedRow?.tagcol;
    this.changeSerialSearchInfo.modelFm = selectedRow?.color_model;
    this.changeSerialSearchInfo.modelTo = selectedRow?.color_model;
    this.changeSerialSearchInfo.costFm = selectedRow?.cost;
    this.changeSerialSearchInfo.costTo = selectedRow?.cost;

    this.changeSerialSearchInfo.serialId = selectedRow?.id;
  }

  edit() {
    this.updateSerialPopupVisible = true;
  }

  saveSerial(e: any){
    this.changeSerialSearchInfo.costFm = this.changeSerialSearchInfo.costFm.toString();
    this.changeSerialSearchInfo.costTo = this.changeSerialSearchInfo.costTo.toString();

    this.pmService.updateChangeSerialTag(this.changeSerialSearchInfo)
        .subscribe((res: any) => {
          this.updateSerialPopupVisible = false;
          this.getSerial();
        }, 
        err => {
          notify({ message: "Error occured during update serial", shading: true, position: top }, "error", 1500) 
        
        });

    e.preventDefault();
  }

  cancelSerial(){
    this.updateSerialPopupVisible = false;
  }

}
