import { Component } from '@angular/core';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ModeratorService } from 'src/app/services/moderator/moderator.service';
import { UserStoreService } from 'src/app/services/user-store/user-store.service';
import { InviteModeratorModalComponent } from '../../invite-moderator/invite-moderator-modal/invite-moderator-modal.component';

import { FormControl } from '@angular/forms';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationDialogService } from '../../../services/common/confirmation-dialog.service';
import { Route, Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { timer } from 'rxjs';

@Component({
  selector: 'app-moderator-invitation',
  templateUrl: './moderator-invitation.component.html',
  styleUrls: ['./moderator-invitation.component.css']
})
export class ModeratorInvitationComponent {
  public role:string = "";
  users:Users[] = [];
  filterControl = new FormControl('false');
  ngbModalOptions: NgbModalOptions = {
    backdrop : 'static',
    keyboard : false
    };
  error: any;
  isShowingPending:boolean = false;

  constructor(private auth:AuthService, private userStore:UserStoreService, 
    private modalService: NgbModal,private moderatorService:ModeratorService,
    private confirmationDialogService: ConfirmationDialogService,
    private router:Router,
    private toastService: NgToastService){}

  ngOnInit(){
    this.userStore.getRoleFromStore()
    .subscribe(val=>{
      const roleFromToken = this.auth.getRoleFromToken();
      this.role = val || roleFromToken;
    })
    this.getAllmoderatorInvitations("false");
    this.filterControl.valueChanges.subscribe( filter => {
      if (filter) {
        this.isShowingPending = true;
        this.getAllmoderatorInvitations(filter)
      } else {
        this.isShowingPending = false;
        this.getAllmoderatorInvitations("false")
      }
    });
  }

  getAllmoderatorInvitations(isPending:string){
    this.moderatorService.getModerators(isPending)
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
  openInviteModeratorModal(){
    const modalRef = this.modalService.open(InviteModeratorModalComponent, this.ngbModalOptions);
  }


  resend(user:Users){
    this.moderatorService.resend(user.id).subscribe({
      next:(res)=>{
        user.isDeleted=true;
        //alert('Invitation Sent successfully');
        //location.reload();
        this.toastService.success({
          detail: 'Success',
          summary: 'Invitation Sent successfully',
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
          summary: 'Failed to send invitation',
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

  delete(user:Users){
    this.confirmationDialogService.confirm('Please confirm', 'Do you really want to delete ' + user.email + ' ?')
    .then((confirmed) => {
      this.moderatorService.deleteModerator(user.id).subscribe({
        next:(res)=>{
          user.isDeleted=true;
          this.toastService.success({
            detail: 'Success',
            summary: 'The ' + user.email + ' Deleted.',
            sticky: true, // Keep the toast visible
            position: 'tr',
            duration: 5000
        });
        timer(5000).subscribe(() => {
          location.reload();
      });
        },
        error:(err)=>{
          //alert(err?.error.message)
          //this.error= err?. error.message;
          this.toastService.error({
            detail: 'Error',
            summary: 'Failed to Delete',
            sticky: true, // Keep the toast visible
            position: 'tr',
            duration: 5000
          });
          timer(5000).subscribe(() => {
            location.reload();
        });
      }
    })
      user.isDeleted = true;
      console.log('User confirmed to delete: ', confirmed)
    })
    .catch(() => console.log('User dismissed the dialog'));
  }

  deleteModerator(user:Users){
    this.confirmationDialogService.confirm('Please confirm', 'Do you really want to delete ' + user.email + ' invitation?')
  .then((confirmed) => {
    this.moderatorService.deleteModeratorFrommoderator(user.id).subscribe({
      next:(res)=>{
        user.isDeleted=true;
        this.toastService.success({
          detail: 'Success',
          summary: 'The invitation for ' + user.email + 'is Deleted.',
          sticky: true, // Keep the toast visible
          position: 'tr',
          duration: 5000
      });
      timer(5000).subscribe(() => {
        location.reload();
    });
      },
      error:(err)=>{
        //alert(err?.error.message)
        //this.error= err?. error.message;
        this.toastService.error({
          detail: 'Error',
          summary: 'Failed to Delete',
          sticky: true, // Keep the toast visible
          position: 'tr',
          duration: 5000
        });
        timer(5000).subscribe(() => {
          location.reload();
      });
    }
  })
    user.isDeleted = true;
    console.log('User confirmed to delete: ', confirmed)
  })
  .catch(() => console.log('User dismissed the dialog'));
}
}
