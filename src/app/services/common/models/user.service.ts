import { Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
import { TokenResponse } from 'src/app/contracts/token/tokenResponse';
import { CreateUser } from 'src/app/contracts/user/create_user';
import { LoginUser } from 'src/app/contracts/user/login-user';
import { User } from 'src/app/entities/user';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../ui/CustomToastr.service';
import { HttpClientService } from '../http-client.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClientService: HttpClientService, private toastrService: CustomToastrService) { }

  async create(user: User): Promise<CreateUser> {
    const observable: Observable<CreateUser | User> = this.httpClientService.post<CreateUser | User>({
      controller: "users"
    }, user);

    return await firstValueFrom(observable) as CreateUser;
  }

  async login(user: LoginUser, callBackFunction?: () => void): Promise<any> {
    const observable: Observable<any> = this.httpClientService.post({
      controller: "users",
      action: "login"
    }, user);

    const tokenResponse = await firstValueFrom(observable) as TokenResponse;
    if (tokenResponse) {
      localStorage.setItem("accessToken",tokenResponse.token.accessToken);

      this.toastrService.notification(
        "User login was successfull. Welcome! We hope you will enjoy spending time on eCommerce.",
        "Signed In!",
        ToastrMessageType.Success, ToastrPosition.TopRight)
    }
    callBackFunction();
  }
}
