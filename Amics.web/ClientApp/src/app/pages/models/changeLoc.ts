export class changeLocRequest
{
    projectId: string ='';
    projectName:string='';
    er:string='';
    budget:string='';
    user:string='';
}

export class changeLocProjDetails{
    projectName:string='';
    warehouse:string='';
    location:string='';
}
export interface changeLocSearchResult
{
     project :string;
     name  :string;
     soMain  :string;
     lineNum :number;
     soLinesId  :string;
     itemnumber  :string;
     description  :string;
     itemType  :string;
     quantity  :string;
     itemsId  :string;
     invType  :string;
     user  :string;
     warehouse  :string;
     location  :string;
     serNo  :string;
     tagNo  :string;
     invSerialId  :string;
     invBasicId  :string;

}