import { Component, OnInit } from '@angular/core';
import { AbstractControl, UntypedFormBuilder, UntypedFormGroup, ValidationErrors, Validators, ValidatorFn } from '@angular/forms';
import { CreateUser } from 'src/app/contracts/user/create_user';
import { User } from 'src/app/entities/user';
import { UserService } from 'src/app/services/common/models/user.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from 'src/app/services/ui/CustomToastr.service';

declare var $: any;

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private formBuilder: UntypedFormBuilder, private toastrService: CustomToastrService, private userService: UserService) { }

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
  }

  get componentRegister() {
    return this.frmRegister.controls;
  }

  get componentLogin() {
    return this.frmLogin.controls;
  }

  onLoginSubmit(data: any) {
    this.loginSubmitted = true;
  }

  async onRegisterSubmit(user: User) {
    this.registerSubmitted = true;
    if (this.frmRegister.invalid)
      return;
    const result: CreateUser = await this.userService.create(user);
    if (result.succeeded) {
      this.toastrService.notification(result.message, "Success!", ToastrMessageType.Success, ToastrPosition.TopRight)
    }
    else {
      this.toastrService.notification(result.message, "Error!", ToastrMessageType.Error, ToastrPosition.TopRight)
    }
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

