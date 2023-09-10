import { Component, Input, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import ValidateForm from 'src/app/helpers/validateform';

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confim-dialog.component.html',
})
export class ConfirmationDialogComponent implements OnInit {

  @Input()
    title!: string;
  @Input()
    message!: string;
  @Input()
    btnOkText!: string;
  @Input()
    btnCancelText!: string;
   
    form = this.fb.group({
        newsDiv:['', Validators.required]
    })  
    approval: boolean= false;

  constructor(private fb:FormBuilder, private activeModal: NgbActiveModal) { }

  ngOnInit() {
  }

  public decline() {
    this.activeModal.close(false);
  }

  public accept() {
    if(!this.form.valid) {
      console.log("Accept error ", this.form)
      ValidateForm.validateAllFormFields(this.form);
      return;
    }
    this.activeModal.close(true);
  }

  public dismiss() {
    this.activeModal.dismiss();
  }

}
