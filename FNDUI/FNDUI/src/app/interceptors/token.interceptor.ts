import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, throwError, timer } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth:AuthService, private router:Router,private toastService:NgToastService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const myToken = this.auth.getToken();

    if(myToken){
      request = request.clone({
        setHeaders: {Authorization: `Bearer ${myToken}`}
      })
    }

    return next.handle(request).pipe(
      catchError((err:any)=>{
        if(err instanceof HttpErrorResponse){
          if(err.status === 401){
            this.toastService.success({
              detail: 'Success',
              summary: 'Token is expired. Login again',
              sticky: true, // Keep the toast visible
              position: 'tr',
              duration: 5000
          });
          // Delay the page reload after 5 seconds
          timer(5000).subscribe(() => {
            this.auth.logout();
            this.router.navigate(['login'])
            // location.reload();
        });
            //alert("Token is expired. Login again");
            
          }
        }
        return throwError(()=>err)
      })
    );
  }
}
