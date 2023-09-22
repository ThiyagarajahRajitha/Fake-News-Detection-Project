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
          if (!res.token) {
            this.error= res.message;
            return;
          }
          this.auth.storeToken(res.token);
          const tokenPayload = this.auth.decodedToken();
          this.userStore.setNameForStore(tokenPayload.unique_name);
          this.userStore.setRoleForStore(tokenPayload.role);
          this.userStore.setIdForStore(tokenPayload.primarysid);
          //alert(res.message);
          if(tokenPayload.role == 'Admin')
          this.router.navigate(['publisher-approval']);
          if(tokenPayload.role == 'Publisher')
          this.router.navigate(['news-list']);
          if(tokenPayload.role == 'Moderator')
          this.router.navigate(['news-list']);
        },
        // error:(err)=>{
        //   alert(err?.error.message())
        //   console.log("authentication failure")
        //   //this.error= "Invalid Username or Password";
        //   //this.loginForm.reset();
          error:(err)=>{
            //alert(err?.error.message)
            this.loginForm.reset();
        }
      })
    }
    else{
      ValidateForm.validateAllFormFields(this.loginForm);
    }
  }

  // onLogin() {
  //   if (this.loginForm.valid) {
  //     this.auth.login(this.loginForm.value)
  //       .subscribe({
  //         next: (res) => {
  //           this.loginForm.reset();
  //           this.auth.storeToken(res.token);
  //           const tokenPayload = this.auth.decodedToken();
  //           this.userStore.setNameForStore(tokenPayload.unique_name);
  //           this.userStore.setRoleForStore(tokenPayload.role);
  //           this.userStore.setIdForStore(tokenPayload.primarysid);
  
  //           // Redirect based on user role
  //           if (tokenPayload.role === 'Admin') {
  //             this.router.navigate(['publisher-approval']);
  //           } else if (tokenPayload.role === 'Publisher' || tokenPayload.role === 'Moderator') {
  //             this.router.navigate(['news-list']);
  //           }
  //         },
  //         // error: (err) => {
  //         //   // Display the error message
  //         //   alert(err);
  
  //         error: (err) => {
  //           console.log(err); // Log the entire error object
  //           alert(err.error.Message); // Display the error message
  //           // Other error handling logic
          
  //           // Reset the form and any other necessary actions
  //           this.loginForm.reset();
  //         }
  //       });
  //   } else {
  //     ValidateForm.validateAllFormFields(this.loginForm);
  //   }
  // }

  // onLogin() {
  //   if (this.loginForm.valid) {
  //     this.auth.login(this.loginForm.value).subscribe({
  //       next: (res) => {
  //         // ... (existing code for successful login)
  //       },
        // error: (err) => {
        //   console.error(err); // Log the entire error response
        //   if (err.error && err.error.message) {
        //     // Display the custom error message from the backend
        //     alert(err.error.message);
        //   } else {
        //     // Fallback error message if the custom one is not available
        //     alert("Authentication failure");
        //   }
        //   console.log("Authentication failure");
        //   // this.error = "Invalid Username or Password";
        //   // this.loginForm.reset();
        // },
  //       error: (err) => {
  //         console.error(err); // Log the entire error response
  //         alert(err); // Display the error message received from the backend
  //         // Handle any additional logic for error handling
  //       },
  //     });
  //   } else {
  //     ValidateForm.validateAllFormFields(this.loginForm);
  //   }
  // }
  
  
}
