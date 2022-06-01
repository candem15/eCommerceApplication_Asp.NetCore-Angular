import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { firstValueFrom } from "rxjs";
import { CreateProduct } from "src/app/contracts/product/create-product";
import { ListProducts } from "src/app/contracts/product/list-products";
import { HttpClientService } from "../http-client.service";

@Injectable({
  providedIn: 'root'
})

export class ProductService {
  constructor(private httpClientService: HttpClientService) { }

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
}
