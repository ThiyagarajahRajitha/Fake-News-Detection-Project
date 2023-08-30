export class ModeratorsVerifyModel{
    public username: string;
    public inviteCode: string;
    constructor(username:string, invitecode: string){
        this.username=username;
        this.inviteCode = invitecode;
        
    }
}