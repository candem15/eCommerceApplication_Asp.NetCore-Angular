import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { catchError, Observable, of } from 'rxjs';
import { SpinnerType } from 'src/app/base/base.component';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../ui/CustomToastr.service';
import { UserAuthService } from './models/user-auth.service';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlerInterceptorService implements HttpInterceptor {

  constructor(private toastrService: CustomToastrService, private userAuthService: UserAuthService, private router: Router, private spinner: NgxSpinnerService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(catchError(error => {
      switch (error.status) {
        case HttpStatusCode.Unauthorized:
          this.userAuthService.refreshTokenLogin(localStorage.getItem("refreshToken"), (state) => {
            if (!state) {
              const url = this.router.url;
              if (url == "/products")
                this.toastrService.notification("Product can't added to your cart.", "Please login first!",
                  ToastrMessageType.Warning,
                  ToastrPosition.TopRight
                );
              else
                this.toastrService.notification("You don't have permission to this.", "Unauthorized!",
                  ToastrMessageType.Warning,
                  ToastrPosition.BottomFullWidth
                );
            }
          }).then(data => { });
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
      this.spinner.hide(SpinnerType.BallPulse);
      return of(error);
    }))
  }
}
