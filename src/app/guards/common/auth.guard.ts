import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { SpinnerType } from 'src/app/base/base.component';
import { _isAuthenticated } from 'src/app/services/common/auth.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from 'src/app/services/ui/CustomToastr.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private jwtHelper: JwtHelperService,
    private toastrService: CustomToastrService,
    private spinner: NgxSpinnerService,
    private router: Router) {

  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    this.spinner.show(SpinnerType.BallPulse);

    if (!_isAuthenticated) {
      this.toastrService.notification(
        "Please login first for access that area.",
        "Unauthorized access attempt!",
        ToastrMessageType.Warning,
        ToastrPosition.TopRight);

      this.router.navigate(["register"], { queryParams: { returnUrl: state.url } })
    }

    this.spinner.hide(SpinnerType.BallPulse);

    return true;
  }

}
