import { SocialUser } from '@abacritt/angularx-social-login';
import { Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
import { TokenResponse } from 'src/app/contracts/token/tokenResponse';
import { TwitterRequestToken } from 'src/app/contracts/token/twitterRequestToken';
import { TwitterResponseToken } from 'src/app/contracts/token/twitterResponseToken';
import { LoginUser } from 'src/app/contracts/user/login-user';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../ui/CustomToastr.service';
import { HttpClientService, RequestParameters } from '../http-client.service';

@Injectable({
  providedIn: 'root'
})
export class UserAuthService {

  constructor(private httpClientService: HttpClientService, private toastrService: CustomToastrService) { }

  async login(user: LoginUser, callBackFunction?: () => void) {
    const observable: Observable<any> = this.httpClientService.post({
      controller: "auth",
      action: "login"
    }, user);

    const tokenResponse = await firstValueFrom(observable).catch((error) => { console.log(error) }) as TokenResponse;
    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);
      localStorage.setItem("refreshToken", tokenResponse.token.refreshToken);

      this.toastrService.notification(
        "User login was successfull. Welcome! We hope you will enjoy spending time on eCommerce.",
        "Signed In!",
        ToastrMessageType.Success, ToastrPosition.TopRight)
    }
    callBackFunction();
  }

  async googleLogin(user: SocialUser, callBackFunction?: () => void) {
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      controller: "auth",
      action: "google-login"
    }, user);

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);
      localStorage.setItem("refreshToken", tokenResponse.token.refreshToken);

      this.toastrService.notification(
        "Google login was successfull. Welcome! We hope you will enjoy spending time on eCommerce.",
        "Signed In!",
        ToastrMessageType.Success, ToastrPosition.TopRight)
    }

    callBackFunction();
  }

  async facebookLogin(user: SocialUser, callBackFunction?: () => void) {
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      controller: "auth",
      action: "facebook-login"
    }, user);

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);
      localStorage.setItem("refreshToken", tokenResponse.token.refreshToken);

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
      localStorage.setItem("refreshToken", tokenResponse.token.refreshToken);

      this.toastrService.notification(
        "VK login was successfull. Welcome! We hope you will enjoy spending time on eCommerce.",
        "Signed In!",
        ToastrMessageType.Success, ToastrPosition.TopRight)
    }

    callBackFunction();
  }

  async microsoftLogin(user: SocialUser, callBackFunction?: () => void) {
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      controller: "auth",
      action: "microsoft-login"
    }, user);

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);
      localStorage.setItem("refreshToken", tokenResponse.token.refreshToken);

      this.toastrService.notification(
        "Microsoft login was successfull. Welcome! We hope you will enjoy spending time on eCommerce.",
        "Signed In!",
        ToastrMessageType.Success, ToastrPosition.TopRight)
    }

    callBackFunction();
  }

  async twitterLogin(oauth: TwitterResponseToken, callBackFunction?: () => void) {
    debugger;
    const observable: Observable<TwitterResponseToken | TokenResponse> = this.httpClientService.post<TwitterResponseToken | TokenResponse>({
      controller: "auth",
      action: "twitter-login"
    }, oauth);

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);
      localStorage.setItem("refreshToken", tokenResponse.token.refreshToken);

      this.toastrService.notification(
        "Twitter login was successfull. Welcome! We hope you will enjoy spending time on eCommerce.",
        "Signed In!",
        ToastrMessageType.Success, ToastrPosition.TopRight)
    }

    callBackFunction();
  }

  async refreshTokenLogin(refreshToken: string, callBackFunction?: () => void) {
    const observable: Observable<any | TokenResponse> = this.httpClientService.post<any | TokenResponse>({
      controller: "auth",
      action: "refresh-token-login"
    }, { refreshToken: refreshToken });

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);
      localStorage.setItem("refreshToken", tokenResponse.token.refreshToken);
    }

    callBackFunction();
  }

  getTwitterRequestToken(): Observable<TwitterRequestToken> {
    return this.httpClientService.get<TwitterRequestToken>({
      controller: "auth",
      action: "get-twitter-request-token"
    });
  }

}
