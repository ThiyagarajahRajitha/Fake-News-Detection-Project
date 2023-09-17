import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import ValidateForm from 'src/app/helpers/validateform';
import { Users } from 'src/app/models/users.model';
import { PublisherApprovalService } from 'src/app/services/publisher-approval/publisher-approval.service';

@Component({
  selector: 'app-publisher-apporval-modal',
  templateUrl: './publisher-apporval-modal.component.html',
  styleUrls: ['./publisher-apporval-modal.component.css']
})
export class PublisherApporvalModalComponent {

  constructor(private fb:FormBuilder, public activeModal: NgbActiveModal,
    private publisherApproval:PublisherApprovalService) { }
  form = this.fb.group({
    input:['', [Validators.required]]
  }) 
  isApprove: boolean = false;
  result: any= '';
  showForm: boolean = true;
  user:Users = {} as Users;


  dismissModal() {
    this.activeModal.dismiss();
  }

  onSubmit() {
    if(!this.form.valid) {
      ValidateForm.validateAllFormFields(this.form);
      return;
    }
    // You can access the form data and perform any necessary operations
    const input = this.form.value.input || '';    

    if (this.isApprove) {
      this.publisherApproval.approve(this.user.id)
      .subscribe({
        next:(x)=>{
          this.result = "Approved Successfully";
          this.showForm = false;
          this.user.status = 1;
        }
      })
    } else {
      this.user.status = -1;
    }

    // // Make a POST request to the API endpoint
    // this.subscriberService.subscribe(createSubscriberModel).subscribe(
    //     response => {
    //       // Handle the response from the API
    //       console.log('API response:', response);
    //       this.result = response.email;
    //       this.showForm = false;

    //       // Reset the form or perform further operations
    //     },
    //     error => {
    //       // Handle any errors that occur during the request
    //       console.error('API error:', error);
    //     }
    //   );
  }

}
