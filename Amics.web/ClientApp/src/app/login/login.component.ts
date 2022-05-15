import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginService } from 'src/core/services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent  implements OnInit {
    userName:string;
    password:string;
    form: FormGroup;
    public loginInvalid: boolean;
    private formSubmitAttempt: boolean;
    private returnUrl: string;  
    
    constructor(
        private readonly loginService: LoginService,
        private fb: FormBuilder, 
        private route: ActivatedRoute, 
        private router: Router,
        private loginservice: LoginService 
    ) {
    
    }
   
    async ngOnInit() {
    
        this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/game';
    
        this.form = this.fb.group({     
        username: ['', Validators.required],     
        password: ['', Validators.required]    
        });
     
    }
     
    async onSubmit() {    
        this.loginInvalid = false; 
        this.formSubmitAttempt = false; 
        if (this.form.valid) { 
                try { 
                    const username = this.form.get('username').value; 
                    const password = this.form.get('password').value; 
                    this.loginService.login(username, password).then(()=>{});
                } catch (err) {
                    this.loginInvalid = true;
            }
        } else { 
            this.formSubmitAttempt = true;
        } 
    }
    
    login() { 
        this.loginService.login(this.userName,this.password);
      }

}
