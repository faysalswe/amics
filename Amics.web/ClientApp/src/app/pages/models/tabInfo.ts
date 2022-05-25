import { ComponentType } from "./componentType";

export class TabInfo {
     title: string; 
     type: ComponentType;
     
     constructor(title:string, type:ComponentType)
     {
         this.title = title; 
         this.type = type;
     }
  }