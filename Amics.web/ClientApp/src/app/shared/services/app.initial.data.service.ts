import { Injectable } from '@angular/core';
import {
  CompanyOptionsInt,
  LabelInt,
} from '../models/rest.api.interface.model';
import { SearchService } from 'src/app/pages/services/search.service';

@Injectable({
  providedIn: 'root',
})

export class AppInitialDataService {
  labels: LabelInt[] = [];
  companyOptions: CompanyOptionsInt[] = [];

  constructor(private searchService: SearchService) {}

  loadData() {
    this.searchService.getLabels().subscribe((l) => (this.labels = l));
    this.searchService
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
