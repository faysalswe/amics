import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-bulk-transfer',
  templateUrl: './bulk-transfer.component.html',
  styleUrls: ['./bulk-transfer.component.scss']
})
export class BulkTransferComponent implements OnInit {

  fromWarehouseList = [];
  fromLocationList = [];
  toWarehouseList = [];
  toLocationList = [];

  bulkGridList = [];

  constructor() { }

  ngOnInit(): void {
  }

}
