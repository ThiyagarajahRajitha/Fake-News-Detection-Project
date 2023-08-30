import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';
import { UserStoreService } from 'src/app/services/user-store/user-store.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent {
  public role:string = "";
  public username:string="";

  constructor(private auth:AuthService, private userStore:UserStoreService, private route: ActivatedRoute, private router: Router){}

  ngOnInit(){
    this.userStore.getRoleFromStore()
    .subscribe(val=>{
      const roleFromToken = this.auth.getRoleFromToken();
      this.username=this.auth.getNameFromToken();
      this.role = val || roleFromToken;
    })

    this.route.firstChild?.url.subscribe(segments => {
      const path = segments[0]?.path;
      if (path) {
        this.activeTab = path;
      }
    });
  }

  publisherSignIn(){
    this.router.navigate(['login']);
  }

  logout(){
    this.auth.logout();
    window.location.reload();
  }
  activeTab: string = 'publisher-approval'; // Default active tab

  loadComponent(tab: string) {
    this.activeTab = tab;
    this.router.navigate([tab], { relativeTo: this.route });
  }
}
