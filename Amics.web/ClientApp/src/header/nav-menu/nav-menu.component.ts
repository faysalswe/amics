import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { InternalRoute } from 'src/app/shared/models/internal-route';
import { LoginService } from 'src/core/services/login.service'; 

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  isExpanded = false;
  @Input() internalRoutes: InternalRoute[];
  isCollapsed = true;
  constructor(private readonly loginService: LoginService, private router: Router) {}

  ngOnInit() {}

  logout() {
    this.loginService.logout();
  }

  login() {
    this.router.navigateByUrl('/login');
    // this.loginService.login();
  }
  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
