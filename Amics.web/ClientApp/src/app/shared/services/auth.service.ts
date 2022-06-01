import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApplicationUser } from '../models/application-user';
import { AppSettingsService } from './app-settings.service';
import { OnInit } from '@angular/core';
const defaultPath = '/';
@Injectable({
  providedIn: "root",
})
export class AuthService {
  private _appUser = new BehaviorSubject<ApplicationUser>(new ApplicationUser());
  user$: Observable<ApplicationUser> = this._appUser.asObservable();
  user?: string;

  get loggedIn(): boolean {
    return !!this.user;
  }

  private _lastAuthenticatedPath: string = defaultPath;
  set lastAuthenticatedPath(value: string) {
    this._lastAuthenticatedPath = value;
  }

  constructor(private router: Router, private readonly httpClient: HttpClient) {
    
  }

  async logIn(userName: string, password: string): Promise<any> {
    try {
      // Send request
      password = btoa(password);
      return this.httpClient
        .get<ApplicationUser>(`{apiUrl}/login?userName=${userName}&password=${password}`)
        .toPromise()
        .then((result) => {
          if (!!result) {
            this.getUser().then(x => {
              this.user = x.userId;
              if (!!x.userId) {
                this.router.navigate([this._lastAuthenticatedPath])
                return {
                  isOk: true,
                  message: "Authentication success"
                };
              }
              else {
                this.router.navigate(['/login-form']);
                return {
                  isOk: false,
                  message: "Authentication failed"
                };
              }
            });
            return {
              isOk: true,
              message: "Authentication success"
            };
          }
          else {
            console.log("error logged in");
            return {
              isOk: false,
              message: "Authentication failed"
            };
          }
        })
        .catch((e) => {
          this.router.navigate(['/login-form']);
          console.log("error", e);
          return {
            isOk: false,
            message: "Authentication failed"
          };
        });
    }
    catch {
      return {
        isOk: false,
        message: "Authentication failed"
      };
    }
  }

  getUser(): Promise<ApplicationUser> {
    let user = new ApplicationUser();

    return this.httpClient
      .get<ApplicationUser>("{apiUrl}/user")
      .toPromise()
      .then((x) => {
        if (x) {
          user = new ApplicationUser(x.userId, x.userName, x.firstName, x.password, x.warehouse, x.lastName, x.email, x.userDataBase, x.buyer, x.salesPerson, x.webAccess, x.amicsUser, x.empList, x.invTrans, x.forgotPwdAns);
          this.user = user.userId;
        }
        this._appUser.next(user);

        return user;
      })
      .catch(() => {
        this._appUser.next(user);
        return user;
      });
  }

  async createAccount(email: string, password: string) {
    try {
      // Send request
      console.log(email, password);

      this.router.navigate(['/create-account']);
      return {
        isOk: true
      };
    }
    catch {
      return {
        isOk: false,
        message: "Failed to create account"
      };
    }
  }

  async changePassword(email: string, recoveryCode: string) {
    try {
      // Send request
      console.log(email, recoveryCode);

      return {
        isOk: true
      };
    }
    catch {
      return {
        isOk: false,
        message: "Failed to change password"
      }
    };
  }

  async resetPassword(email: string) {
    try {
      // Send request
      console.log(email);

      return {
        isOk: true
      };
    }
    catch {
      return {
        isOk: false,
        message: "Failed to reset password"
      };
    }
  }

  logOut(): void {
    let url = `logout`;
    if ((document as any).documentMode) {
      const base = document.getElementsByTagName('base')[0].href;
      url = base + url;
    }
    try {
      window.location.replace(url);
    } catch {
      window.location.href = url;
    }
  }
}

@Injectable({
  providedIn: "root",
})
export class AuthGuardService implements CanActivate {
  constructor(private router: Router, private authService: AuthService, private readonly environmentService: AppSettingsService) {

  }


  canActivate(route: ActivatedRouteSnapshot): boolean {
    const isLoggedIn = this.authService.loggedIn;
    const isAuthForm = [
      'login-form',
      'reset-password',
      'create-account',
      'change-password/:recoveryCode'
    ].includes(route.routeConfig?.path || defaultPath);

    if (isLoggedIn && isAuthForm) {
      this.authService.lastAuthenticatedPath = defaultPath;
      this.router.navigate([defaultPath]);
      return false;
    }

    if (!isLoggedIn && !isAuthForm) {
      this.router.navigate(['/login-form']);
    }

    if (isLoggedIn) {
      this.authService.lastAuthenticatedPath = route.routeConfig?.path || defaultPath;
    }

    return isLoggedIn || isAuthForm;
  }
}
