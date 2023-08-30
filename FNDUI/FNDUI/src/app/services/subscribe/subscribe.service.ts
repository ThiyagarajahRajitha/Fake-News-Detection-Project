import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateSubscriberModel } from 'src/app/models/create-subscriber.model';
import { SubscriberModel } from 'src/app/models/subscriber.model';
import { environment } from 'src/environments/environment.development';
import { ResponseResult } from '../common/responseResult';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SubscribeService {

  baseApiUrl:string = environment.baseApiUrl;
  constructor(private http:HttpClient) { }

  subscribe(createSubscriberModel: CreateSubscriberModel): Observable<SubscriberModel> {
    return this.http.post<SubscriberModel>(this.baseApiUrl + '/api/Subscriber', createSubscriberModel);
  }
}
