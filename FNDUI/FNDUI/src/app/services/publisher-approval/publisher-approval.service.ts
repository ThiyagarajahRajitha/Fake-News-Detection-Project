import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Publication } from 'src/app/models/Publication.model';
import { ApprovePublisherModel } from 'src/app/models/approve-publisher.model';
import { RejectPublisherModel } from 'src/app/models/reject-publisher.model';
import { Users } from 'src/app/models/users.model';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class PublisherApprovalService {

  baseApiUrl:string = environment.baseApiUrl;
  constructor(private http:HttpClient) { }

  getPublishers(filter: string):Observable<Users[]>{
     var usee = this.http.get<Users[]>(this.baseApiUrl + '/api/Publisher?PendingApproval=' + filter);
     console.log(this.baseApiUrl + '/api/Publisher?PendingApproval=' + filter);
     return usee;
  }

  getPublication():Observable<Publication[]>{
    var usee = this.http.get<Publication[]>(this.baseApiUrl + '/api/Publisher/GetPublication');
    console.log(this.baseApiUrl + '/api/Publisher/GetPublication');
    return usee;
 }

  // approve(id: Number){
  //   const apiUrl = `${this.baseApiUrl}/api/Publisher/${id}/activate`;
  //   console.log("apiurl " +apiUrl);
  //   const body: string = `[{"op":"replace","path":"/id","value":345}]`;
  //   return this.http.patch(apiUrl, body, {
  //     headers: new HttpHeaders({'Content-Type': 'application/json-patch+json'}),
  //     observe: 'response',
  //     responseType: 'text'
  // });
  // }

  // approve(id: number) {
  //   const apiUrl = `${this.baseApiUrl}/api/Publisher/${id}/activate`;
  //   console.log("apiurl " + apiUrl);
  
  //   // Construct the object as per the backend's expectations
    
  //   return this.http.patch(apiUrl, <ApprovePublisherModel>, {
  //     headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  //     observe: 'response',
  //   });
  // }
  approve(id: number, approvalData: ApprovePublisherModel): Observable<any> {
    const apiUrl = `${this.baseApiUrl}/api/Publisher/${id}/activate`;

    // Send the approvalData object in the request body
    return this.http.patch(apiUrl, approvalData, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      observe: 'response',
    });
  }

  rejectPublisher(id: Number, rejectData: RejectPublisherModel):Observable<any>{
    const apiUrl = `${this.baseApiUrl}/api/Publisher/${id}/reject`;
    //var resp = this.http.patch<any>(this.baseApiUrl + '/api/Publisher/'+ id, {observe: 'response'});
    var resp = this.http.patch(apiUrl,rejectData, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      observe: 'response',});
    //console.log(this.baseApiUrl + '/api/Publisher/'+ id);
    return resp;
    }

  deletePublisher(id: Number):Observable<any>{
    var resp = this.http.delete<any>(this.baseApiUrl + '/api/Publisher/'+ id, {observe: 'response'});
    console.log(this.baseApiUrl + '/api/Publisher/'+ id);
    return resp;
    }
}
