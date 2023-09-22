import { Component, Input, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import ValidateForm from 'src/app/helpers/validateform';
import { ReviewedResultModel } from 'src/app/models/reviewed-result.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { NewsService } from 'src/app/services/news.service';

@Component({
  selector: 'app-submit-review-modal',
  templateUrl: './submit-review-modal.component.html',
  styleUrls: ['./submit-review-modal.component.css']
})
export class SubmitReviewModalComponent {
  @Input() newId: number = 0;
  constructor(private fb:FormBuilder, public activeModal: NgbActiveModal, private newsService: NewsService, private auth:AuthService) { }

  form = this.fb.group({
    result:['', Validators.required],
    newscontent: ['', Validators.required]
  }) 

  result: any= '';
  error:any='';
  showForm: boolean = true;
  userId= this.auth.getprimarySidFromToken();

  dismissModal() {
    this.activeModal.dismiss();
  }

  onSubmit() {

    if(!this.form.valid) {
      ValidateForm.validateAllFormFields(this.form);
      return;
    }
    
    // Handle the form submission here
    // You can access the form data and perform any necessary operations
    //const newsTitle = this.titleInput.nativeElement.value;
    const feedback = this.form.value.newscontent || '';
    const reviewResult = this.form.value.result || '';
    //this.activeModal.close();

    // Create an object with the data to be sent
    const reviewedResult: ReviewedResultModel = {
      requestReviewId: this.newId,
      reviewResult:reviewResult,
      reviewFeedback: feedback,
      reviewerId:this.userId
    };

    // Make a POST request to the API endpoint
    this.newsService.submitReview(reviewedResult).subscribe(
      response => {
        console.log('API response:', response);
            this.result = response.status;
            this.showForm = false;

      },
      error => {
        this.error = error;
          console.error('API error:', error);
      });
  }
}
