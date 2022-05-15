import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

@Component({
  selector: "app-contactUs",
  templateUrl: "./contact-us.component.html",
})
export class ContactUSComponent implements OnInit {
  constructor(private router: Router) {}

  ngOnInit() {
    window.open("https://amics.net/contact-us", "_blank");
    this.router.navigateByUrl("/");
  }
}
