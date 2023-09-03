import { Component } from '@angular/core';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ModeratorService } from 'src/app/services/moderator/moderator.service';
import { UserStoreService } from 'src/app/services/user-store/user-store.service';
import { InviteModeratorModalComponent } from '../../invite-moderator/invite-moderator-modal/invite-moderator-modal.component';

import { FormControl } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-moderator-invitation',
  templateUrl: './moderator-invitation.component.html',
  styleUrls: ['./moderator-invitation.component.css']
})
export class ModeratorInvitationComponent {
  public role:string = "";
  users:Users[] = [];
  filterControl = new FormControl('false');


  constructor(private auth:AuthService, private userStore:UserStoreService, private modalService: NgbModal,private moderatorService:ModeratorService){}

  ngOnInit(){
    this.userStore.getRoleFromStore()
    .subscribe(val=>{
      const roleFromToken = this.auth.getRoleFromToken();
      this.role = val || roleFromToken;
    })
    this.getAllmoderatorInvitations("false");
    this.filterControl.valueChanges.subscribe( filter => {
      if (filter) {
        this.getAllmoderatorInvitations(filter)
      } else {
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
    const modalRef = this.modalService.open(InviteModeratorModalComponent);
  }
}
