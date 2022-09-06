import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ListProducts } from 'src/app/contracts/product/list-products';
import { ProductService } from 'src/app/services/common/models/product.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

  constructor(private productService: ProductService, private activatedRoute: ActivatedRoute) { }

  currentPageNo: number;
  products: ListProducts[];
  totalProductsCount: number;
  totalPageCount: number;
  pageSize: number = 2;
  baseUrl : string ="http://127.0.0.1:8887//";
  pageList: number[] = [];

  async ngOnInit() {
    this.activatedRoute.params.subscribe(async params => {
      this.currentPageNo = parseInt(params["pageNo"] ?? 1);
      const data: { totalProductsCount: number, products: ListProducts[] } = await
        this.productService.read(this.currentPageNo - 1, this.pageSize, () => {

        },
          errorMessage => {

          })
      this.products = data.products;
      this.totalProductsCount = data.totalProductsCount;
      this.totalPageCount = Math.ceil(this.totalProductsCount / this.pageSize);
      this.pageList = [];

      if (this.currentPageNo - 3 <= 0)
        for (let i = 1; i <= this.totalPageCount; i++)
          this.pageList.push(i);
      else if (this.currentPageNo + 3 >= this.totalPageCount)
        for (let i = this.totalPageCount - 6; i <= this.totalPageCount; i++) {
          if (i > 0)
            this.pageList.push(i);
        }
      else
        for (let i = this.currentPageNo - 3; i < this.currentPageNo + 3; i++)
          this.pageList.push(i);
    })
  }

}
