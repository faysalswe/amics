export class ContractSearch {    
    contractno: string = '';
    project: string = '';    
}
export class ContractSearchResult {
    id: string = '';
    contractno: string = '';
    contractname: string = '';
    markup1: number = 0;
    markup2: number = 0;
    user: string = '';
    actionflag : number = 1;
}

export class Project{
    id: string = '';
    actionflag: number = 1;
    projectid: string = '';
    name: string = '';
    createdby: string = '';
    contractid: string = '';
    user: string = '';   
}