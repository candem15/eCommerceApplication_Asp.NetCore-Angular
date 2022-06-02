import { Directive, ElementRef, EventEmitter, HostListener, Input, Output, Renderer2 } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from 'src/app/base/base.component';
import { ProductService } from 'src/app/services/common/models/product.service';

declare var $: any;

@Directive({
  selector: '[app-delete]'
})
export class DeleteDirective {

  constructor(
    private element: ElementRef,
    private _renderer: Renderer2,
    private productService: ProductService,
    private spinner:NgxSpinnerService
  ) {
    const button = _renderer.createElement('MatButton');
    const mat_icon = _renderer.createElement('mat-icon');
    button.setAttribute('style', 'color:red;');
    mat_icon.setAttribute("style", "cursor:pointer;");
    _renderer.addClass(mat_icon, 'mat-icon');
    _renderer.addClass(mat_icon, 'material-icons');
    _renderer.appendChild(mat_icon, _renderer.createText('delete'));
    _renderer.appendChild(button, mat_icon);
    _renderer.appendChild(element.nativeElement, button);
  }

  @Input() id: string;
  @Output() listRefreshCallback: EventEmitter<any> = new EventEmitter();

  @HostListener("click")
  async onClick() {
    this.spinner.show(SpinnerType.SquareLoader);
    const td: HTMLTableCellElement = this.element.nativeElement;
    await this.productService.delete(this.id);
    $(td.parentElement).fadeOut(2500, () => {
      this.listRefreshCallback.emit();
    });

  }
}
