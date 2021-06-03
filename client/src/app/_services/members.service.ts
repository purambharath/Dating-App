import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
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

baseUrl = environment.apiUrl;  
  constructor(private http: HttpClient) { }
  
  getMembers(){
    return this.http.get<Member[]>(this.baseUrl + "users",httpHeaders)
  }

  getMember(username : string)
  {

    console.log(localStorage.getItem('user'));
 return this.http.get<Member>(this.baseUrl + 'users/' + username,httpHeaders)
  }


}
