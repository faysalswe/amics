import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-serial-documents',
  templateUrl: './serial-documents.component.html',
  styleUrls: ['./serial-documents.component.scss']
})
export class SerialDocumentsComponent implements OnInit {
  serialDocArray = [];
  serialActionArray = [];
  serialDocOptions = [
    'Part Number',
    'Serial #',
    'Tag #',
    'Description',
  ];
  searchLabel: string = 'Part Number';

  constructor() { }

  ngOnInit(): void {
  }

  onValueChanged($event: any) {
    this.searchLabel = $event.value;
  }

}
