import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';   
import { Router } from '@angular/router';
import { LoginService } from 'src/core/services/login.service';
@Component({
  selector: 'amics-branding',
  templateUrl: './amics-branding.component.html',
  styleUrls: ['./amics-branding.component.scss']
})
export class AmicsBrandingComponent implements OnInit { 
  @Input() user?: string; 
  @Input() showTestWarning?: boolean = false;

  constructor(private readonly loginService: LoginService, private router: Router) {}

  ngOnInit() {}

  logout() {
    this.loginService.logout();
  }

  login() {
    this.router.navigateByUrl('/login');
    // this.loginService.login();
  }
}
