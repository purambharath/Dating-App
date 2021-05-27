import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { error } from 'selenium-webdriver';
import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toastr : ToastrService,  private route : Router ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(catchError(error => {
       if(error)
       {
         switch (error.status) {
           case 400:
           
           if(error.error.errors){
            console.log(error.error.errors);
             var modelStateErrors = [];
             for(const key in error.error.errors){

           
               if(error.error.errors[key])
               {
               modelStateErrors.push(error.error.errors[key]);
                          
               }

             }

             throw modelStateErrors.flat();
           }
           else {
             this.toastr.error(error.statusText,error.status);
           }
             
             break;
         case 401 :

         this.toastr.error(error.statusText,error.status);
           break;

           case 404 :
             this.route.navigateByUrl('/not-found');
             break;
             case 500 :
               const navigationExtras: NavigationExtras = { state : {error : error.error}};
               this.route.navigateByUrl('/server-error', navigationExtras);
               break;
           default:

           this.toastr.error('some thing unexpected went wrong');
           console.log(error);
             break;
         }
       }
       return throwError(error)
    }
    
    ))
  }
}
