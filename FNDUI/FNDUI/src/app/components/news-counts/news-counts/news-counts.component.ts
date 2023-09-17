import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Publication } from 'src/app/models/Publication.model';
import { NewsClassificationCountsModel, PublisherDashboardModel, ReviewRequestCountDashboardModel } from 'src/app/models/news-classification-counts-model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { NewsService } from 'src/app/services/news.service';
import { PublisherApprovalService } from 'src/app/services/publisher-approval/publisher-approval.service';
import { UserStoreService } from 'src/app/services/user-store/user-store.service';

@Component({
  selector: 'app-news-counts',
  templateUrl: './news-counts.component.html',
  styleUrls: ['./news-counts.component.css']
})
export class NewsCountsComponent implements OnInit{

  public name: string = "";
  userId:String='';
  uid:number=0;
  fakeColor:string = "#FF8000";
  realColor:string = "#4682B4";
  from: Date = new Date(new Date().setDate(new Date().getDate() - 30));;
  to: Date = new Date();
  formattedFromDate:any;
  formattedToDate:any;
 
  publicationMap = new Map([
    [0 , "Unknown"]
  ]);

  moderatorMap = new Map([]);

  newsChart: any;
  rrChart:any;  
  newsChartOptions = {
    animationEnabled: true,
    theme: "light2",
  title: {
      text: "News classification Result"
    },
    data: [{
      type: "pie",
      dataPoints: [
      ]
    }] 
  }
  rrChartOptions = {
    animationEnabled: true,
    theme: "light2",
  title: {
      text: "Review Requests Result"
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
			dataPoints: []
		},
		{
			type: "line",
			name: "Real",
			showInLegend: true,
      color: this.realColor,
			dataPoints: []
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
                ]
	      }]
	}
  constructor(private newsService: NewsService, private publisherService:PublisherApprovalService,
    private datePipe: DatePipe, 
    private auth: AuthService, private router:Router, private userStore:UserStoreService){               
  };
  
  ngOnInit(): void {
    this.userId = this.auth.getprimarySidFromToken();
    this.userStore.getNameFromStore()
    .subscribe(val =>{
      let nameFromToken = this.auth.getNameFromToken();
      this.name = val || nameFromToken
    })
    this.uid = Number(this.userId);
    this.getPublication();
    this.getModerators();
    
    this.getNewsCount();
  }

  getPublication(){
    this.publisherService.getPublication()
    .subscribe({
      next:(publications) => {
        publications.forEach( (publication) => {
          var pubId=publication.publication_Id;
          var pubName = publication.publication_Name;

          this.publicationMap.set(pubId,pubName);
        });
      }
  })
  }

  getModerators(){
    this.newsService.getModerators('false')
    .subscribe({
      next:(moderators) => {
        moderators.forEach((moderator)=>{
          var mid = moderator.id;
          var modName = moderator.name;

          this.moderatorMap.set(mid,modName);
        })
      }
    })
  }

  getNewsChartInstance(chart: object) {
    this.newsChart = chart;
  }

  getReviewRequestsChartInstance(chart: object) {
    this.rrChart = chart;
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
    this.newsService.getNewsCount(this.uid,this.formattedFromDate, this.formattedToDate)
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
        this.getReviewRequestByModeratorCount();
        this.GetNewsCountByMonth();
      },
      error:(response) =>{
        console.log(response);
      }
    })

    this.newsService.getReviewRequestCount(this.uid,this.formattedFromDate, this.formattedToDate)
    .subscribe({
      next:(rrCounts) => {
        var dataPoints = [
          { label: "Pending",  y: rrCounts.reviewRending, color: this.fakeColor },
          { label: "Completed", y: rrCounts.reviewCompleted , color: this.realColor }
        ]
        this.rrChart.options.data[0].dataPoints = dataPoints;
        this.rrChart.render();
      },
      error:(response) =>{
        console.log(response);
      }
    })
  }

  getPublisherCount(){
        this.newsService.getNewsCountByPublisher(this.uid, this.formattedFromDate, this.formattedToDate)
        .subscribe({
          next:(newsCountsByPublication)=>{
            var realDataPoints = newsCountsByPublication.map(({ pid: pId, realCount }) => 
                ({ label: this.publicationMap.get(pId), y: realCount })
              );
            var fakeDataPoints = newsCountsByPublication.map(({ pid: pId, fakeCount }) => 
                  ({ label: this.publicationMap.get(pId), y: fakeCount })
                );
                  
            this.pubhlisherChart.options.data[0].dataPoints = realDataPoints;
            this.pubhlisherChart.options.data[1].dataPoints = fakeDataPoints;
            this.pubhlisherChart.render();
          },
          error:(response) =>{
            console.log(response);
          }
        })
  }

  getReviewRequestPublisherCount() {
    this.newsService.getReviewRequestCountByPublisher(this.uid, this.formattedFromDate, this.formattedToDate)
    .subscribe({
      next:(requestReviewCountsByPublication)=>{
    var realDataPoints = requestReviewCountsByPublication.map(({ pid: pId, reviewRending }) => 
            ({ label: this.publicationMap.get(pId), y: reviewRending })
          );
    var fakeDataPoints = requestReviewCountsByPublication.map(({ pid: pId, reviewCompleted }) => 
          ({ label: this.publicationMap.get(pId), y: reviewCompleted })
        )
        this.reviewPublisherChart.options.data[0].dataPoints = realDataPoints;
        this.reviewPublisherChart.options.data[1].dataPoints = fakeDataPoints;
        this.reviewPublisherChart.render();
      }
    })
  }


  getReviewRequestByModeratorCount() {
    this.newsService.getReviewRequestByModeratorCount(this.uid, this.formattedFromDate, this.formattedToDate)
    .subscribe({
      next:(requestReviewCountsByModerator)=>{  
      var realDataPoints = requestReviewCountsByModerator.map(({ mid, count }) => 
              ({ label: this.moderatorMap.get(mid), y: count })
            );
      
          this.reviewModeratorChart.options.data[0].dataPoints = realDataPoints;
          
          this.reviewModeratorChart.render();
        }
      })
  
  }  
  
  GetNewsCountByMonth(){
        this.newsService.getNewsCountByMonth(this.uid, this.formattedFromDate, this.formattedToDate)
        .subscribe({
          next:(newsCountsByMonth)=>{
            var realDataPoints = newsCountsByMonth.map(({ year, month, realCount }) => 
                ({ x: new Date(year, month), y: realCount })
              );
            var fakeDataPoints = newsCountsByMonth.map(({ year, month, fakeCount }) => 
                  ({ x: new Date(year, month), y: fakeCount })
                );
                  
            this.timeSeriesChart.options.data[0].dataPoints = realDataPoints;
            this.timeSeriesChart.options.data[1].dataPoints = fakeDataPoints;
            this.timeSeriesChart.render();
          },
          error:(response) =>{
            console.log(response);
          }
        })
  }
}
