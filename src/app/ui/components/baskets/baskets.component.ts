import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { List_Basket_Item } from 'src/app/contracts/basket/list_basket_item';
import { Update_Basket_Item } from 'src/app/contracts/basket/update_basket_item';
import { Create_Order } from 'src/app/contracts/order/create_order';
import { BasketService } from 'src/app/services/common/models/basket.service';
import { OrderService } from 'src/app/services/common/models/order.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from 'src/app/services/ui/CustomToastr.service';

declare var $: any;

@Component({
  selector: 'app-baskets',
  templateUrl: './baskets.component.html',
  styleUrls: ['./baskets.component.scss']
})
export class BasketsComponent extends BaseComponent implements OnInit {

  constructor(spinner: NgxSpinnerService,
    private basketService: BasketService,
    private orderService: OrderService,
    private toastrService: CustomToastrService,
    private router:Router) {
    super(spinner)
  }

  basketItems: List_Basket_Item[];

  async ngOnInit() {
    this.showSpinner(SpinnerType.BallPulse)
    this.basketItems = await this.basketService.get()
    this.hideSpinner(SpinnerType.BallPulse)
  }

  async changeQuantity(object: any) {
    this.showSpinner(SpinnerType.BallPulse)
    const basketItemId: string = object.target.attributes["id"].value;
    const quantity: number = object.target.value;
    const basketItem: Update_Basket_Item = new Update_Basket_Item();
    basketItem.basketItemId = basketItemId;
    basketItem.quantity = quantity;
    await this.basketService.updateQuantity(basketItem);
    this.hideSpinner(SpinnerType.BallPulse)
  }

  async removeBasketItem(basketItemId: string) {
    this.showSpinner(SpinnerType.BallPulse);
    await this.basketService.remove(basketItemId);

    var a = $("." + basketItemId)
    $("." + basketItemId).fadeOut(500, () => this.hideSpinner(SpinnerType.BallPulse));
  }

  async confirmOrder() {
    this.showSpinner(SpinnerType.SquareLoader);
    const order: Create_Order = new Create_Order();
    order.address = "İzmir/Buca";
    order.description = "A4 Tech Webcam";
    await this.orderService.create(order)
    this.hideSpinner(SpinnerType.SquareLoader);
    this.toastrService.notification("Order completed.", "Success", ToastrMessageType.Info, ToastrPosition.TopCenter);
    this.router.navigate(["/"])
  }
}
