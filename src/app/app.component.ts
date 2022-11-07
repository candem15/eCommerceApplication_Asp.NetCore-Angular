import { SocialAuthService, SocialUser } from '@abacritt/angularx-social-login';
import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicLoadComponentDirective } from './directives/common/dynamic-load-component.directive';
import { AuthService } from './services/common/auth.service';
import { ComponentType, DynamicLoadComponentService } from './services/common/dynamic-load-component.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from './services/ui/CustomToastr.service';
const http = new XMLHttpRequest();

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'eCommerceClient';
  @ViewChild(DynamicLoadComponentDirective, { static: true })
  dynamicLoadComponentDirective: DynamicLoadComponentDirective;
  constructor(
    public authService: AuthService,
    private toastrService: CustomToastrService,
    public socialAuthService: SocialAuthService,
    private router: Router,
    private dynamicLoadComponentService: DynamicLoadComponentService) {
    this.authService.identityCheck();
  }

  signOut() {
    this.socialAuthService.signOut(true);
    this.socialAuthService.authState.subscribe(async (user: SocialUser) => {
      user.provider = null;
    })
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    this.authService.identityCheck();
    this.toastrService.notification(
      "Sign out is successfull.",
      "Logged out!",
      ToastrMessageType.Info,
      ToastrPosition.TopRight
    )
  }

  loadComponent() {
    this.dynamicLoadComponentService.loadComponent(ComponentType.BasketsComponent, this.dynamicLoadComponentDirective.viewContainerRef);
  }
}
