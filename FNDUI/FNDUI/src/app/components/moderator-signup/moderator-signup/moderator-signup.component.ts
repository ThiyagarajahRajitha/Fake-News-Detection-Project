import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import ValidateForm from 'src/app/helpers/validateform';
import { ModeratorSignupModel } from 'src/app/models/moderator-signup.model';
import { ModeratorsVerifyModel } from 'src/app/models/moderator-verify.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ModeratorService } from 'src/app/services/moderator/moderator.service';

@Component({
  selector: 'app-moderator-signup',
  templateUrl: './moderator-signup.component.html',
  styleUrls: ['./moderator-signup.component.css']
})
export class ModeratorSignupComponent implements OnInit{

  constructor(private activateRoute: ActivatedRoute,private moderatorService: ModeratorService,private fb:FormBuilder,private auth:AuthService, private router:Router){}

  moderatorVerify!: ModeratorsVerifyModel;
  result: any = '';
  type:string = "password"
  isText:boolean = false
  eyeIcon:string = "fa-eye-slash"
  signUpForm! : FormGroup;
  showSignUpForm:boolean = false;
  error:any;
  email:string="";
 

  ngOnInit(): void {
    this.activateRoute.queryParams.subscribe((query: Params) => {
      console.log(query);
      this.verifyModerator(query);
    });
    
    //call service post call - verifymoderator
    //result
  }
  //if(result true) 
  //sign up html show else error

  verifyModerator(query:Params){
    this.moderatorVerify = new ModeratorsVerifyModel(query['username'], query['inviteCode'])
    this.moderatorService.validateModerator(this.moderatorVerify).subscribe(
      response => {
        // Handle the response from the API
        console.log('API response:', response);
        this.result = response.status;
        this.email=query['username'];
        console.log(this.result);
        if(this.result==200)
          this.showSignUpForm = true;
          this.validateForm();

        // Reset the form or perform further operations
      },
      error => {
        this.error = error;
        // Handle any errors that occur during the request
        console.error('API error:', error);
        alert(error? 'Username or invite code is invalid!':'');
        console.log(this.result);
      }
    );
  }



  
   validateForm():void{
    this.signUpForm = this.fb.group({
      name:['',Validators.required],
      email:['',Validators.required],
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
      const signUpModeratorModel: ModeratorSignupModel = {
        Email: this.moderatorVerify.username,
        InviteCode: this.moderatorVerify.inviteCode,
        Name: this.signUpForm.value.name,
        Password:this.signUpForm.value.password
      };
        this.auth.moderatorSignup(signUpModeratorModel)
        .subscribe({
          next:(res=>{
            alert(res.message);
            this.signUpForm.reset();
            this.router.navigate(['login']);
          })
          ,error:(err=>{
            alert(err?.error.message)
          })
        })
      
    }
    else{
      ValidateForm.validateAllFormFields(this.signUpForm);
      alert("Your Form is Invalid");
    }
  }

}
