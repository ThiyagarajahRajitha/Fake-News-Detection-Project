import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Publication } from 'src/app/models/Publication.model';
import { NewsClassificationCountsModel, PublisherDashboardModel } from 'src/app/models/news-classification-counts-model';
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
  fakeColor:string = "#FF8000";
  realColor:string = "#4682B4";
  from: Date = new Date(new Date().setDate(new Date().getDate() - 7));;
  to: Date = new Date();
  formattedFromDate:any;
  formattedToDate:any;
 
  publicationMap = new Map([
    [ 1, "News One" ],
    [ 2, "Test News" ]
]);
  publications: Publication[] = [
    {name: 'All', id: 0},
    {name: 'News1st', id: 1},
    {name: 'Daily Mirror', id: 2},
  ];

  newsChart: any;  
  newsChartOptions = {
    animationEnabled: true,
    theme: "light2",
  title: {
      text: "Classification Result"
    },
    data: [{
      type: "pie",
      dataPoints: [
      ]
    }] 
  }

  pubhlisherChart: any;
  publisherChartOptions = {
	  animationEnabled: true,
	  title: {
		text: "Classification By Publication"
	  },
	  axisY: {
		  title: "Count"
	  },
	  toolTip: {
		  shared: true
	  },
	  legend:{
      cursor:"pointer",
      itemclick: function(e: any){
        if (typeof(e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
        e.dataSeries.visible = false;
        }
        else {
        e.dataSeries.visible = true;
        }
        e.chart.render();
      }
	  },
	  data: [{
              type: "column",	
              name: "Real News count",
              showInLegend: true, 
              color: this.realColor,
              dataPoints:[
                ]
            }, {
              type: "column",	
              name: "Fake News Count",
              showInLegend: true,
              color: this.fakeColor,
              dataPoints:[
		          ]
	      }]
	};

  timeSeriesChart: any;
  timeSeriesChartOptions = {
    
		animationEnabled: true,
		theme: "light2",
		title: {
			text: "Classification Time Series"
		},
		axisX: {
			valueFormatString: "MMM",
			intervalType: "month",
			interval: 1
		},
		axisY: {
			title: "Count"
		},
		toolTip: {
			shared: true
		},
		legend: {
			cursor: "pointer",
			itemclick: function(e: any){
				if (typeof(e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
					e.dataSeries.visible = false;
				} else{
					e.dataSeries.visible = true;
				}
				e.chart.render();
			}
		},
		data: [{
			type:"line",
			name: "Fake",
			showInLegend: true,
      color: this.fakeColor,
			dataPoints: [		
				{ x: new Date(2022, 9, 1), y: 5 },
				{ x: new Date(2022, 10, 1), y: 4 },
				{ x: new Date(2022, 11, 1), y: 3 },
				{ x: new Date(2022, 12, 1), y: 7 },
				{ x: new Date(2023, 1, 1), y: 8 },
				{ x: new Date(2023, 2, 1), y: 5 },
				{ x: new Date(2023, 3, 1), y: 5 },
				{ x: new Date(2023, 4, 1), y: 4 },
				{ x: new Date(2023, 5, 1), y: 2 },
				{ x: new Date(2023, 6, 1), y: 9 },
				{ x: new Date(2023, 7, 1), y: 8 },
				{ x: new Date(2023, 8, 1), y: 1 }
			]
		},
		{
			type: "line",
			name: "Real",
			showInLegend: true,
      color: this.realColor,
			dataPoints: [
        { x: new Date(2022, 9, 1), y: 64 },
				{ x: new Date(2022, 10, 1), y: 54 },
				{ x: new Date(2022, 11, 1), y: 44 },
				{ x: new Date(2022, 12, 1), y: 40 },
				{ x: new Date(2023, 1, 1), y: 42 },
				{ x: new Date(2023, 2, 1), y: 50 },
				{ x: new Date(2023, 3, 1), y: 62 },
				{ x: new Date(2023, 4, 1), y: 72 },
				{ x: new Date(2023, 5, 1), y: 80 },
				{ x: new Date(2023, 6, 1), y: 85 },
				{ x: new Date(2023, 7, 1), y: 84 },
				{ x: new Date(2023, 8, 1), y: 10 }
			]
		}]

  }

  reviewRequestChart: any;
  getReviewRequestsChartInstance(chart: object) {
    this.reviewRequestChart = chart;
  }
  reviewRequestChartOptions = {
    animationEnabled: true,
    theme: "light2",
  title: {
      text: "Review Requests"
    },
    data: [{
      type: "pie",
      dataPoints: [
        {label : "Pending", y : 10},
        {label : "Completed", y : 20}
      ]
    }] 
  }

  reviewPublisherChart: any;
  getReviewPublisherChartInstance(chart: object) {
    this.reviewPublisherChart = chart;
  }
  reviewPublisherChartOptions = {
	  animationEnabled: true,
	  title: {
		text: "Review Requests By Publication"
	  },
	  axisY: {
		  title: "Count"
	  },
	  toolTip: {
		  shared: true
	  },
	  legend:{
      cursor:"pointer",
      itemclick: function(e: any){
        if (typeof(e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
        e.dataSeries.visible = false;
        }
        else {
        e.dataSeries.visible = true;
        }
        e.chart.render();
      }
	  },
	  data: [{
              type: "column",	
              name: "Completed Count",
              showInLegend: true, 
              dataPoints:[
                ]
            }, {
              type: "column",	
              name: "Pending Count",
              showInLegend: true,
              dataPoints:[
		          ]
	      }]
	}

  reviewModeratorChart: any;
  getReviewModeratorChartInstance(chart: object) {
    this.reviewModeratorChart = chart;
  }
  reviewModeratorChartOptions = {
	  animationEnabled: true,
	  title: {
		text: "Reviews By Moderator"
	  },
	  axisY: {
		  title: "Count"
	  },
	  toolTip: {
		  shared: true
	  },
	  legend:{
      cursor:"pointer",
      itemclick: function(e: any){
        if (typeof(e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
        e.dataSeries.visible = false;
        }
        else {
        e.dataSeries.visible = true;
        }
        e.chart.render();
      }
	  },
	  data: [{
              type: "column",	
              name: "Reviewed Count",
              showInLegend: true, 
              dataPoints:[
                  {label : "Reviwer One", y : 3},
                  {label : "News Reviewer", y : 9},
                  {label : "Test Moderator", y : 5},
                
                ]
	      }]
	}
  constructor(private newsService: NewsService, private datePipe: DatePipe, private auth: AuthService, private router:Router, private userStore:UserStoreService){               
  };
  
  ngOnInit(): void {
    this.userStore.getNameFromStore()
    .subscribe(val =>{
      let nameFromToken = this.auth.getNameFromToken();
      this.name = val || nameFromToken
    })
    this.getNewsCount();
  }

  getNewsChartInstance(chart: object) {
    this.newsChart = chart;
  }

  getPublisherChartInstance(chart: object) {
    this.pubhlisherChart = chart;
  }

  getTimeSeriesChartInstance(chart:object) {
    this.timeSeriesChart = chart;
  }

  fromDateChanged($event: { target: { value: any; }; }){
    console.log($event.target.value);
    this.from = $event.target.value;
    this.getNewsCount();
  }

  toDateChanged($event: { target: { value: any; }; }){
    console.log($event.target.value);
    this.to = $event.target.value;
    this.getNewsCount();

  }

  getNewsCount(){
    if (this.from !== null) {
      this.formattedFromDate = this.datePipe.transform(this.from, 'yyyy-MM-dd');
    } 
    if (this.to !== null) {
      this.formattedToDate = this.datePipe.transform(this.to, 'yyyy-MM-dd'); 
    }
    this.newsService.getNewsCount(this.formattedFromDate, this.formattedToDate)
    .subscribe({
      next:(newsCounts) => {
        var dataPoints = [
          { label: "Fake",  y: newsCounts.fakeCount, color: this.fakeColor },
          { label: "Real", y: newsCounts.realCount , color: this.realColor }
        ]
        this.newsChart.options.data[0].dataPoints = dataPoints;
        this.newsChart.render();
        
        this.getPublisherCount();
        this.getReviewRequestPublisherCount();
      },
      error:(response) =>{
        console.log(response);
      }
    })
  }

  getPublisherCount(){
    const newsCountsByPublication: PublisherDashboardModel[] =
        [
          {"realCount":4, "fakeCount": 1, "pId" : 1},
          {"realCount":3, "fakeCount": 2, "pId" : 2},
        ];

    var realDataPoints = newsCountsByPublication.map(({ pId, realCount }) => 
            ({ label: this.publicationMap.get(pId), y: realCount })
          );
    var fakeDataPoints = newsCountsByPublication.map(({ pId, fakeCount }) => 
          ({ label: this.publicationMap.get(pId), y: fakeCount })
        )
          
    this.reviewPublisherChart.options.data[0].dataPoints = realDataPoints;
    this.reviewPublisherChart.options.data[1].dataPoints = fakeDataPoints;
    this.reviewPublisherChart.render();
  }

  getReviewRequestPublisherCount() {

    const newsCountsByPublication: PublisherDashboardModel[] =
        [
          {"realCount":20, "fakeCount": 1, "pId" : 1},
          {"realCount":10, "fakeCount": 1, "pId" : 2},
        ];

    var realDataPoints = newsCountsByPublication.map(({ pId, realCount }) => 
            ({ label: this.publicationMap.get(pId), y: realCount })
          );
    var fakeDataPoints = newsCountsByPublication.map(({ pId, fakeCount }) => 
          ({ label: this.publicationMap.get(pId), y: fakeCount })
        )
          
    this.pubhlisherChart.options.data[0].dataPoints = realDataPoints;
    this.pubhlisherChart.options.data[1].dataPoints = fakeDataPoints;
    this.pubhlisherChart.render();
  }
}
