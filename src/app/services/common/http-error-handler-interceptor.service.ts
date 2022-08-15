import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of } from 'rxjs';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../ui/CustomToastr.service';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlerInterceptorService implements HttpInterceptor {

  constructor(private toastrService: CustomToastrService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(catchError(error => {
      switch (error.status) {
        case HttpStatusCode.Unauthorized:
          this.toastrService.notification("Authorization required!",
            "Unauthorized!",
            ToastrMessageType.Error,
            ToastrPosition.BottomRight);
          break;
        case HttpStatusCode.NotFound:
          this.toastrService.notification("Section you want to access is not available!",
            "Not Found!",
            ToastrMessageType.Error,
            ToastrPosition.BottomRight);
          break;
        case HttpStatusCode.InternalServerError:
          this.toastrService.notification("Something went wrong. Please try again later.",
            "Server Side Error!",
            ToastrMessageType.Error,
            ToastrPosition.BottomRight);
          break;
        default:
          this.toastrService.notification("An unexpedted error occurred. Please try again or contact support.",
            "Unknown Error!",
            ToastrMessageType.Error,
            ToastrPosition.BottomRight);
          break;
      }
      return of(error);
    }))
  }
}
