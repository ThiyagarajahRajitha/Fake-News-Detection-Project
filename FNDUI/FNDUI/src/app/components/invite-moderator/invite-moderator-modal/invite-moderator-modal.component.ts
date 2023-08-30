import { HttpResponse } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { InviteModeratorModel} from 'src/app/models/invite-moderator.model';
import { ModeratorService } from 'src/app/services/moderator/moderator.service';

@Component({
  selector: 'app-invite-moderator-modal',
  templateUrl: './invite-moderator-modal.component.html',
  styleUrls: ['./invite-moderator-modal.component.css']
})
export class InviteModeratorModalComponent {
  constructor(public activeModal: NgbActiveModal, private moderatorService: ModeratorService) { }
  @ViewChild('emailInput') emailInput: any;
 
  result: any= '';
  error:any='';
  showForm: boolean = true;


  dismissModal() {
    this.activeModal.dismiss();
  }

  onSubmit() {
    // You can access the form data and perform any necessary operations
    const email = this.emailInput.nativeElement.value;
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
