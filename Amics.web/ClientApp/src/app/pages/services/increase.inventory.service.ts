import { Injectable } from '@angular/core';

export interface Company {
  ID: number;

  Name: string;

  Address: string;

  City: string;

  State: string;

  ZipCode: number;

  Phone: string;

  Fax: string;

  Website: string;

  Active: boolean;
}

export interface Employee {
  ID: number;

  FirstName: string;

  LastName: string;

  CompanyName: string;

  Position: string;

  OfficeNo: string;

  BirthDate: Date;

  HireDate: Date;

  Address: string;

  City: string;

  State: string;

  Zipcode: string;

  Phone: string;

  Email: string;

  Skype: string;
}

const employee: Employee = {
  ID: 1,
  FirstName: 'John',
  LastName: 'Heart',
  CompanyName: 'Super Mart of the West',
  Position: 'CEO',
  OfficeNo: '901',
  BirthDate: new Date(1964, 2, 16),
  HireDate: new Date(1995, 0, 15),
  Address: '351 S Hill St.',
  City: 'Los Angeles',
  State: 'CA',
  Zipcode: '90013',
  Phone: '+1(213) 555-9392',
  Email: 'jheart@dx-email.com',
  Skype: 'jheart_DX_skype',
};



const companies: Company[] = [
  {
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
},
  {
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
},
  {
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
},
  {
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


const positions : string[] = [
  'HR Manager',
  'IT Manager',
  'CEO',
  'Controller',
  'Sales Manager',
  'Support Manager',
  'Shipping Manager',
];

@Injectable()
export class IncreaseInventoryService {
  getEmployee() : Employee {
    return employee;
  }

  getCompanies() : Company[] {
    return companies;
  }

  getPositions() : string[] {
    return positions;
  }


}
