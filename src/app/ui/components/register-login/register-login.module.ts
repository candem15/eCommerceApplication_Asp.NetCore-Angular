import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterLoginComponent } from './register-login.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    RegisterLoginComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: "", component: RegisterLoginComponent }
    ]),
    ReactiveFormsModule
  ]
})
export class RegisterLoginModule { }
