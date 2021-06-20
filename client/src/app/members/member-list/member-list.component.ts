  import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';




@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
members: Member[];

pagination: Pagination;
userParms: UserParams;
user:User;
genderList = [{value: 'male',display: 'Males'},{value: 'female',display: 'Females '}];

  constructor( private membersService: MembersService ) 
  { 
this.userParms = this.membersService.getUserParams();
   
  }


  resetFilters(){
    this.userParms = this.membersService.resetUserParams();
    this.loadmember();
  }
  ngOnInit(): void {
  this.loadmember();
  }
loadmember(){

  this.membersService.setUserParams(this.userParms);
  this.membersService.getMembers(this.userParms).subscribe(response => 
    {
      this.members = response.result;

      this.pagination = response.pagination;

    })
}
 
pageChanged(event: any )
{
this.userParms.pageNumber = event.page;

this.membersService.setUserParams(this.userParms);

this.loadmember();
}
}
