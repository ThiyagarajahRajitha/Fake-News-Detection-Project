import { News } from "./news.model";

export interface ReviewRequestedNewsListModel{
    id:number;
    comment:string;
    status:number;
    result:string;
    reviewFeedback:string;
    news:News;
    createdOn:Date;
    updatedOn:Date;
}