import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable, catchError, map, throwError } from 'rxjs';
import { ModeratorSignupModel } from 'src/app/models/moderator-signup.model';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseApiUrl:string = environment.baseApiUrl;
  private userPayload:any;
  //private baseUrl:string = "https://localhost:7288/api/User/"
  constructor(private http : HttpClient, private router:Router) {
    this.userPayload = this.decodedToken();
   }

  signup(userObj:any){
    var signupRequset ={
      "name":userObj.name,
      "url":userObj.url,
      "email":userObj.email,
      "password": userObj.password
    }
    //return this.http.post<any>(`${this.baseUrl}register`, signupRequset)
    return this.http.post<any>(this.baseApiUrl + '/api/User/CreateUser', signupRequset);
  }

  moderatorSignup(userObj:ModeratorSignupModel){
    var signupRequset ={
      "email":userObj.Email,
      "name":userObj.Name,
      "invitecode":userObj.InviteCode,
      "password": userObj.Password
    }
    //return this.http.post<any>(`${this.baseUrl}register`, signupRequset)
    return this.http.post<any>(this.baseApiUrl + '/api/Moderator/RegisterModerator', signupRequset);
  }

  login(loginObj:any){
    var loginrequst={
      "username": loginObj.username,
      "password":loginObj.password
    }
    //return this.http.post<any>(`${this.baseUrl}authenticate`, loginrequst)
    return this.http.post<any>(this.baseApiUrl + '/api/User/authenticate', loginrequst);
  }

  storeToken(tokenValue:string){
    localStorage.setItem('token', tokenValue);
    this.userPayload = this.decodedToken();
  }

  getToken(){
    return localStorage.getItem('token');
  }

  isLoggedIn():boolean{
    return !!localStorage.getItem('token')
  }

  logout(){
    localStorage.clear();
    //localStorage.removeItem('token');
    this.router.navigate(['news-list'])
  }

  decodedToken(){
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    return jwtHelper.decodeToken(token);
  }

  getNameFromToken(){
    if(this.userPayload)
      return this.userPayload.unique_name;
  }
  getRoleFromToken(){
    if(this.userPayload)
      return this.userPayload.role;
  }
  getprimarySidFromToken(){
    if(this.userPayload)
      return this.userPayload.primarysid;
  }
}
