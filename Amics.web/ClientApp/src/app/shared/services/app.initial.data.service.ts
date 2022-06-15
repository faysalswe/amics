import {Injectable} from "@angular/core"; 
import {LabelInt} from "../models/rest.api.interface.model";
import { SearchService } from "src/app/pages/services/search.service"; 

@Injectable({
    providedIn: "root",
  })
  
export class AppInitialDataService {

  labels: LabelInt[] = []; 
  constructor(private searchService : SearchService) {
  }

  loadData() {
    this.searchService.getLabels().subscribe(l=> this.labels = l); 
  }

  getData(): LabelInt[] {
    return this.labels;
  }
}
