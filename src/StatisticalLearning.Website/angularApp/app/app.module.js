var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { OAuthModule } from 'angular-oauth2-oidc';
import { AppComponent, AuthPinDialog, InstallExtensionHelpDialog } from './app.component';
import { routes } from './app.routes';
import { HomeModule } from './home/home.module';
import { MaterialModule } from './infrastructure/material.module';
import { AddressService } from './infrastructure/services/address.service';
import { AuthGuard } from './infrastructure/services/auth-guard.service';
import { MedikitExtensionService } from './infrastructure/services/medikitextension.service';
import { SharedModule } from './infrastructure/shared.module';
import { ReferenceTableService } from './referencetable/services/reference-table-service';
import { appReducer } from './stores/appstate';
import { MedicinalProductService } from './stores/medicinalproduct/services/medicinalproduct-service';
import { PatientEffects } from './stores/patient/patient-effects';
import { PatientService } from './stores/patient/services/patient-service';
import { PharmaPrescriptionEffects } from './stores/pharmaprescription/prescription-effects';
export function createTranslateLoader(http) {
    var url = process.env.BASE_URL + 'assets/i18n/';
    return new TranslateHttpLoader(http, url, '.json');
}
var AppModule = (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        NgModule({
            imports: [
                RouterModule.forRoot(routes),
                SharedModule,
                MaterialModule,
                HomeModule,
                MatFormFieldModule,
                FlexLayoutModule,
                BrowserAnimationsModule,
                HttpClientModule,
                OAuthModule.forRoot(),
                EffectsModule.forRoot([PatientEffects, PharmaPrescriptionEffects]),
                StoreModule.forRoot(appReducer),
                StoreDevtoolsModule.instrument({
                    maxAge: 10
                }),
                TranslateModule.forRoot({
                    loader: {
                        provide: TranslateLoader,
                        useFactory: (createTranslateLoader),
                        deps: [HttpClient]
                    }
                })
            ],
            declarations: [
                AppComponent, AuthPinDialog, InstallExtensionHelpDialog
            ],
            entryComponents: [AuthPinDialog, InstallExtensionHelpDialog],
            bootstrap: [AppComponent],
            providers: [PatientService, MedicinalProductService, ReferenceTableService, MedikitExtensionService, AddressService, AuthGuard]
        })
    ], AppModule);
    return AppModule;
}());
export { AppModule };
//# sourceMappingURL=app.module.js.map