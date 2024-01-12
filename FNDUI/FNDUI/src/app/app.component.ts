import { Component, OnInit } from '@angular/core';
import { SubscribeService } from './services/subscribe/subscribe.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { AuthService } from './services/auth/auth.service';
import { UserStoreService } from './services/user-store/user-store.service';
import { InviteModeratorModalComponent } from './components/invite-moderator/invite-moderator-modal/invite-moderator-modal.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'FNDUI';
  
  public role:string = "";

  constructor(private subscribeService: SubscribeService, private modalService: NgbModal, private router:Router, private auth:AuthService, private userStore:UserStoreService){}
  ngOnInit(){
    this.userStore.getRoleFromStore()
    .subscribe(val=>{
      const roleFromToken = this.auth.getRoleFromToken();
      this.role = val || roleFromToken;
    })
  }
  openInviteModeratorModal(){
    const modalRef = this.modalService.open(InviteModeratorModalComponent);
  }
  publisherSignIn(){
    this.router.navigate(['login']);
  }
  logout(){
    this.auth.logout();
    window.location.reload();
  }
}
