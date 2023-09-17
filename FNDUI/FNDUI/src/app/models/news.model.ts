export interface News{
    id:number;
    url?:string;
    topic:string;
    content:string;
    publisher_id?:number;
    classification_Decision:string;
    comment:string;
    status:number;
    reviewFeedback:string;
    reviewerName:string;
    result:string
    seeMore: boolean;
}