import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NgxFileDropEntry, FileSystemFileEntry, FileSystemDirectoryEntry } from 'ngx-file-drop';
import { FileUploadDialogComponent, FileUploadDialogState } from 'src/app/dialogs/file-upload-dialog/file-upload-dialog.component';
import { AlertifyService, MessageType, Position } from '../../admin/alertify.service';
import { DialogService } from '../../dialog.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../ui/CustomToastr.service';
import { HttpClientService } from '../http-client.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent {
  constructor(
    private httpClientService: HttpClientService,
    private aletifyService: AlertifyService,
    private customToastrService: CustomToastrService,
    private dialog: MatDialog,
    private dialogService: DialogService
  ) { }

  public files: NgxFileDropEntry[];

  @Input() options: Partial<FileUploadOptions>;

  public droppedFiles(files: NgxFileDropEntry[]) {
    this.files = files;
    const fileData: FormData = new FormData();
    for (const droppedFile of files) {
      (droppedFile.fileEntry as FileSystemFileEntry).file((_file: File) => {
        fileData.append(_file.name, _file, _file.name);
      });
    }

    this.dialogService.openDialog(
      {
        componentType: FileUploadDialogComponent,
        data: FileUploadDialogState.Yes,
        afterClosed: () => {
          this.httpClientService.post({
            controller: this.options.controller,
            action: this.options.action,
            queryString: this.options.queryString,
            headers: new HttpHeaders({ "responseType": "blob" })
          }, fileData).subscribe({
            next: (data) => {
              const successMessage: string = "Files uploaded succesfully!";
              if (this.options.isAdminPage) {
                this.aletifyService.notification(successMessage, MessageType.Success, Position.TopCenter);
              }
              else {
                this.customToastrService.notification(successMessage, "Upload is done!", ToastrMessageType.Success, ToastrPosition.TopCenter);
              }
            },
            error: (errorResponse: HttpErrorResponse) => {
              const errorMessage: string = "An unexpected error occurred while upload!";
              if (this.options.isAdminPage) {
                this.aletifyService.notification(errorMessage, MessageType.Error, Position.TopCenter);
              }
              else {
                this.customToastrService.notification(errorMessage, "Upload is failed!", ToastrMessageType.Error, ToastrPosition.TopCenter);
              }
            }
          });
        }
      });
  }
}

export class FileUploadOptions {
  controller?: string;
  action?: string;
  queryString?: string;
  explanation?: string;
  accept?: string;
  isAdminPage?: boolean = false;
}
