import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NewsClassificationCountsModel } from 'src/app/models/news-classification-counts-model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { NewsService } from 'src/app/services/news.service';
import { UserStoreService } from 'src/app/services/user-store/user-store.service';

@Component({
  selector: 'app-news-counts',
  templateUrl: './news-counts.component.html',
  styleUrls: ['./news-counts.component.css']
})
export class NewsCountsComponent implements OnInit{

  public name: string = "";
  newsCounts: NewsClassificationCountsModel[] = [];
  from: Date = new Date(new Date().setFullYear(new Date().getFullYear() - 1));;
  to: Date = new Date();
  formattedFromDate:any;
  formattedToDate:any;

  dataPoints: { label: string, y: number , color:string}[] = [];
  chart: any;  

  chartOptions = {
    animationEnabled: true,
    theme: "light2",
  title: {
      text: "Pie Chart"
    },
    data: [{
      type: "pie",
      dataPoints: [
        { label: "Fake",  y: 0 ,color: "RoyalBlue" },
        { label: "Real", y: 0 ,color: "#b0b1b2" }
      ]
    }] 
  }
  constructor(private newsService: NewsService, private datePipe: DatePipe, private auth: AuthService, private router:Router, private userStore:UserStoreService){               
  };
  
  ngOnInit(): void {
    if (this.from !== null) {
     this.formattedFromDate = this.datePipe.transform(this.from, 'yyyy-MM-dd');
      }
    if (this.to !== null) {
     this.formattedToDate = this.datePipe.transform(this.to, 'yyyy-MM-dd');
      } 
    this.getNewsCount();

    this.userStore.getNameFromStore()
    .subscribe(val =>{
      let nameFromToken = this.auth.getNameFromToken();
      this.name = val || nameFromToken
    })
  }

  getChartInstance(chart: object) {
    this.chart = chart;
  }

  fromDateChanged($event: { target: { value: any; }; }){
    console.log($event.target.value);
    this.from = $event.target.value;
    this.formattedFromDate = this.datePipe.transform(this.from, 'yyyy-MM-dd');
    this.getNewsCount();
  }

  toDateChanged($event: { target: { value: any; }; }){
    console.log($event.target.value);
    this.to = $event.target.value;
    this.formattedToDate = this.datePipe.transform(this.to, 'yyyy-MM-dd');
    this.getNewsCount();

  }

  getNewsCount(){
    this.newsService.getNewsCount(this.formattedFromDate, this.formattedToDate)
    .subscribe({
      next:(newscount) => {
        this.newsCounts = newscount;
        var dataPoints = this.newsCounts.map(({ classification, count }) => ({ label: classification, y: count }))
        //console.log(dataPoints);
        this.chart.options.data[0].dataPoints = dataPoints;
        //console.log(this.chart.options.data[0].dataPoints)
        this.chart.render();
        //console.log(this.newsCounts);
      },
      error:(response) =>{
        console.log(response);
      }
    })
    
  }

 

  
}
