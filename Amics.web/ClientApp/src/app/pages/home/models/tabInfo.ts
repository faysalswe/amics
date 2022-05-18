export class TabInfo {
     title: string;
     component: string;
     selector: string;    
     
     constructor(title:string, component:string, selector:string)
     {
         this.title = title;
         this.component = component;
         this.selector = selector;
     }
  }