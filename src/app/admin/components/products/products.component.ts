import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { HttpClientService } from 'src/app/services/common/http-client.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent extends BaseComponent implements OnInit {

  constructor(spinner: NgxSpinnerService, private httpClientService: HttpClientService) {
    super(spinner)
  }

  ngOnInit(): void {
    this.showSpinner(SpinnerType.SquareLoader);

    this.httpClientService.get({
      controller: "products"
    }).subscribe(data => console.log(data));

    /*this.httpClientService.post({
      controller: "products"
    }, {
      Name: "pro2",
      Stock: 50,
      Price: 25
    }).subscribe(data => console.log(data));*/

    /*this.httpClientService.put({
      controller: "products"
    }, {
      Id: "a607c613-8cb3-478a-bf17-b27c16b284e7",
      Name: "pro69",
      Stock: 69,
      Price: 31
    }).subscribe(data => console.log(data));*/

   /* this.httpClientService.delete({
      controller: "products"
    }, "7d019f20-fc6d-4450-92cd-f0140bb61470").subscribe();*/

  }
}
