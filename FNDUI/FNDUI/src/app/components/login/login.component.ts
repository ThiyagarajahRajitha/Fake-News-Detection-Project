import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import ValidateForm from 'src/app/helpers/validateform';
import { AuthService } from 'src/app/services/auth/auth.service';
import { UserStoreService } from 'src/app/services/user-store/user-store.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  type:string = "password";
  isText:boolean = false;
  eyeIcon:string = "fa-eye-slash";
  error:string="";
  loginForm!:FormGroup;
  constructor(private fb:FormBuilder, private auth:AuthService, private router: Router, private userStore:UserStoreService){}
  
  ngOnInit():void{
    this.loginForm = this.fb.group({
      username:['',Validators.required],
      password:['',Validators.required]
    })
  }
  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye":this.eyeIcon="fa-eye-slash";
    this.isText ? this.type = "text": this.type = "password"; 
  }

  onLogin(){
    if(this.loginForm.valid){
      this.auth.login(this.loginForm.value)
      .subscribe({
        next:(res)=>{
          this.loginForm.reset();
          this.auth.storeToken(res.token);
          const tokenPayload = this.auth.decodedToken();
          this.userStore.setNameForStore(tokenPayload.unique_name);
          this.userStore.setRoleForStore(tokenPayload.role);
          //alert(res.message);
          if(tokenPayload.role == 'Admin')
          this.router.navigate(['publisher-approval']);
          if(tokenPayload.role == 'Publisher')
          this.router.navigate(['news-list']);
          if(tokenPayload.role == 'Moderator')
          this.router.navigate(['news-list']);
        },
        error:(err)=>{
          //alert(err?.error.message)
          this.error= err?. error.message;
          this.loginForm.reset();
        }
      })
    }
    else{
      ValidateForm.validateAllFormFields(this.loginForm);
      alert("Your Form is Invalid");
    }
  }
}
