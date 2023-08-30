import { Component } from '@angular/core';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { PublisherApprovalService } from 'src/app/services/publisher-approval/publisher-approval.service';
import { UserStoreService } from 'src/app/services/user-store/user-store.service';

@Component({
  selector: 'app-moderator-invitation',
  templateUrl: './moderator-invitation.component.html',
  styleUrls: ['./moderator-invitation.component.css']
})
export class ModeratorInvitationComponent {
  public role:string = "";
  users:Users[] = [];
  IsPending: boolean = false;
  isApproved = false;
  isRejected = false;

  constructor(private auth:AuthService, private userStore:UserStoreService, private moderatorInvitation:ModeratorInvitationComponent){}

  ngOnInit(){
    this.userStore.getRoleFromStore()
    .subscribe(val=>{
      const roleFromToken = this.auth.getRoleFromToken();
      this.role = val || roleFromToken;
    })
    //this.getAllmoderatorInvitations();
  }

  // getAllmoderatorInvitations(){
  //   this.moderatorInvitation.getAllmoderatorInvitations()
  //   .subscribe({
  //     next:(users) => {
  //       this.users = users;
  //       console.log(users);
  //     },
  //     error:(response) =>{
  //       console.log(response);
        
  //     }
  //   })
  // }

  // approve(id:number){
  //   this.isApproved = true;
  //   this.publisherApproval.approve(id)
  //   .subscribe({
  //     next:(x)=>{
  //       console.log("succ");
  //       alert("Approved Successfully");
        
  //     }
  //   })
  //   //console.log(id+" APPROVE function called");
  // }

  reject(id:number){
    this.isRejected = true;
    console.log("reject function called");
  }
}
