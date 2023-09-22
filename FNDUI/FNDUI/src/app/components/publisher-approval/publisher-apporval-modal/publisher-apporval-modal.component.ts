import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgToastService } from 'ng-angular-popup';
import { timer } from 'rxjs';
import ValidateForm from 'src/app/helpers/validateform';
import { ApprovePublisherModel } from 'src/app/models/approve-publisher.model';
import { RejectPublisherModel } from 'src/app/models/reject-publisher.model';
import { Users } from 'src/app/models/users.model';
import { PublisherApprovalService } from 'src/app/services/publisher-approval/publisher-approval.service';

@Component({
  selector: 'app-publisher-apporval-modal',
  templateUrl: './publisher-apporval-modal.component.html',
  styleUrls: ['./publisher-apporval-modal.component.css']
})
export class PublisherApporvalModalComponent {
  error: any;

  constructor(private fb:FormBuilder, public activeModal: NgbActiveModal,
    private publisherApproval:PublisherApprovalService,
    private toastService:NgToastService) { }
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
    const approvalData: ApprovePublisherModel = {
      divClass:input
      // Populate the properties of the model as needed
      // Example:
      // id: 345,
      // otherProperty: 'value',
    };

    const rejectData: RejectPublisherModel = {
      rejectReason:input
      // Populate the properties of the model as needed
      // Example:
      // id: 345,
      // otherProperty: 'value',
    };

    if (this.isApprove) {
      this.publisherApproval.approve(this.user.id, approvalData)
      .subscribe({
        next:(x)=>{
          this.result = "Approved Successfully";
          this.showForm = false;
          this.user.status = 1;
          this.toastService.success({
            detail: 'Success',
            summary: 'The ' + this.user.email + ' is approved.',
            sticky: true, // Keep the toast visible
            position: 'tr',
            duration: 5000
        });
        // Delay the page reload after 5 seconds
        timer(5000).subscribe(() => {
          location.reload();
      });
        },
        error:(err)=>{
          //alert(err?.error.message)
            //this.error= err?. error.message;
            this.toastService.error({
              detail: 'Error',
              summary: 'Approval Failed',
              sticky: true, // Keep the toast visible
              position: 'tr',
              duration: 5000
            });
            timer(5000).subscribe(() => {
              location.reload();
          });
        }
      })
    } 
    else {
      this.publisherApproval.rejectPublisher(this.user.id,rejectData)
      .subscribe({
        next:(x)=>{
          this.result = "Publisher Rejected Successfully";
          this.showForm = false;
          this.user.status = -1;
          this.toastService.success({
            detail: 'Success',
            summary: 'The ' + this.user.email + ' is rejected.',
            sticky: true, // Keep the toast visible
            position: 'tr',
            duration: 5000
        });
        // Delay the page reload after 5 seconds
        timer(5000).subscribe(() => {
          location.reload();
      });
        },
        error:(err)=>{
          // alert(err?.error.message)
          //   this.error= err?. error.message;
          this.toastService.error({
            detail: 'Error',
            summary: 'Failed to Reject',
            sticky: true, // Keep the toast visible
            position: 'tr',
            duration: 5000
          });
          timer(5000).subscribe(() => {
            location.reload();
        });
        }
      })
       
     }

    // if (!this.isApprove) {
    //   this.publisherApproval.rejectPublisher(this.user.id)
    //   .subscribe({
    //     next:(x)=>{
    //       this.result = "Rejected Publisher";
    //       this.showForm = false;
    //       this.user.status = 1;
    //     }
    //   })
    // } else {
    //   this.user.status = -1;
    // }

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
