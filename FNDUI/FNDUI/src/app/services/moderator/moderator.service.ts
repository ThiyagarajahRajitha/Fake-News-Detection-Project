import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { InviteModeratorModel} from 'src/app/models/invite-moderator.model';
import { ModeratorSignupModel } from 'src/app/models/moderator-signup.model';
import { ModeratorsVerifyResultModel } from 'src/app/models/moderator-verify-result.model';
import { ModeratorsVerifyModel } from 'src/app/models/moderator-verify.model';
import { ModeratorModel } from 'src/app/models/moderator.model';
import { RequestReviewModel } from 'src/app/models/request-review.model';
import { ReviewRequestedNewsListModel } from 'src/app/models/review-requested-newsList.model';
import { Users } from 'src/app/models/users.model';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ModeratorService {

  baseApiUrl:string = environment.baseApiUrl;
  constructor(private http:HttpClient) { }

  inviteModerator(createModeratorModel:InviteModeratorModel): Observable<any> {
    return this.http.post<any>(this.baseApiUrl + '/api/Moderator/CreateModerator', createModeratorModel, {observe: 'response'});
  }

  validateModerator(verifyModel:ModeratorsVerifyModel):Observable<any>{
    return this.http.post<any>(this.baseApiUrl + '/api/Moderator/ValidateModerator?username=' + verifyModel.username.toString()+'&inviteCode='+ verifyModel.inviteCode, verifyModel, {observe: 'response'});
 }

 registerModerator(moderatorSignUpModel:ModeratorSignupModel):Observable<any>{
  return this.http.post<any>(this.baseApiUrl + '/api/Moderator/RegisterModerator?Email='+ moderatorSignUpModel.Name, {observe: 'response'});
  }

  getModerators(filter: string):Observable<Users[]>{
    var usee = this.http.get<Users[]>(this.baseApiUrl + '/api/Moderator?Pending=' + filter);
    console.log(this.baseApiUrl + '/api/Moderator?Pending=' + filter);
    return usee;
 }

 deleteModerator(id: Number):Observable<any>{
  var resp = this.http.delete<any>(this.baseApiUrl + '/api/user/'+ id, {observe: 'response'});
  console.log(this.baseApiUrl + '/api/user/'+ id);
  return resp;
  }

  resend(id: Number):Observable<any>{
    var resp = this.http.post<any>(this.baseApiUrl + '/api/Moderator/Reinvite?id='+ id, {observe: 'response'});
    console.log(this.baseApiUrl + '/api/Moderator/'+ id);
    return resp;
    }

  deleteModeratorFrommoderator(id:Number):Observable<any>{
    var resp = this.http.delete<any>(this.baseApiUrl + '/api/Moderator/'+ id, {observe: 'response'});
    console.log(this.baseApiUrl + '/api/Moderator/'+ id);
    return resp;
  }  
}

