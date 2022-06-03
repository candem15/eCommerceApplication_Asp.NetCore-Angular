import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr'

@Injectable({
  providedIn: 'root'
})
export class CustomToastrService {

  constructor(private toastr: ToastrService) { }

  notification(message: string, title: string, messageType: ToastrMessageType, position: ToastrPosition) {
    this.toastr[messageType](message, title, { positionClass: position });
  }
}

export enum ToastrMessageType {
  Error = "error",
  Info = "info",
  Success = "success",
  Warning = "warning",
}

export enum ToastrPosition {
  TopRight = "toast-top-right",
  TopCenter = "toast-top-center",
  TopLeft = "toast-top-left",
  BottomRight = "toast-bottom-right",
  BottomCenter = "toast-bottom-center",
  BottomLeft = "toast-bottom-left",
  TopFullWidth = "toast-top-full-width",
  BottomFullWidth = "toast-bottom-full-width"
}
