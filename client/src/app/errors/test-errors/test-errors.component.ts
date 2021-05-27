import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit {


  baseUrl = "https://localhost:5001/api/";
 validationErrors: string[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  Get404Error() 
  {

    this.http.get(this.baseUrl + 'buggy/not-found').subscribe(
      Response => {
        console.log(Response);
      },
      error => {
        console.log(error);
      }
    )
  }

  Get500Error() 
  {

    this.http.get(this.baseUrl + 'buggy/server-error').subscribe(
      Response => {
        console.log(Response);
      },
      error => {
        console.log(error);
      }
    )
  }

  Get401Error(){

this.http.get(this.baseUrl + "buggy/auth").subscribe(
  Response => {
    console.log(Response);
  },error => {
    console.log(error);
  }
)

  }

  Get400Error(){

    this.http.get(this.baseUrl+"buggy/bad-request").subscribe(
      Response => {
        console.log(Response);
      },error => {
        console.log(error);
      }
    )
    
      }

      Get400ValidationError(){

        this.http.post(this.baseUrl+"account/register",{}).subscribe(
          Response => {
            console.log(Response);
          },error => {
            console.log(error);
this.validationErrors = error;
          }
        )
        
          }


          

}
