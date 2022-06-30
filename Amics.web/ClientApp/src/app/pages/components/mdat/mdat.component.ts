import { Component, OnInit } from '@angular/core';
import { pmDetails } from '../../models/pmdetails';

@Component({
  selector: 'app-mdat',
  templateUrl: './mdat.component.html',
  styleUrls: ['./mdat.component.scss']
})
export class MdatComponent implements OnInit {

  customers = [];
  pmSearchResults = [];
  warehouseList = [];
  mdatList = [];
  now: Date = new Date();
  constructor() { }

  ngOnInit(): void {
  }

  onSelectionChanged($event:any){
    
  }

}
