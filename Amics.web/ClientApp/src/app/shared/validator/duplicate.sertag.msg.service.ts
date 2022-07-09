import { Injectable } from '@angular/core';
import { BehaviorSubject, filter } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DuplicateSerTagErrorMsgService {
  private duplicateSerTagMessageSub$ = new BehaviorSubject<string | null>(null);
  duplicateSerTagMessageObs$ = this.duplicateSerTagMessageSub$.asObservable();

  constructor() {
    /* this.duplicateSerTagMessageObs$.subscribe(val => {
      console.log(val);
    }) */
  }

  public add(error: string) {
    console.log(error);
    this.duplicateSerTagMessageSub$.next(error);
  }
}
