import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { News } from '../models/news.model';
import { Observable } from 'rxjs';
import { ClassifyNewsModel } from '../models/classify-news.model';
import { ClassifyNewsResultModel } from '../models/classify-news-result.model';
import { NewsClassificationCountsModel, NewsCountByMonthDashboardModel, PublisherDashboardModel, ReviewRequestCountByModeratorDashboardModel, ReviewRequestCountDashboardModel, ReviewRequestCountsModel } from '../models/news-classification-counts-model';
import { RequestReviewModel } from '../models/request-review.model';
import { ReviewedResultModel } from '../models/reviewed-result.model';
import { ReviewRequestedNewsListModel } from '../models/review-requested-newsList.model';
import { Users } from '../models/users.model';

@Injectable({
  providedIn: 'root'
})
export class NewsService {
 
  baseApiUrl:string = environment.baseApiUrl;
  constructor(private http:HttpClient) { }

  getAllNews(filterFake: boolean):Observable<News[]>{
    return this.http.get<News[]>(this.baseApiUrl + '/api/News?FakeNewsOnly=' + filterFake.toString());
  }
  getNewsByPublisherId(publisherId: number, fitler:string):Observable<News[]>{
    return this.http.get<News[]>(this.baseApiUrl + '/api/News/'+publisherId+'?filter='+fitler);
  }

  classify(classifyNewsModel: ClassifyNewsModel): Observable<ClassifyNewsResultModel> {
    return this.http.post<ClassifyNewsResultModel>(this.baseApiUrl + '/api/News', classifyNewsModel);
  }

  requestReview(requestReviewModel:RequestReviewModel):Observable<any>{
    return this.http.post<any>(this.baseApiUrl + '/api/News/RequestReview',requestReviewModel)
  }
  getNewsCount(id:number,from: Date, to: Date): Observable<NewsClassificationCountsModel> {
    const apiUrl = `${this.baseApiUrl}/api/News/GetNewsCount?userId=${id}&from=${from}&to=${to}`;
    console.log("apiurl " +apiUrl);
    return this.http.get<NewsClassificationCountsModel>(apiUrl);
  }

  getReviewRequestCount(id:number,from: Date, to: Date): Observable<ReviewRequestCountsModel> {
    const apiUrl = `${this.baseApiUrl}/api/News/GetReviewRequestCount?userId=${id}&from=${from}&to=${to}`;
    console.log("apiurl " +apiUrl);
    return this.http.get<ReviewRequestCountsModel>(apiUrl);
  }

  getNewsCountByPublisher(id:number,from: Date, to: Date): Observable<PublisherDashboardModel[]> {
    const apiUrl = `${this.baseApiUrl}/api/News/GetNewsClassificationCountByPublisher?userId=${id}&from=${from}&to=${to}`;
    console.log("apiurl " +apiUrl);
    return this.http.get<PublisherDashboardModel[]>(apiUrl);
  }

  getReviewRequestCountByPublisher(id:number,from: Date, to: Date): Observable<ReviewRequestCountDashboardModel[]> {
    const apiUrl = `${this.baseApiUrl}/api/News/GetReviewRequestCountByPublisher?userId=${id}&from=${from}&to=${to}`;
    console.log("apiurl " +apiUrl);
    return this.http.get<ReviewRequestCountDashboardModel[]>(apiUrl);
  }
  getReviewRequestByModeratorCount(id:number,from: Date, to: Date):Observable<ReviewRequestCountByModeratorDashboardModel[]> {
    const apiUrl = `${this.baseApiUrl}/api/Moderator/GetReviewRequestCountByModerator?userId=${id}&from=${from}&to=${to}`;
    console.log("apiurl " +apiUrl);
    return this.http.get<ReviewRequestCountByModeratorDashboardModel[]>(apiUrl);
  }

  getModerators(filter: string):Observable<Users[]>{
    var usee = this.http.get<Users[]>(this.baseApiUrl + '/api/Moderator?Pending=false');
    console.log(this.baseApiUrl + '/api/Moderator?Pending=false');
    return usee;
 }

 getNewsCountByMonth(id:number,from: Date, to: Date): Observable<NewsCountByMonthDashboardModel[]> {
  const apiUrl = `${this.baseApiUrl}/api/News/GetNewsClassificationCountByMonth?userId=${id}&from=${from}&to=${to}`;
  console.log("apiurl " +apiUrl);
  return this.http.get<NewsCountByMonthDashboardModel[]>(apiUrl);
 }

  submitReview(reviewedResultModel:ReviewedResultModel):Observable<any>{
    return this.http.post<any>(this.baseApiUrl +'/api/News/SubmitReview',reviewedResultModel)
  }
  GetAllReviewRequestedNews(filter:string) {
    return this.http.get<News[]>(this.baseApiUrl + '/api/News/GetAllReviewRequestedNews?filter=' + filter);
  }

  getPublishers(filter: string):Observable<Users[]>{
    var usee = this.http.get<Users[]>(this.baseApiUrl + '/api/Publisher?PendingApproval=false');
    console.log(this.baseApiUrl + '/api/Publisher?PendingApproval=false');
    return usee;
 }
}
