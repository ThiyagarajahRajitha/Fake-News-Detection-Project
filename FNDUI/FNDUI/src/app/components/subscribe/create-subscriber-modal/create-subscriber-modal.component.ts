import { Component, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import ValidateForm from 'src/app/helpers/validateform';
import { CreateSubscriberModel } from 'src/app/models/create-subscriber.model';
import { SubscribeService } from 'src/app/services/subscribe/subscribe.service';

@Component({
  selector: 'app-create-subscriber-modal',
  templateUrl: './create-subscriber-modal.component.html',
  styleUrls: ['./create-subscriber-modal.component.css']
})
export class CreateSubscriberModalComponent {

  constructor(private fb:FormBuilder, public activeModal: NgbActiveModal, private subscriberService: SubscribeService) { }
  form = this.fb.group({
    email:['', [Validators.required, Validators.email]]
  }) 
  result: any= '';
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
    const createSubscriberModel: CreateSubscriberModel = {
      email: email
    };

    // Make a POST request to the API endpoint
    this.subscriberService.subscribe(createSubscriberModel).subscribe(
        response => {
          // Handle the response from the API
          console.log('API response:', response);
          this.result = response.email;
          this.showForm = false;

          // Reset the form or perform further operations
        },
        error => {
          // Handle any errors that occur during the request
          console.error('API error:', error);
        }
      );
  }
}
