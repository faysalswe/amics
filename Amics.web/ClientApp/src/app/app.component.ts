import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ApplicationUser } from 'src/core/models/application-user';
import { UserService } from 'src/core/services/user.service';
import { InternalRoute } from './shared/models/internal-route';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ["./app.component.scss"],
})
export class AppComponent implements OnInit, OnDestroy  {
  title = 'app';
  internalRoutes: InternalRoute[];
  user: string;
  private userSubscription: Subscription;

  constructor(
    private readonly userService: UserService
  ) {}

  ngOnInit(): void {
    this.userSubscription = this.userService.user$.subscribe((user) => {
        this.user = user.userId;
        this.setupNavMenus(user); 
      }); 
    
    }

    setupNavMenus(user: ApplicationUser) {
      this.internalRoutes = [];
      if (user.isAuthenticated() && user.isBasicUser()) {
        this.internalRoutes = [
          new InternalRoute("Inventory", "/inventory"),
          new InternalRoute("Sale Order", "/sales", [
            new InternalRoute("Purchase Order", "/purchase"),
            new InternalRoute("Work Order", "/work")            
          ]),
        ];
      }
      if (user.isAuthenticated() &&user.isAdmin()) {        
          this.internalRoutes = [
            ...this.internalRoutes,
            new InternalRoute("Accounting", "/accounting"),
            new InternalRoute("Admin", "/admin"),
          ];        
      } 
      if (user.isAuthenticated() && user.isBasicUser()) {
        this.internalRoutes = [
          ...this.internalRoutes,
          new InternalRoute("Contact Us", "/ContactUs"),
        ];
      }
    }
  
    ngOnDestroy(): void {
      if (this.userSubscription) {
        this.userSubscription.unsubscribe();
      }
    }
  }


