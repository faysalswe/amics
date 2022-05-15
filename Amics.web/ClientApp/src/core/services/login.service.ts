import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ApplicationUser } from '../models/application-user';
import { UserService } from './user.service';

@Injectable({ providedIn: 'root' })
export class LoginService {
  returnUri = '';
  constructor(private readonly httpClient: HttpClient, private readonly userService:UserService, private router: Router) { }

  login(userName, password): Promise<ApplicationUser> {
    let user = new ApplicationUser();   
    return this.httpClient
      .get<ApplicationUser>(`login?userName=${userName}&password=${password}`)
      .toPromise()
      .then((result) => { 
        if(!!result)
        { 
          console.log("success fully logged in");
          var user = this.userService.getUser();
          return user;         
        }
        console.log("error logged in");
        return null;        
      })
      .catch((e) => {
        console.log("error",e);
       return null;
      });  
  }   

  logout(): void {
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
