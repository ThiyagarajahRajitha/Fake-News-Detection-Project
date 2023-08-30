import { Component, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CreateSubscriberModel } from 'src/app/models/create-subscriber.model';
import { SubscribeService } from 'src/app/services/subscribe/subscribe.service';

@Component({
  selector: 'app-create-subscriber-modal',
  templateUrl: './create-subscriber-modal.component.html',
  styleUrls: ['./create-subscriber-modal.component.css']
})
export class CreateSubscriberModalComponent {

  constructor(public activeModal: NgbActiveModal, private subscriberService: SubscribeService) { }
  @ViewChild('emailInput') emailInput: any;
 
  result: any= '';
  showForm: boolean = true;


  dismissModal() {
    this.activeModal.dismiss();
  }

  onSubmit() {
    // You can access the form data and perform any necessary operations
    const email = this.emailInput.nativeElement.value;
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
