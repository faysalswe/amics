import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";

@Component({
  selector: "app-footer",
  templateUrl: "./app-footer.component.html",
  styleUrls: ["./app-footer.component.scss"],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppFooterComponent implements OnInit {
  year = 0;
  emailLinkText = "AMICS Services";
  emailDistributionList = "hr@amics.net";
  emailSubject = "Questions about the AMICS Website";
  emailHref: string;
  constructor() {}

  ngOnInit() {
    const today = new Date();
    this.year = today.getFullYear();
    this.emailHref = `mailto:${
      this.emailDistributionList
    }?subject=${this.emailSubject.replace(" ", "%20")}`;
  }
}
