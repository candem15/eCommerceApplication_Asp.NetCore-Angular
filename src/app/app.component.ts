import { Component } from '@angular/core';
import { AuthService } from './services/common/auth.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from './services/ui/CustomToastr.service';
const http = new XMLHttpRequest();

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'eCommerceClient';
  constructor(
    public authService: AuthService,
    private toastrService: CustomToastrService) {
    this.authService.identityCheck();
  }

  signOut() {
    localStorage.removeItem("accessToken");
    this.authService.identityCheck();
    this.toastrService.notification(
      "Sign out is successfull.",
      "Logged out!",
      ToastrMessageType.Info,
      ToastrPosition.TopRight
    )
  }
}
