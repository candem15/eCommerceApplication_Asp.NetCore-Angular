import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomersComponent } from './customers.component';



@NgModule({
  declarations: [
    CustomersComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    CustomersComponent
  ]
})
export class CustomersModule { }
