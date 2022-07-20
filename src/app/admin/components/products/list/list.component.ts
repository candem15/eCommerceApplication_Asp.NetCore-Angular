import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { ListProducts } from 'src/app/contracts/product/list-products';
import { SelectProductImagesDialogComponent } from 'src/app/dialogs/select-product-images-dialog/select-product-images-dialog.component';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { ProductService } from 'src/app/services/common/models/product.service';
import { DialogService } from 'src/app/services/dialog.service';

declare var $: any;

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent extends BaseComponent implements OnInit {

  constructor(private productService: ProductService,
    private dialogService: DialogService,
    spinner: NgxSpinnerService,
    private alertifyService: AlertifyService) {
    super(spinner);
  }

  displayedColumns: string[] = ['name', 'stock', 'price', 'createdDate', 'updatedDate', 'photos', 'editProduct', 'deleteProduct'];
  dataSource: MatTableDataSource<ListProducts> = null;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  async getProducts() {
    this.showSpinner(SpinnerType.SquareLoader);

    const allProducts: { totalProductsCount: number, products: ListProducts[] } = await this.productService.read(
      this.paginator ? this.paginator.pageIndex : 0,
      this.paginator ? this.paginator.pageSize : 5,
      () => this.hideSpinner(SpinnerType.SquareLoader),
      errorMessage => this.alertifyService.notification(errorMessage, MessageType.Error, Position.TopRight));

    this.dataSource = new MatTableDataSource<ListProducts>(allProducts.products);
    this.paginator.length = allProducts.totalProductsCount;
  }

  addProductImages(id: string) {
    this.dialogService.openDialog({
      componentType: SelectProductImagesDialogComponent,
      data: id,
      options: {
        width: "1000px"
      }
    })
  }

  async pageChanged() {
    await this.getProducts();
  }

  async ngOnInit() {
    await this.getProducts();
  }

}
