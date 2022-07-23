import { AfterViewChecked, AfterViewInit, Component, ElementRef, EventEmitter, Inject, Input, OnChanges, OnInit, Output, QueryList, ViewChildren } from '@angular/core';
import { MatCard } from '@angular/material/card';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BehaviorSubject, Observable, switchMap } from 'rxjs';
import { ListProductImage } from 'src/app/contracts/product/list-product-image';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { FileUploadOptions } from 'src/app/services/common/file-upload/file-upload.component';
import { ProductService } from 'src/app/services/common/models/product.service';
import { DialogService } from 'src/app/services/dialog.service';
import { BaseDialog } from '../base/base-dialog';
import { DeleteDialogComponent, DeleteDialogState } from '../delete-dialog/delete-dialog.component';

declare var $: any;

@Component({
  selector: 'app-select-product-images-dialog',
  templateUrl: './select-product-images-dialog.component.html',
  styleUrls: ['./select-product-images-dialog.component.scss']
})
export class SelectProductImagesDialogComponent extends BaseDialog<SelectProductImagesDialogComponent> implements OnInit {

  constructor(
    dialogRef: MatDialogRef<SelectProductImagesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ProductImagesDialogState | string,
    private productService: ProductService,
    private alertify: AlertifyService,
    private dialogService: DialogService) {
    super(dialogRef)
  }

  refreshImages$ = new BehaviorSubject<boolean>(true);

  @ViewChildren("cardElement", { read: ElementRef }) imageCardList: QueryList<ElementRef>;

  baseUrl = "http://127.0.0.1:8887/";
  productImages: Observable<ListProductImage[]>;

  async ngOnInit() {
    this.productImages = await this.refreshImages$.pipe(switchMap((_) => this.productService.readImages(this.data as string)));
  }

  async refreshImages() {
    this.refreshImages$.next(true);
  }

  async deleteImage(id: string, cardIndex: any) {
    this.dialogService.openDialog({
      componentType: DeleteDialogComponent,
      data: DeleteDialogState.Yes,
      afterClosed: async () => {
        await this.productService.deleteImage(id).then((_) => {
          var cardElement = this.imageCardList.find((item, index) => index === cardIndex);
          $(cardElement.nativeElement).fadeOut("slow");
          this.alertify.notification("Image deleted successfully!", MessageType.Success, Position.TopRight);
        }
        ).catch((_) => {
          this.alertify.notification("Unexpected error occurred while delete image!", MessageType.Error, Position.TopRight);
        });
      }
    });
  }

  @Output() options: Partial<FileUploadOptions> = {
    controller: "products",
    action: "upload",
    explanation: "Drop or browse images to here...",
    isAdminPage: true,
    accept: ".png, .jpg, .jpeg, .tif, .tiff, .raw, .gif, .bmp",
    queryString: `productId=${this.data}`
  }
}

export enum ProductImagesDialogState {
  Close
}
