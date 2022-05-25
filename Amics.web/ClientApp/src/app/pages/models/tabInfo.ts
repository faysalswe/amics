import { ComponentType } from "./componentType";

export class TabInfo {
     title: string;
     component: string;
     selector: string;    
     type: ComponentType;
     
     constructor(title:string, component:string, selector:string, type:ComponentType)
     {
         this.title = title;
         this.component = component;
         this.selector = selector;
         this.type = type;
     }
  }