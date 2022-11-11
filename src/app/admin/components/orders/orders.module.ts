import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersComponent } from './orders.component';
import { RouterModule } from '@angular/router';
import { ListComponent } from './list/list.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { DialogModule } from 'src/app/dialogs/dialog.module';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { DeleteDirectiveModule } from 'src/app/directives/admin/delete.directive.module';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  declarations: [
    OrdersComponent,
    ListComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: "", component: OrdersComponent }
    ]),
    MatSidenavModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatTableModule, MatPaginatorModule, MatIconModule,
    DialogModule,
    DeleteDirectiveModule
  ],
  exports: [
    OrdersComponent
  ]
})
export class OrdersModule { }
