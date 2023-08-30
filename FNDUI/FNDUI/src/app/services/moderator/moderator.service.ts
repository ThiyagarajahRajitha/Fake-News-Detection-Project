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
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ModeratorService {

  baseApiUrl:string = environment.baseApiUrl;
  constructor(private http:HttpClient) { }

  inviteModerator(createModeratorModel:InviteModeratorModel): Observable<any> {
    return this.http.post<any>(this.baseApiUrl + '/api/User/CreateModerator', createModeratorModel, {observe: 'response'});
  }

  validateModerator(verifyModel:ModeratorsVerifyModel):Observable<any>{
    return this.http.post<any>(this.baseApiUrl + '/api/User/ValidateModerator?username=' + verifyModel.username.toString()+'&inviteCode='+ verifyModel.inviteCode, verifyModel, {observe: 'response'});
 }

 registerModerator(moderatorSignUpModel:ModeratorSignupModel):Observable<any>{
  return this.http.post<any>(this.baseApiUrl + '/api/User/RegisterModerator?Email='+ moderatorSignUpModel.Name, {observe: 'response'});
}

GetAllReviewRequestedNews():Observable<ReviewRequestedNewsListModel[]>{
  var usee = this.http.get<ReviewRequestedNewsListModel[]>(this.baseApiUrl + '/api/News/GetAllReviewRequestedNews');
  return usee;
}

}

