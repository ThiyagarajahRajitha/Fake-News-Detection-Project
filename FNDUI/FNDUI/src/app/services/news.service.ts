import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { News } from '../models/news.model';
import { Observable } from 'rxjs';
import { ClassifyNewsModel } from '../models/classify-news.model';
import { ClassifyNewsResultModel } from '../models/classify-news-result.model';
import { NewsClassificationCountsModel } from '../models/news-classification-counts-model';
import { RequestReviewModel } from '../models/request-review.model';
import { ReviewedResultModel } from '../models/reviewed-result.model';
import { ReviewRequestedNewsListModel } from '../models/review-requested-newsList.model';

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
  // getNewsCount(from:Date, to:Date):Observable<NewsClassificationCountsModel[]>{
  //   return this.http.get<NewsClassificationCountsModel[]>(this.baseApiUrl + '/api/News/GetNewsCount');
  // }
  getNewsCount(from: Date, to: Date): Observable<NewsClassificationCountsModel[]> {
    const apiUrl = `${this.baseApiUrl}/api/News/GetNewsCount?from=${from}&to=${to}`;
    console.log("apiurl " +apiUrl);
    return this.http.get<NewsClassificationCountsModel[]>(apiUrl);
  }

  submitReview(reviewedResultModel:ReviewedResultModel):Observable<any>{
    return this.http.post<any>(this.baseApiUrl +'/api/News/SubmitReview',reviewedResultModel)
  }

  GetReviewRequestedNewsByPublisherId(publisherId: number):Observable<ReviewRequestedNewsListModel[]>{
    return this.http.get<ReviewRequestedNewsListModel[]>(this.baseApiUrl + '/api/News/GetReviewRequestedNewsByPublisherId?userId='+publisherId);
  }
}
