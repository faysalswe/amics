import {
  AfterViewInit,
  ChangeDetectorRef,
  Directive,
  ElementRef,
  Input,
  OnChanges,
  SimpleChanges,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { AppInitialDataService } from '../services/app.initial.data.service';
import SelectBox from 'devextreme/ui/select_box';
import TextBox from 'devextreme/ui/text_box';
import NumberBox from 'devextreme/ui/number_box';
import DateBox from 'devextreme/ui/date_box';

@Directive({
  selector: '[ngDisplayLabel]',
})
export class DisplayLabelDirective {
  // @Input('ngDisplayLabel') optionId: number = 0;

  constructor(
    private elementRef: ElementRef,
    private appInitDataService: AppInitialDataService,
    private vcr: ViewContainerRef,
    private tempRef: TemplateRef<any>
  ) {}

  /*ngOnChanges(changes: SimpleChanges): void {
    var data = this.appInitDataService.getCompanyOptions();
    var result = new Map(data.map((i) => [i.optionId, i.yesOrNo]));
    var displayYesOrNo = result.get(this.optionId);
    console.log(displayYesOrNo);
    if (displayYesOrNo) {
      this.vcr.createEmbeddedView(this.tempRef);
    } else {
      this.vcr.clear();
    }
    throw new Error('Method not implemented.');
  }*/

  @Input('ngDisplayLabel') set ngDisplayLabel(optionId: number) {
    console.log(optionId);
    var data = this.appInitDataService.getCompanyOptions();
    var result = new Map(data.map((i) => [i.optionId, i.yesOrNo]));
    var displayYesOrNo = result.get(optionId);
    console.log(displayYesOrNo);
    if (displayYesOrNo || (displayYesOrNo === undefined)) {
      this.vcr.createEmbeddedView(this.tempRef);
    } else {
      this.vcr.clear();
    }
  }
}
