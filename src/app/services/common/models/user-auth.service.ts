import { SocialUser } from '@abacritt/angularx-social-login';
import { Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
import { TokenResponse } from 'src/app/contracts/token/tokenResponse';
import { LoginUser } from 'src/app/contracts/user/login-user';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../ui/CustomToastr.service';
import { HttpClientService } from '../http-client.service';

@Injectable({
  providedIn: 'root'
})
export class UserAuthService {

  constructor(private httpClientService: HttpClientService, private toastrService: CustomToastrService) { }

  async login(user: LoginUser, callBackFunction?: () => void): Promise<any> {
    const observable: Observable<any> = this.httpClientService.post({
      controller: "auth",
      action: "login"
    }, user);

    const tokenResponse = await firstValueFrom(observable).catch((error) => { console.log(error) }) as TokenResponse;
    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);

      this.toastrService.notification(
        "User login was successfull. Welcome! We hope you will enjoy spending time on eCommerce.",
        "Signed In!",
        ToastrMessageType.Success, ToastrPosition.TopRight)
    }
    callBackFunction();
  }

  async googleLogin(user: SocialUser, callBackFunction?: () => void) {
    debugger;
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      controller: "auth",
      action: "google-login"
    }, user);

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);

      this.toastrService.notification(
        "Google login was successfull. Welcome! We hope you will enjoy spending time on eCommerce.",
        "Signed In!",
        ToastrMessageType.Success, ToastrPosition.TopRight)
    }

    callBackFunction();
  }

  async facebookLogin(user: SocialUser, callBackFunction?: () => void) {
    debugger;
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      controller: "auth",
      action: "facebook-login"
    }, user);

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);

      this.toastrService.notification(
        "Facebook login was successfull. Welcome! We hope you will enjoy spending time on eCommerce.",
        "Signed In!",
        ToastrMessageType.Success, ToastrPosition.TopRight)
    }

    callBackFunction();
  }

  async vkLogin(user: SocialUser, callBackFunction?: () => void) {
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      controller: "auth",
      action: "vk-login"
    }, user);

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);

      this.toastrService.notification(
        "VK login was successfull. Welcome! We hope you will enjoy spending time on eCommerce.",
        "Signed In!",
        ToastrMessageType.Success, ToastrPosition.TopRight)
    }

    callBackFunction();
  }

  async microsoftLogin(user: SocialUser, callBackFunction?: () => void) {
    debugger;
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      controller: "auth",
      action: "microsoft-login"
    }, user);

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);

      this.toastrService.notification(
        "Microsoft login was successfull. Welcome! We hope you will enjoy spending time on eCommerce.",
        "Signed In!",
        ToastrMessageType.Success, ToastrPosition.TopRight)
    }

    callBackFunction();
  }
}
