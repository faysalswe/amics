import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-mdat',
  templateUrl: './mdat.component.html',
  styleUrls: ['./mdat.component.scss']
})
export class MdatComponent implements OnInit {

  customers = [];
  pmSearchResults = [];
  now: Date = new Date();
  constructor() { }

  ngOnInit(): void {
  }

  onSelectionChanged($event:any){
    
  }

}
