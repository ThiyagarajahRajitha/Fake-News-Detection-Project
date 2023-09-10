import { Injectable } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationDialogComponent } from './confim-dialog.component';
import { FormBuilder, Validators } from '@angular/forms';


@Injectable({
    providedIn: 'root'
})
export class ConfirmationDialogService {

  constructor(private fb:FormBuilder, private modalService: NgbModal) { }

  public confirm(
    title: string,
    message: string,
    btnOkText: string = 'OK',
    btnCancelText: string = 'Cancel',
    dialogSize: 'sm'|'lg' = 'sm'): Promise<boolean> {
    const modalRef = this.modalService.open(ConfirmationDialogComponent, { size: dialogSize });
    modalRef.componentInstance.title = title;
    modalRef.componentInstance.message = message;
    modalRef.componentInstance.btnOkText = btnOkText;
    modalRef.componentInstance.btnCancelText = btnCancelText;
    modalRef.componentInstance.approval = true;
    modalRef.componentInstance.form = this.fb.group({
                                          newsDiv:['', Validators.required]
                                      }) 

    console.log(modalRef);
    return modalRef.result;
  }

}
