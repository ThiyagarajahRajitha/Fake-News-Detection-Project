import { Component, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import ValidateForm from 'src/app/helpers/validateform';
import { ClassifyNewsModel } from 'src/app/models/classify-news.model';
import { NewsService } from 'src/app/services/news.service';

@Component({
  selector: 'app-classify-news-modal',
  templateUrl: './classify-news-modal.component.html',
  styleUrls: ['./classify-news-modal.component.css']
})
export class ClassifyNewsModalComponent {
  
  constructor(private fb:FormBuilder, public activeModal: NgbActiveModal, private newsService: NewsService) { }

  form = this.fb.group({
    title:['',Validators.required],
    content:['',Validators.required]
  })
  result: any = '';
  showForm: boolean = true;

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
    const newsTitle = this.form.value.title || '';
    const newsContent = this.form.value.content || '';
    //this.activeModal.close();

    // Create an object with the data to be sent
    const classifyNewsModel: ClassifyNewsModel = {
      topic: newsTitle,
      content: newsContent
    };

    // Make a POST request to the API endpoint
    this.newsService.classify(classifyNewsModel).subscribe(
      response => {
        // Handle the response from the API
        console.log('API response:', response);
        this.result = response.result;
        this.showForm = false;

        // Reset the form or perform further operations
      },
      error => {
        // Handle any errors that occur during the request
        console.error('API error:', error);
      });
  }
}
