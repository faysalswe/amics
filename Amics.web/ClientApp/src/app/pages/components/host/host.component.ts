import {
    Component,
    OnInit, 
    Input,
    ComponentFactoryResolver,
    ViewChild,
    ViewContainerRef,
    SimpleChanges,
} from '@angular/core';
import { ComponentType } from "../../models/componentType";
import { PartMasterComponent } from "../PartMaster/partmaster.component";
import { ProfileComponent } from "../profile/profile.component";


@Component({
    selector: "app-host",
    templateUrl: "./host.component.html",
  })
export class HostComponent implements OnInit {
    @Input() type: ComponentType = ComponentType.PartMaster;
    @ViewChild('vrf', { read: ViewContainerRef })
    vrf!: ViewContainerRef;

    constructor(private componentFactoryResolver: ComponentFactoryResolver) { }

    ngOnInit(): void {
        console.log(`loading.. ${this.type} `);
        this.loadDynamicComponent(this.type);
        console.log(`done loading.. ${this.type} `);
     } 

    loadDynamicComponent(type: ComponentType) {
        let component: any = PartMasterComponent;
        switch (type) {
            case ComponentType.PartMaster:
                component = PartMasterComponent;
                break;
            case ComponentType.ProfileComponent:
                component = ProfileComponent;
                break;
        }
        const componentFactory =
            this.componentFactoryResolver.resolveComponentFactory(component);
        
        this.vrf!.createComponent(componentFactory);
    }
}