import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

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

  constructor(private fb:FormBuilder, private activeModal: NgbActiveModal) { }

  ngOnInit() {
  }

  public decline() {
    this.activeModal.close(false);
  }

  public accept() {
    this.activeModal.close(true);
  }

  public dismiss() {
    this.activeModal.dismiss();
  }

}
