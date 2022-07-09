import { HttpClient } from '@angular/common/http';
import { ElementRef } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  NgControl,
  ValidationErrors,
  ValidatorFn,
} from '@angular/forms';
import { map, tap } from 'rxjs';
import { DuplicateSerTagErrorMsgService } from './duplicate.sertag.msg.service';
import { SerTagValidateInt } from '../models/rest.api.interface.model';
import { ValidationService } from '../services/validation.service';

export class DuplicateSerTagCheck {
  constructor(private elementRef: ElementRef) {}
  static validate(
    controls: FormArray,
    dupsertagerrormsg: DuplicateSerTagErrorMsgService,
    validationService: ValidationService,
    itemsid: string,
    sertag: string
  ): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      if (control.value != null) {
        console.log(control);
        var tmpValues =
          sertag == 'SERIAL'
            ? controls.controls.map((obj) => obj.get('serNo'))
            : controls.controls.map((obj) => obj.get('tagNo'));
        var finalVals = tmpValues.filter((res) => res?.value == control.value);
        if (
          finalVals.length > 1 &&
          finalVals[0]?.value != null &&
          finalVals[0]?.value != ''
        ) {
          DuplicateSerTagCheck.reset(
            dupsertagerrormsg,
            finalVals,
            sertag,
            control
          );
        } else {
          validationService
            .validateSerTag(
              itemsid,
              control.value,
              sertag === 'SERIAL' ? 'SERIAL' : 'TAG'
            )
            .pipe(
              tap((obj) => console.log(obj)),
              map((obj: SerTagValidateInt) => {
                if (obj === null) {
                } else {
                  if (Object.keys(obj).length > 0) {
                    DuplicateSerTagCheck.reset(
                      dupsertagerrormsg,
                      finalVals,
                      sertag,
                      control
                    );
                  }
                }
              })
            )
            .subscribe();
        }
      }
      return null;
    };
  }

  private static reset(
    dupsertagerrormsg: DuplicateSerTagErrorMsgService,
    finalVals: (AbstractControl | null)[],
    sertag: string,
    c: AbstractControl
  ) {
    console.log('Duplicate Found');
    if (finalVals[0]?.value != null && finalVals[0]?.value != '') {
      dupsertagerrormsg.add(
        `Found Duplicate in client - ${finalVals[0]?.value}`
      );
      if (sertag === 'SERIAL') {
        setTimeout(() => {
          document
            .getElementById(c.parent?.get('serElementId')?.value)
            ?.focus();
        }, 0);
      }

      if (sertag === 'TAG') {
        setTimeout(() => {
          document
            .getElementById(c.parent?.get('tagElementId')?.value)
            ?.focus();
        }, 0);
      }

      c.setValue(null);
    }
  }
}
