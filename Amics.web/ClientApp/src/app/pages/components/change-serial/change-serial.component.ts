import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-change-serial',
  templateUrl: './change-serial.component.html',
  styleUrls: ['./change-serial.component.scss']
})
export class ChangeSerialComponent implements OnInit {
  changesserialArray = [];
  changeserialOptions = [
    'Ware house',
    'Location',
    'Cost',
    'Serial',
    'Tag No',
    'Model',
    'Exp Date',
    'Original year',
    'Current year'
  ];
  constructor() { }

  ngOnInit(): void {
  }
  
}
