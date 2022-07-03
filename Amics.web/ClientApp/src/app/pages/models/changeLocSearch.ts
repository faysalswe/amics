export interface ChangeLocSearch{
    name: string;
    description: string;
}

export class ChangeLocSearchDto{
    projectId: string = "";
    projectName: string = "";
    er: string = "";
    budget: string = "";
    user: string = "Amicsmaster2";

    constructor(){}
}