import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AdminModule } from './admin/admin.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UiModule } from './ui/ui.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { RegisterLoginComponent } from './ui/components/register-login/register-login.component';
import { FacebookLoginProvider, GoogleLoginProvider, MicrosoftLoginProvider, SocialAuthServiceConfig, SocialLoginModule, VKLoginProvider } from '@abacritt/angularx-social-login';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpErrorHandlerInterceptorService } from './services/common/http-error-handler-interceptor.service';
import { DynamicLoadComponentDirective } from './directives/common/dynamic-load-component.directive';


@NgModule({
  declarations: [
    AppComponent,
    RegisterLoginComponent,
    DynamicLoadComponentDirective
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AdminModule,
    UiModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    NgxSpinnerModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => localStorage.getItem("accessToken"),
        allowedDomains: ["localhost:7237"]
      }
    }),
    SocialLoginModule,
    ReactiveFormsModule
  ],
  providers: [
    { provide: "baseUrl", useValue: "https://localhost:7237/api", multi: true },
    {
      provide: "SocialAuthServiceConfig",
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider("864076638180-6ph1s0qgh6e5hh6kgj9ngj5epu74oc4t.apps.googleusercontent.com")
          },
          {
            id: FacebookLoginProvider.PROVIDER_ID,
            provider: new FacebookLoginProvider("463547378661649")
          },
          {
            id: VKLoginProvider.PROVIDER_ID,
            provider: new VKLoginProvider("51398063")
          },
          {
            id: MicrosoftLoginProvider.PROVIDER_ID,
            provider: new MicrosoftLoginProvider("6248bd08-f760-4a73-ba65-378a82b64741")
          }
        ],
        onError: err => console.log(err)
      } as SocialAuthServiceConfig
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorHandlerInterceptorService,
      multi: true
    }
  ],
  bootstrap: [AppComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
