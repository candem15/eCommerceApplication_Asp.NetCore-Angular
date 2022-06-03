import { EventEmitter } from '@angular/core';
import { Component, OnInit, Output } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { CreateProduct } from 'src/app/contracts/product/create-product';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { FileUploadOptions } from 'src/app/services/common/file-upload/file-upload.component';
import { ProductService } from 'src/app/services/common/models/product.service';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent extends BaseComponent implements OnInit {

  constructor(spinner: NgxSpinnerService, private productService: ProductService, private alertify: AlertifyService) {
    super(spinner)
  }

  ngOnInit(): void {
  }

  @Output() createdProduct: EventEmitter<CreateProduct> = new EventEmitter();

  @Output() fileUploadOptions: Partial<FileUploadOptions> = {
    controller: "products",
    action: "upload",
    explanation: "Drop or browse images to here...",
    isAdminPage: true,
    accept: ".png, .jpg, .jpeg, .tif, .tiff, .raw, .gif, .bmp"
  }

  create(name: HTMLInputElement, stock: HTMLInputElement, price: HTMLInputElement) {
    this.showSpinner(SpinnerType.SquareLoader);
    const createProduct: CreateProduct = new CreateProduct();
    createProduct.name = name.value;
    createProduct.stock = parseInt(stock.value);
    createProduct.price = parseFloat(price.value);

    this.productService.create(createProduct, () => {
      this.hideSpinner(SpinnerType.SquareLoader);
      this.alertify.notification("Product added successfully!",
        MessageType.Success,
        Position.TopRight
      );
      this.createdProduct.emit(createProduct);
    }, errorMessage => {
      this.alertify.notification(errorMessage, MessageType.Error, Position.TopRight);
    }
    );
  }

}
