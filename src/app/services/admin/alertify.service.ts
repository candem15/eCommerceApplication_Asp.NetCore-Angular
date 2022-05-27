import { Injectable } from '@angular/core';
declare var alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  notification(message: string, messageType: MessageType, position: Position = Position.BottomRight, delay: number = 3) {
    alertify.set('notifier', 'position', delay);
    alertify.set('notifier', 'position', position);
    alertify[messageType](message);
  }

  setPosition(position: Position) {

  }
}

export enum MessageType {
  Error = "error",
  Success = "success",
  Notify = "notify",
  Warning = "warning",
  Message = "message"
}

export enum Position {
  TopRight = "top-right",
  TopCenter = "top-center",
  TopLeft = "top-left",
  BottomRight = "bottom-right",
  BottomCenter = "bottom-center",
  BottomLeft = "bottom-left"
}
