import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { TransLogInt } from 'src/app/shared/models/rest.api.interface.model';

@Component({
  selector: 'app-trans-log-sub-details',
  templateUrl: './trans-log-sub-details.component.html',
  styleUrls: ['./trans-log-sub-details.component.scss']
})
export class TransLogSubDetailsComponent implements OnInit, AfterViewInit {

  @Input('transLogInt') trasLog!: TransLogInt;
  trasLogArray: TransLogInt[] = [];

  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    console.log(this.trasLog);
    console.log(this.trasLogArray);
    this.trasLogArray.push(this.trasLog);
  }

}
