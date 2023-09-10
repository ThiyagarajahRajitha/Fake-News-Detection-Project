import { HttpResponse } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import ValidateForm from 'src/app/helpers/validateform';
import { InviteModeratorModel} from 'src/app/models/invite-moderator.model';
import { ModeratorService } from 'src/app/services/moderator/moderator.service';

@Component({
  selector: 'app-invite-moderator-modal',
  templateUrl: './invite-moderator-modal.component.html',
  styleUrls: ['./invite-moderator-modal.component.css']
})
export class InviteModeratorModalComponent {
  constructor(private fb:FormBuilder, public activeModal: NgbActiveModal, private moderatorService: ModeratorService) { }
  form = this.fb.group({
    email:['', [Validators.required, Validators.email]]
  })  
  result: any= '';
  error:any='';
  showForm: boolean = true;


  dismissModal() {
    this.activeModal.dismiss();
  }

  onSubmit() {
    if(!this.form.valid) {
      ValidateForm.validateAllFormFields(this.form);
      return;
    }
    // You can access the form data and perform any necessary operations
    const email = this.form.value.email || '';
    //this.activeModal.close();
    
    // Create an object with the data to be sent
    const createModeratorModel: InviteModeratorModel = {
      email: email
    };

    // Make a POST request to the API endpoint
    this.moderatorService.inviteModerator(createModeratorModel).subscribe(
        // next:(res=>{
          // Handle the response from the API
          // console.log('API response:', res.message);
          // //this.result = response.result;
          // alert(res.message);
          // this.showForm = false;
          response => {
            // Handle the response from the API
            console.log('API response:', response);
            this.result = response.status;
            this.showForm = false;
        },
        error => {
          this.error = error;
          // Handle any errors that occur during the request
          console.error('API error:', error);
        });

          // .subscribe({
          //   next:(res=>{
          //     alert(res.message);
          // Reset the form or perform further operations
        //   error:(err=>{
        //     alert(err?.error.message)
        //   })
        // })
  }
}
