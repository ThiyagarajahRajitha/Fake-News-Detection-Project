export interface NewsClassificationCountsModel{
    realCount:number;
    fakeCount:number;
}

export interface ReviewRequestCountsModel{
    reviewRending:number;
    reviewCompleted:number;
}

export interface PublisherDashboardModel{
    realCount:number;
    fakeCount:number;
    pid:number;
}

export interface ReviewRequestCountDashboardModel{
    reviewRending:number;
    reviewCompleted:number;
    pid:number;
}

export interface ReviewRequestCountByModeratorDashboardModel{
    count:number;
    mid:number
}

export interface NewsCountByMonthDashboardModel{
    year:number;
    month:number;
    fakeCount: number,
    realCount: number
}