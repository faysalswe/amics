import { Component, OnInit } from '@angular/core';
import { LabelMap } from '../../models/Label';
import { TextboxStyle } from '../textbox-style/textbox-style';

@Component({
  selector: 'app-equipment',
  templateUrl: './equipment.component.html',
  styleUrls: ['./equipment.component.scss']
})
export class EquipmentComponent implements OnInit {

  private readonly newProperty = './equipment.component.html';

  statusList = [];
  equipSearchList = [];
  equipMainList = [];
  searchingYear = [
    'This',
    'Last',
    'All',
  ];
  equipNotes = [
    'Task Notes',
    'PO Notes',
    'PO Rec Notes',
  ];
  equipradio = [
    'Line items',
    'Documents',
    'Purchase Order',
    'shipments'
  ];

  notesHeader: string = "Task Notes";
  StylingMode: string = TextboxStyle.StylingMode;
  LabelMode: string = TextboxStyle.LabelMode;
  labelMap: typeof LabelMap;
  constructor() { 
    this.labelMap = LabelMap;
  }

  ngOnInit(): void {
  }

  changeNotesHeader(event: any)
  {
    this.notesHeader = event.value;
  }

}
