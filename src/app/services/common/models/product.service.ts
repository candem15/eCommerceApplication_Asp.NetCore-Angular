import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, firstValueFrom, map, Observable } from "rxjs";
import { CreateProduct } from "src/app/contracts/product/create-product";
import { ListProducts } from "src/app/contracts/product/list-products";
import { ListProductImage } from "src/app/contracts/product/list-product-image";
import { HttpClientService } from "../http-client.service";
import { AlertifyService, MessageType, Position } from "../../admin/alertify.service";
import { EditProduct } from "src/app/contracts/product/edit-product";

@Injectable({
  providedIn: 'root'
})

export class ProductService {
  constructor(private httpClientService: HttpClientService, private alertifyService: AlertifyService) { }

  create(product: CreateProduct, successCallBack?: () => void, errorCallBack?: (errorMessage: string) => void) {
    this.httpClientService.post({
      controller: "products"
    }, product)
      .subscribe(result => {
        successCallBack();
      }, (errorResponse: HttpErrorResponse) => {
        const _error: Array<{ key: string, value: Array<string> }> = errorResponse.error;
        let message = "";
        _error.forEach((v, index) => {
          v.value.forEach((_v, index) => {
            message += `${_v}<br>`;
          });
        });
        errorCallBack(message);
      });
  }

  async read(
    page: number = 0,
    size: number = 5,
    successCallBack?: () => void,
    errorCallBack?: (errorMessage: string) => void): Promise<{ totalProductsCount: number, products: ListProducts[] }> {
    const promiseData: Promise<{ totalProductsCount: number, products: ListProducts[] }> =
      firstValueFrom(
        this.httpClientService.get<{ totalProductsCount: number, products: ListProducts[] }>({
          controller: "products",
          queryString: `page=${page}&size=${size}`
        }));

    promiseData.then(_ => successCallBack())
      .catch((errorResponse: HttpErrorResponse) => errorCallBack(errorResponse.message));

    return await promiseData;
  }

  async delete(id: string) {
    const deleteObservable: Observable<any> = this.httpClientService.delete({
      controller: "products"
    }, id);
    return await firstValueFrom(deleteObservable);
  }

  async deleteImage(id: string) {
    const deleteObservable: Observable<any> = this.httpClientService.delete({
      controller: "products",
      action: "DeleteImage"
    }, id);
    return await firstValueFrom(deleteObservable);
  }

  async readImages(id: string): Promise<ListProductImage[]> {
    const observableData: Observable<ListProductImage[]> = this.httpClientService.get<ListProductImage[]>({
      action: "getimages",
      controller: "products",
    }, id)
    return await firstValueFrom(observableData);
  }

  async editProduct(product: EditProduct, successCallBack?: () => void, errorCallBack?: (errorMessage: string) => void) {
    this.httpClientService.put({
      controller: "products",
      action:"EditProduct"
    }, product)
      .subscribe(result => {
        successCallBack();
      }, (errorResponse: HttpErrorResponse) => {
        const _error: Array<{ key: string, value: Array<string> }> = errorResponse.error;
        let message = "";
        _error.forEach((v, index) => {
          v.value.forEach((_v, index) => {
            message += `${_v}<br>`;
          });
        });
        errorCallBack(message);
      });
    }
}
