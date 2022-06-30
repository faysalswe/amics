import { Component, Input, OnInit } from '@angular/core';
import { TransLogInt } from 'src/app/shared/models/rest.api.interface.model';

@Component({
  selector: 'app-trans-log',
  templateUrl: './trans-log.component.html',
  styleUrls: ['./trans-log.component.scss']
})
export class TransLogComponent implements OnInit {

  @Input()
  data: TransLogInt[] = [];

  constructor() { }

  ngOnInit(): void {
  }

}
