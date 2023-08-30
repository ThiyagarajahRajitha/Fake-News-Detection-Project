import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserStoreService {
  private unique_name$ = new BehaviorSubject<string>("");
  private role$ = new BehaviorSubject<string>("");
  private primarysid$ = new BehaviorSubject<string>("");

  constructor() { }

  public getRoleFromStore(){
    return this.role$.asObservable();
  }

  public setRoleForStore(role:string){
    this.role$.next(role);
  }

  public getNameFromStore(){
    return this.unique_name$.asObservable();
  }

  public setNameForStore(name:string){
    this.unique_name$.next(name);
  }

  public getIdFromStore(){
    return this.primarysid$.asObservable();
  }

  public setIdForStore(primarysid:string){
    this.primarysid$.next(primarysid);
  }
}
