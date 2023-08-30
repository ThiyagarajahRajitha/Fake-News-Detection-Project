export interface Users{
    id:number,
    name:string,
    email:string,
    password_hash:string,
    created_at: Date|string,
    token:string,
    role:string,
    status:number
}