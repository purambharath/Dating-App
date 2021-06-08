import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, MapOperator } from 'rxjs/internal/operators/map';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

const httpHeaders =
{
  headers: new HttpHeaders({
    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
  })
}

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  members: Member[] = [];

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getMembers() {

    if (this.members.length > 0) return of(this.members)
    return this.http.get<Member[]>(this.baseUrl + "users", httpHeaders).pipe(
      map(
        members => {
          this.members = members;

          return members;
        }
      )
    )
  }

  getMember(username: string) {

    const member = this.members.find(member => member.username === username);
    if (member != undefined)
      return of(member);
    console.log(localStorage.getItem('user'));
    return this.http.get<Member>(this.baseUrl + 'users/' + username, httpHeaders)
  }


  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member, httpHeaders).pipe(map(() => {
      const index = this.members.indexOf(member);
      this.members[index] = member;

    }))
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId,{},httpHeaders);
  }

  DeletePhoto(photoId : number)
  {
    

    return this.http.delete(this.baseUrl+ 'users/delete-photo/'+ photoId,httpHeaders);
  }

}
