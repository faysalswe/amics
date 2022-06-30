import { Injectable } from '@angular/core';
import {
  CompanyOptionsInt,
  LabelInt,
} from '../models/rest.api.interface.model';
import { ConfigService } from 'src/app/pages/services/config.service';

@Injectable({
  providedIn: 'root',
})

export class AppInitialDataService {
  labels: LabelInt[] = [];
  companyOptions: CompanyOptionsInt[] = [];

  constructor(private configService: ConfigService) {}

  loadData() {
    this.configService.getLabels().subscribe((l) => (this.labels = l));
    this.configService
      .getCompanyOptions()
      .subscribe((l) => (this.companyOptions = l));
  }

  getLabel(): LabelInt[] {
    return this.labels;
  }

  getCompanyOptions(): CompanyOptionsInt[] {
    return this.companyOptions;
  }
}
