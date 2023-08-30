import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Users } from 'src/app/models/users.model';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class PublisherApprovalService {

  baseApiUrl:string = environment.baseApiUrl;
  constructor(private http:HttpClient) { }

  getPendingPublishers(filterPending: boolean):Observable<Users[]>{
     var usee = this.http.get<Users[]>(this.baseApiUrl + '/api/Publisher?PendingApproval=' + filterPending.toString());
     console.log(this.baseApiUrl + '/api/Publisher?PendingApproval=' + filterPending.toString());
     return usee;
  }

  approve(id: Number){
    const apiUrl = `${this.baseApiUrl}/api/Publisher/${id}/activate`;
    console.log("apiurl " +apiUrl);
    const body: string = `[{"op":"replace","path":"/id","value":345}]`;
    return this.http.patch(apiUrl, body, {
      headers: new HttpHeaders({'Content-Type': 'application/json-patch+json'}),
      observe: 'response',
      responseType: 'text'
  });
  }
}
