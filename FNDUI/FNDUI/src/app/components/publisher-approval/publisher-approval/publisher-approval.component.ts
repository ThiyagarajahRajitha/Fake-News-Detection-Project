import { Component } from '@angular/core';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { PublisherApprovalService } from 'src/app/services/publisher-approval/publisher-approval.service';
import { UserStoreService } from 'src/app/services/user-store/user-store.service';
import { FormControl } from '@angular/forms';
import { ConfirmationDialogService } from '../../confim-dialog/confirmation-dialog.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { PublisherApporvalModalComponent } from '../publisher-apporval-modal/publisher-apporval-modal.component';
import { NgToastService } from 'ng-angular-popup';

@Component({
  selector: 'app-publisher-approval',
  templateUrl: './publisher-approval.component.html',
  styleUrls: ['./publisher-approval.component.css']
})
export class PublisherApprovalComponent {
  public role:string = "";
  users:Users[] = [];
  ngbModalOptions: NgbModalOptions = {
    backdrop : 'static',
    keyboard : false
    };
  filterControl = new FormControl('false');

  constructor(private auth:AuthService, private userStore:UserStoreService, 
    private publisherApproval:PublisherApprovalService, 
    private confirmationDialogService: ConfirmationDialogService,
    private modalService: NgbModal,
    private toastService: NgToastService){}

  ngOnInit(){
    this.userStore.getRoleFromStore()
    .subscribe(val=>{
      const roleFromToken = this.auth.getRoleFromToken();
      this.role = val || roleFromToken;
    })
    this.getPublishers("false");
    this.filterControl.valueChanges.subscribe( filter => {
      if (filter) {
        this.getPublishers(filter)
      } else {
        this.getPublishers("false")
      }
    });
  }

  getPublishers(IsPending:string){
    this.publisherApproval.getPublishers(IsPending)
    .subscribe({
      next:(users) => {
        this.users = users;
        console.log(users);
      },
      error:(response) =>{
        console.log(response);
        
      }
    })
  }

  approve(user:Users){
    const modalRef = this.modalService.open(PublisherApporvalModalComponent, this.ngbModalOptions);
    modalRef.componentInstance.user = user;
    modalRef.componentInstance.isApprove = true;  
  }

  reject(user:Users){
    const modalRef = this.modalService.open(PublisherApporvalModalComponent, this.ngbModalOptions);
    modalRef.componentInstance.user = user;
  }

  delete(user:Users){
    
    this.confirmationDialogService.confirm('Please confirm', 'Do you really want to delete ' + user.email + ' ?')
    .then((confirmed) => {
      if (confirmed) {
        // Delete the user here.
        user.status = -1;
        this.toastService.success({detail:'Success',summary:'The ' + user.email  + ' Deleted.',
                                   sticky:false,position:'tr', duration:5000})
        console.log('User confirmed to delete: ', confirmed)
      }
    })
    .catch(() => console.log('User dismissed the dialog'));
  }
}
