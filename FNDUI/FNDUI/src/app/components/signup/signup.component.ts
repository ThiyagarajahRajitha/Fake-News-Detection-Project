import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import ValidateForm from 'src/app/helpers/validateform';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  type:string = "password"
  isText:boolean = false
  eyeIcon:string = "fa-eye-slash"
  signUpForm! : FormGroup;
  showSignUpForm:boolean = true;
  created:boolean = false;
  error:any;

  constructor(private fb:FormBuilder,private auth:AuthService, private router:Router){}
  
  ngOnInit():void{
    this.signUpForm = this.fb.group({
      name:['',Validators.required],
      email:['',[Validators.required, Validators.email]],
      url:['',Validators.required],
      password:['',Validators.required],
    })
  }
  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye":this.eyeIcon="fa-eye-slash";
    this.isText ? this.type = "text": this.type = "password"; 
  }

  onSignUp(){
    if(this.signUpForm.valid){
      if(this.signUpForm.valid){
        this.auth.signup(this.signUpForm.value)
        .subscribe({
          next:(res=>{
            this.showSignUpForm = false;
            this.created = true;
          })
          ,error:(err=>{
            this.error = err?.error.message;
          })
        })
      }
    }
    else{
      ValidateForm.validateAllFormFields(this.signUpForm);
    }
  }
}
