import { Directive, ElementRef, EventEmitter, HostListener, Input, Output, Renderer2 } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from 'src/app/base/base.component';
import { HttpClientService } from 'src/app/services/common/http-client.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DeleteDialogComponent, DeleteDialogState } from 'src/app/dialogs/delete-dialog/delete-dialog.component';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { HttpErrorResponse } from '@angular/common/http';

declare var $: any;

@Directive({
  selector: '[app-delete]'
})
export class DeleteDirective {

  constructor(
    private element: ElementRef,
    private _renderer: Renderer2,
    private httpClientService: HttpClientService,
    private spinner: NgxSpinnerService,
    public dialog: MatDialog,
    private alertify: AlertifyService
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
  @Input() controller: string;
  @Output() listRefreshCallback: EventEmitter<any> = new EventEmitter();

  @HostListener("click")
  async onClick() {
    this.openDialog(async () => {
      this.spinner.show(SpinnerType.SquareLoader);
      const td: HTMLTableCellElement = this.element.nativeElement;
      this.httpClientService.delete({
        controller: this.controller
      }, this.id).subscribe(data => {
        $(td.parentElement).addClass("bg-danger").animate({
          opacity: 0,
          height: "toggle"
        }, 2000, () => {
          this.listRefreshCallback.emit();
          this.alertify.notification("Deleted successfully!", MessageType.Success, Position.TopRight);
        });
      }, (errorResponse: HttpErrorResponse) => {
        this.spinner.hide(SpinnerType.SquareLoader);
        this.alertify.notification("An Unexpected error occurred while deleting!", MessageType.Error, Position.TopRight);
      });
    });
  }


  openDialog(afterClosed: any): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '250px',
      data: DeleteDialogState.Yes,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == DeleteDialogState.Yes) {
        afterClosed();
      }
    });
  }
}

