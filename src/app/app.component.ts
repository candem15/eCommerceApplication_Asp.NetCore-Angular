import { Component } from '@angular/core';
import { CustomToastrService, MessageType, ToastrPosition } from './services/ui/CustomToastr.service';
const http = new XMLHttpRequest();

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'eCommerceClient';
  constructor(private toastr: CustomToastrService) {
    toastr.notification("tebrikler", "100 aldın!", MessageType.Info,ToastrPosition.TopLeft);
  }
}
