import { Component, EventEmitter, Inject, OnInit, Output } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { EditProduct } from 'src/app/contracts/product/edit-product';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { ProductService } from 'src/app/services/common/models/product.service';
import { DialogService } from 'src/app/services/dialog.service';
import { BaseDialog } from '../base/base-dialog';
import { EditDialogComponent, EditDialogState } from '../edit-dialog/edit-dialog.component';

declare var $: any;

@Component({
  selector: 'app-edit-product-dialog',
  templateUrl: './edit-product-dialog.component.html',
  styleUrls: ['./edit-product-dialog.component.scss']
})
export class EditProductDialogComponent extends BaseDialog<EditProductDialogComponent> implements OnInit {

  constructor(
    dialogRef: MatDialogRef<EditProductDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditProduct,
    private dialogService: DialogService,
    private productService: ProductService,
    private alertify: AlertifyService) {
    super(dialogRef)
  }

  @Output() editedProduct: EventEmitter<EditProduct> = new EventEmitter();

  editProduct: Observable<EditProduct>;
  ngOnInit(): void {
    this.editProduct = new Observable(o => {
      o.next({
        id: this.data.id,
        name: this.data.name,
        stock: this.data.stock,
        price: this.data.price
      });
      o.complete();
    })
  }

  updateProduct(id: HTMLInputElement, name: HTMLInputElement, stock: HTMLInputElement, price: HTMLInputElement) {
    const editProduct: EditProduct = new EditProduct();
    editProduct.id = id.value;
    editProduct.name = name.value;
    editProduct.stock = Number(stock.value);
    editProduct.price = Number(price.value);

    this.dialogService.openDialog({
      componentType: EditDialogComponent,
      data: EditDialogState.Yes,
      afterClosed: async () => {
        await this.productService.editProduct(editProduct, () => {
          this.alertify.notification("Product edited successfully!",
            MessageType.Success,
            Position.TopRight
          );
          this.editedProduct.emit(editProduct);
        }, errorMessage => {
          this.alertify.notification(errorMessage, MessageType.Error, Position.TopRight);
        })
      }
    });
  }
}
