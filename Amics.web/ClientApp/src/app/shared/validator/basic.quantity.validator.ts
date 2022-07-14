import { AbstractControl, ValidatorFn } from '@angular/forms';
import { DuplicateSerTagErrorMsgService } from './duplicate.sertag.msg.service';

export class BasicQuantityValidator {
  static validate(
    dupsertagerrormsg: DuplicateSerTagErrorMsgService
  ): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      if (control.value != null) {
        let decreasedQuan = control.value;
        let currentQuan = control?.parent?.get('quantity')?.value;
        if (decreasedQuan > currentQuan) {
          dupsertagerrormsg.add('Decreased Quantity should be less than or equal to current quantity');
        }
        return null;
      }
      return null;
    };
  }
}
