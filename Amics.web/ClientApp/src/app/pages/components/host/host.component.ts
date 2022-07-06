import {
    Component,
    OnInit,
    Input,
    ComponentFactoryResolver,
    ViewChild,
    ViewContainerRef,
    SimpleChanges,
    Output,
    EventEmitter
} from '@angular/core';
import { ComponentType } from "../../models/componentType";
import { PartMasterComponent } from "../PartMaster/partmaster.component";
import { ProfileComponent } from "../profile/profile.component";


@Component({
    selector: "app-host",
    templateUrl: "./host.component.html",
    styleUrls: ['./host.component.scss'],
})
export class HostComponent implements OnInit {
    @Input() type: ComponentType | undefined;
    @Output() action : EventEmitter<any> =new EventEmitter<any>();
    componentType: typeof ComponentType;
    constructor() { this.componentType = ComponentType; }

    ngOnInit(): void {
        console.log(`loading.. ${this.type} `);
    }

    onExit(){
        this.action.emit();
    }

}