import { Component, OnInit } from '@angular/core';

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
  constructor() { }

  ngOnInit(): void {
  }

  changeNotesHeader(event: any){}

}
