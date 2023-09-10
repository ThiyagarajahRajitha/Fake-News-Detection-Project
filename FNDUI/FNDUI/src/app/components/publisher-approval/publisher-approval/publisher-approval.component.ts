import { Component } from '@angular/core';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { PublisherApprovalService } from 'src/app/services/publisher-approval/publisher-approval.service';
import { UserStoreService } from 'src/app/services/user-store/user-store.service';
import { FormControl } from '@angular/forms';
import { ConfirmationDialogService } from '../../confim-dialog/confirmation-dialog.service';

@Component({
  selector: 'app-publisher-approval',
  templateUrl: './publisher-approval.component.html',
  styleUrls: ['./publisher-approval.component.css']
})
export class PublisherApprovalComponent {
  public role:string = "";
  users:Users[] = [];
  filterControl = new FormControl('false');

  constructor(private auth:AuthService, private userStore:UserStoreService, 
    private publisherApproval:PublisherApprovalService, 
    private confirmationDialogService: ConfirmationDialogService){}

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
    user.status = 1;
    this.publisherApproval.approve(user.id)
    .subscribe({
      next:(x)=>{
        console.log("succ");
        alert("Approved Successfully");
        
      }
    })
    //console.log(id+" APPROVE function called");
  }

  reject(user:Users){
    this.confirmationDialogService.confirm('Please confirm', 'Do you really want to delete ' + user.email + ' ?')
    .then((confirmed) => {
      user.status = -1;
      console.log('User confirmed:', confirmed)
    })
    .catch(() => console.log('User dismissed the dialog'));
  }
}
