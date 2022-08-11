import { FacebookLoginProvider, GoogleLoginProvider, MicrosoftLoginProvider, SocialAuthService, SocialUser, VKLoginProvider } from '@abacritt/angularx-social-login';
import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { CreateUser } from 'src/app/contracts/user/create_user';
import { LoginUser } from 'src/app/contracts/user/login-user';
import { User } from 'src/app/entities/user';
import { AuthService } from 'src/app/services/common/auth.service';
import { UserAuthService } from 'src/app/services/common/models/user-auth.service';
import { UserService } from 'src/app/services/common/models/user.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from 'src/app/services/ui/CustomToastr.service';

declare var $: any;

@Component({
  selector: 'app-register-login',
  templateUrl: './register-login.component.html',
  styleUrls: ['./register-login.component.scss']
})
export class RegisterLoginComponent extends BaseComponent implements OnInit {

  constructor(
    private formBuilder: UntypedFormBuilder,
    private toastrService: CustomToastrService,
    private userAuthService: UserAuthService,
    spinner: NgxSpinnerService,
    private authService: AuthService,
    private userService: UserService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private socialAuthService: SocialAuthService) {
    super(spinner);
    socialAuthService.authState.subscribe(async (user: SocialUser) => {
      console.log(user)
      this.showSpinner(SpinnerType.BallPulse)

      switch (user.provider) {
        case "GOOGLE":
          await this.userAuthService.googleLogin(user, () => {
            this.authService.identityCheck();
            this.hideSpinner(SpinnerType.BallPulse);
          })
          break;
        case "FACEBOOK":
          await this.userAuthService.facebookLogin(user, () => {
            this.authService.identityCheck();
            this.hideSpinner(SpinnerType.BallPulse);
          })
          break;
        case "VK":
          await this.userAuthService.vkLogin(user, () => {
            this.authService.identityCheck();
            this.hideSpinner(SpinnerType.BallPulse);
          })
          break;
        case "MICROSOFT":
          await this.userAuthService.microsoftLogin(user, () => {
            this.authService.identityCheck();
            this.hideSpinner(SpinnerType.BallPulse);
          })
          break;
      }
    });
  }

  frmLogin: UntypedFormGroup;
  frmRegister: UntypedFormGroup;
  registerSubmitted: boolean = false;
  loginSubmitted: boolean = false;

  ngOnInit(): void {
    this.frmRegister = this.formBuilder.group({
      name: ["",
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(50),
        ]],
      username: ["",
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(50)
        ]],
      email: ["",
        [
          Validators.required,
          Validators.email
        ]],
      password: ["",
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(50)
        ]],
      passwordConfirm: ["",
        [
          Validators.required
        ]]
    })
    this.frmLogin = this.formBuilder.group({
      usernameOrEmail: [""],
      password: [""]
    })
  }

  get componentRegister() {
    return this.frmRegister.controls;
  }

  get componentLogin() {
    return this.frmLogin.controls;
  }

  async onLoginSubmit(user: LoginUser) {
    this.loginSubmitted = true;
    this.showSpinner(SpinnerType.BallClipRotatePulse);
    await this.userAuthService.login(user, () => {
      this.authService.identityCheck();
      this.activatedRoute.queryParams.subscribe(params => {
        const returnUrl: string = params["returnUrl"];
        if (returnUrl)
          this.router.navigate([returnUrl]);
      })
      this.hideSpinner(SpinnerType.BallClipRotatePulse)
    });
  }

  async onRegisterSubmit(user: User) {
    this.registerSubmitted = true;
    if (this.frmRegister.invalid)
      return;
    const result: CreateUser = await this.userService.create(user);
    if (result.succeeded) {
      this.router.navigate(["/"]);
      this.toastrService.notification(result.message, "Success!", ToastrMessageType.Success, ToastrPosition.TopRight)
    }
    else {
      this.toastrService.notification(result.message, "Error!", ToastrMessageType.Error, ToastrPosition.TopRight)
    }
  }

  async facebookLogin() {
    this.socialAuthService.signIn(FacebookLoginProvider.PROVIDER_ID)
  }

  async googleLogin() {
  }

  async microsoftLogin() {
    this.socialAuthService.signIn(MicrosoftLoginProvider.PROVIDER_ID)
  }

  async vkLogin() {
    this.socialAuthService.signIn(VKLoginProvider.PROVIDER_ID)
  }

  async switchTabs(tabName: string) {

    if (tabName == "login") {
      $("#pills-register").attr("class", "tab-pane fade");
      $("#pills-login").attr("class", "tab-pane fade show active");
      $("#tab-login").attr("class", "nav-link active");
      $("#tab-register").attr("class", "nav-link");
    } else {
      $("#pills-login").attr("class", "tab-pane fade");
      $("#pills-register").attr("class", "tab-pane fade show active");
      $("#tab-register").attr("class", "nav-link active");
      $("#tab-login").attr("class", "nav-link");
    }
  }
}

