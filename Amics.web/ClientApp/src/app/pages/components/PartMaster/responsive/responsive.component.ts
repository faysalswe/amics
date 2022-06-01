import { Component } from "@angular/core";

@Component({
    selector: "app-responsive",
    templateUrl: "./responsive.component.html",
    styleUrls: ['./responsive.component.scss']
})
export class ResponsiveComponent {
    screenFn (width: number): "sm" | "lg" {
        return (width < 700) ? 'sm' : 'lg';
      }
}