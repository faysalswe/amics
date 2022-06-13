import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Company } from '../models/company';
import { pmBomDetails } from '../models/pmBomDetails';
import { pmDetails } from '../models/pmdetails';
import { pmPoDetails } from '../models/pmPoDetails';
import { SearchService } from './search.service';

const companies: Company[] = [{
  ID: 1,
  Name: 'Super Mart of the West',
  Address: '702 SW 8th Street',
  City: 'Bentonville',
  State: 'Arkansas',
  ZipCode: 72716,
  Phone: '(800) 555-2797',
  Fax: '(800) 555-2171',
  Website: '',
  Active: true,
}, {
  ID: 2,
  Name: 'Electronics Depot',
  Address: '2455 Paces Ferry Road NW',
  City: 'Atlanta',
  State: 'Georgia',
  ZipCode: 30339,
  Phone: '(800) 595-3232',
  Fax: '(800) 595-3231',
  Website: '',
  Active: true,
}, {
  ID: 3,
  Name: 'K&S Music',
  Address: '1000 Nicllet Mall',
  City: 'Minneapolis',
  State: 'Minnesota',
  ZipCode: 55403,
  Phone: '(612) 304-6073',
  Fax: '(612) 304-6074',
  Website: '',
  Active: true,
}, {
  ID: 4,
  Name: "Tom's Club",
  Address: '999 Lake Drive',
  City: 'Issaquah',
  State: 'Washington',
  ZipCode: 98027,
  Phone: '(800) 955-2292',
  Fax: '(800) 955-2293',
  Website: '',
  Active: true,
}];

@Injectable({
  providedIn: "root",
})
export class PartMasterService {

  private readonly api = '{apiUrl}/api/partmaster';

  constructor(private readonly httpClient: HttpClient, private readonly searchService: SearchService) { }
  getCompanies(): Company[] {
    return companies;
  }

  getPartMaster(itemNumber: string, rev: string) {
    return this.httpClient.get<pmDetails>(`${this.api}?itemnumber=${itemNumber}&rev=${rev}`);
  }

  getBomDetails(itemId: Guid) {
    return this.httpClient.get<pmBomDetails[]>(`${this.api}/BomDetails?itemsId=${itemId}`);
  }
  getPoDetails(itemId: Guid) {
    return this.httpClient.get<pmPoDetails[]>(`${this.api}/PODetails?itemsId=${itemId}`);
  }

  AddorUpdatePMDetails(item: pmDetails, uomId: Guid) {
    item.uomid = uomId;
    return this.httpClient.post<string>(this.api, item);
  }

  DeletePM(itemNumber: string, rev: string) {
    return this.httpClient.delete<string>(`${this.api}?itemnumber=${itemNumber}&rev=${rev}`);
  }
}