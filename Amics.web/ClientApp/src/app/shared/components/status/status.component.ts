import { Component, OnInit } from '@angular/core';
import notify from 'devextreme/ui/notify';
import { filter, Observable, Subscription, tap } from 'rxjs';
import { DuplicateSerTagErrorMsgService } from '../../validator/duplicate.sertag.msg.service';

@Component({
  selector: 'app-status',
  templateUrl: './status.component.html',
  styleUrls: ['./status.component.scss'],
})
export class StatusComponent implements OnInit {
  constructor(public duplicateMsgService: DuplicateSerTagErrorMsgService) {}
  ngOnInit(): void {
    let obs$ = this.duplicateMsgService.duplicateSerTagMessageObs$;
    obs$.pipe(
      tap((res) => console.log(res)),
      filter((res) => res != null)
    );
    obs$.subscribe((msg) => {
      if (msg != null && msg != '') {
        setTimeout(() => {
          notify(
            {
              position: {
                at: 'top center',
                my: 'top center',
                offset: '0 0',
              },
              of: '.dx-overlay-content',
              message: msg,
            },
            'info',
            800
          );
        }, 100);
      }
    });
  }
}
