import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, MapOperator } from 'rxjs/internal/operators/map';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';

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
  memberCache = new Map();
  user: User;
  userParams: UserParams;



  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient, private accountService: AccountService) {

    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
    })
  }


  getUserParams() {
    return this.userParams;
  }
setUserParams(parms:UserParams ){
this.userParams = parms;
}

resetUserParams(){
  this.userParams = new UserParams(this.user);

  return this.userParams;
}

  getMembers(user: UserParams) {


    console.log(Object.values(user).join('-'));

    var response = this.memberCache.get(Object.values(user).join('-'));
    if (response) {
      return of(response);
    }

    let params = this.getPaginationHeaders(user.pageNumber, user.pageSize);

    params = params.append('minAge', user.minAge.toString());
    params = params.append('maxAge', user.maxAge.toString());
    params = params.append('gender', user.gender);
    params = params.append('orderBy', user.OrderBy);
    //httpHeaders
    return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params).pipe(
      map(response => {
        this.memberCache.set(Object.values(user).join('-'), response);

        return response;
      })
    )
  }


  private getPaginatedResult<T>(url, params) {

    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(responce => {
        paginatedResult.result = responce.body;

        if (responce.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(responce.headers.get('Pagination'));

        }

        return paginatedResult;
      })
    );
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();


    {
      params = params.append("pageNumber", pageNumber.toString());

      params = params.append("pageSize", pageSize.toString());

      // params = params.append("CurrentUsername",JSON.parse(localStorage.getItem('user'))?.username);
      return params;
    }
  }
  getMember(username: string) {


    const member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: Member) => member.username == username)

    console.log(member);

    if (member) {
      return of(member)
    }
    // const member = this.members.find(member => member.username === username);
    // if (member != undefined)
    //   return of(member);
    // console.log(localStorage.getItem('user'));
    return this.http.get<Member>(this.baseUrl + 'users/' + username, httpHeaders)
  }


  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member, httpHeaders).pipe(map(() => {
      const index = this.members.indexOf(member);
      this.members[index] = member;

    }))
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {}, httpHeaders);
  }

  DeletePhoto(photoId: number) {


    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId, httpHeaders);
  }

}
