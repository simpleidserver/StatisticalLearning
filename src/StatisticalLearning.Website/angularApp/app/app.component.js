var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewEncapsulation } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import { OAuthService, JwksValidationHandler } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
import { MedikitExtensionService } from './infrastructure/services/medikitextension.service';
import { MatSnackBar, MatDialogRef, MatDialog } from '@angular/material';
import { FormGroup, FormControl, Validators } from '@angular/forms';
var medikitLanguageName = "medikitLanguage";
var AuthPinDialog = (function () {
    function AuthPinDialog(dialogRef) {
        this.dialogRef = dialogRef;
        this.authenticateFormGroup = new FormGroup({
            pin: new FormControl('', [
                Validators.required,
                Validators.pattern('^[0-9]{4}$')
            ])
        });
    }
    AuthPinDialog.prototype.close = function () {
        this.dialogRef.close();
    };
    AuthPinDialog.prototype.authenticate = function () {
        if (this.authenticateFormGroup.invalid) {
            return;
        }
        var pin = this.authenticateFormGroup.controls['pin'].value;
        this.dialogRef.close({ pin: pin });
    };
    AuthPinDialog = __decorate([
        Component({
            selector: 'auth-pin',
            templateUrl: './auth.pin.html'
        }),
        __metadata("design:paramtypes", [MatDialogRef])
    ], AuthPinDialog);
    return AuthPinDialog;
}());
export { AuthPinDialog };
var InstallExtensionHelpDialog = (function () {
    function InstallExtensionHelpDialog(dialogRef) {
        this.dialogRef = dialogRef;
        this.windowsUrl = process.env.REDIRECT_URL + "/assets/images/windows-os.svg";
        this.chromeUrl = process.env.REDIRECT_URL + "/assets/images/chrome.svg";
    }
    InstallExtensionHelpDialog.prototype.close = function () {
        this.dialogRef.close();
    };
    InstallExtensionHelpDialog = __decorate([
        Component({
            selector: 'install-extension-help',
            templateUrl: './install-extension-help.html',
            styleUrls: [
                './install-extension-help.scss'
            ],
        }),
        __metadata("design:paramtypes", [MatDialogRef])
    ], InstallExtensionHelpDialog);
    return InstallExtensionHelpDialog;
}());
export { InstallExtensionHelpDialog };
var AppComponent = (function () {
    function AppComponent(route, translate, router, oauthService, medikitExtensionService, snackBar, dialog) {
        this.route = route;
        this.translate = translate;
        this.router = router;
        this.oauthService = oauthService;
        this.medikitExtensionService = medikitExtensionService;
        this.snackBar = snackBar;
        this.dialog = dialog;
        this.logoUrl = process.env.REDIRECT_URL + "/assets/images/logo-no-text.svg";
        this.prescriptionsLogoUrl = process.env.REDIRECT_URL + "/assets/images/medical-prescription.png";
        this.medicalFilesLogoUrl = process.env.REDIRECT_URL + "/assets/images/patient-folder.png";
        this.sessionValidityHour = 0;
        this.isExtensionInstalled = false;
        this.isEhealthSessionActive = false;
        this.isConnected = false;
        this.expanded = false;
        this.activeLanguage = 'en';
        this.activeLanguage = sessionStorage.getItem(medikitLanguageName);
        if (!this.activeLanguage) {
            this.activeLanguage = 'en';
        }
        translate.setDefaultLang(this.activeLanguage);
        translate.use(this.activeLanguage);
        this.configureOAuth();
    }
    AppComponent.prototype.openHelpDialog = function () {
        this.dialog.open(InstallExtensionHelpDialog, {
            width: '600px'
        });
    };
    AppComponent.prototype.configureOAuth = function () {
        this.oauthService.configure(authConfig);
        this.oauthService.tokenValidationHandler = new JwksValidationHandler();
        var self = this;
        this.oauthService.loadDiscoveryDocumentAndTryLogin({
            disableOAuth2StateCheck: true
        });
        this.sessionCheckTimer = setInterval(function () {
            if (!self.oauthService.hasValidIdToken()) {
                self.oauthService.logOut();
                self.route.navigate(["/"]);
            }
        }, 3000);
    };
    AppComponent.prototype.login = function () {
        this.oauthService.customQueryParams = {
            'prompt': 'login'
        };
        this.oauthService.initImplicitFlow();
        return false;
    };
    AppComponent.prototype.chooseSession = function () {
        this.oauthService.customQueryParams = {
            'prompt': 'select_account'
        };
        this.oauthService.initImplicitFlow();
        return false;
    };
    AppComponent.prototype.disconnect = function () {
        this.oauthService.logOut();
        this.router.navigate(['/home']);
        return false;
    };
    AppComponent.prototype.init = function () {
        var claims = this.oauthService.getIdentityClaims();
        if (!claims) {
            this.isConnected = false;
            ;
            return;
        }
        this.name = claims.given_name;
        this.roles = claims.role;
        this.isConnected = true;
    };
    AppComponent.prototype.createEHealthSessionWithCertificate = function () {
        var _this = this;
        this.medikitExtensionService.createEhealthSessionWithCertificate().subscribe(function (_) {
            if (_) {
                _this.refreshEHealthSession();
                _this.snackBar.open(_this.translate.instant('ehealth-session-created'), _this.translate.instant('undo'), {
                    duration: 2000
                });
            }
            else {
                _this.snackBar.open(_this.translate.instant('ehealth-session-not-created'), _this.translate.instant('undo'), {
                    duration: 2000
                });
            }
        });
    };
    AppComponent.prototype.createEHealthSessionWithEID = function () {
        var _this = this;
        var self = this;
        var dialogRef = this.dialog.open(AuthPinDialog, {
            width: '400px'
        });
        dialogRef.afterClosed().subscribe(function (_) {
            if (!_) {
                return;
            }
            self.medikitExtensionService.createEhealthSessionWithEID(_.pin).subscribe(function (_) {
                if (_) {
                    self.refreshEHealthSession();
                    self.snackBar.open(_this.translate.instant('ehealth-session-created'), _this.translate.instant('undo'), {
                        duration: 2000
                    });
                }
                else {
                    self.snackBar.open(_this.translate.instant('ehealth-session-not-created'), _this.translate.instant('undo'), {
                        duration: 2000
                    });
                }
            });
        });
    };
    AppComponent.prototype.dropEhealthSession = function () {
        this.medikitExtensionService.disconnect();
        this.isEhealthSessionActive = false;
    };
    AppComponent.prototype.refreshEHealthSession = function () {
        var self = this;
        self.medikitExtensionService.isExtensionInstalled().subscribe(function (_) {
            self.isExtensionInstalled = _;
            if (self.isExtensionInstalled) {
                var session = self.medikitExtensionService.getEhealthSession();
                if (session === null) {
                    self.isEhealthSessionActive = false;
                }
                else {
                    self.isEhealthSessionActive = true;
                    var notOnOrAfter = new Date(session['not_onorafter']);
                    var notBefore = new Date();
                    var diff = Math.round(((notOnOrAfter.getTime() - notBefore.getTime()) / 36e5) * 100) / 100;
                    self.sessionValidityHour = diff;
                }
            }
        });
    };
    AppComponent.prototype.chooseLanguage = function (lng) {
        this.translate.use(lng);
        sessionStorage.setItem(medikitLanguageName, lng);
        this.activeLanguage = lng;
    };
    AppComponent.prototype.ngOnInit = function () {
        var _this = this;
        var self = this;
        this.init();
        this.oauthService.events.subscribe(function (e) {
            if (e.type === "logout") {
                _this.isConnected = false;
            }
            else if (e.type === "token_received") {
                _this.init();
            }
        });
        this.router.events.subscribe(function (opt) {
            var url = opt.urlAfterRedirects;
            if (!url || _this.expanded) {
                return;
            }
            if (url.startsWith('/prescription')) {
                _this.expanded = true;
            }
        });
        this.refreshEHealthSession();
        this.extensionCheckTimer = setInterval(function () {
            self.refreshEHealthSession();
        }, 3000);
    };
    AppComponent.prototype.ngOnDestroy = function () {
        if (this.extensionCheckTimer) {
            clearInterval(this.extensionCheckTimer);
        }
        if (this.sessionCheckTimer) {
            clearInterval(this.sessionCheckTimer);
        }
    };
    AppComponent = __decorate([
        Component({
            selector: 'app-component',
            templateUrl: './app.component.html',
            styleUrls: [
                './app.component.scss'
            ],
            encapsulation: ViewEncapsulation.None,
            animations: [
                trigger('indicatorRotate', [
                    state('collapsed', style({ transform: 'rotate(0deg)' })),
                    state('expanded', style({ transform: 'rotate(180deg)' })),
                    transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4,0.0,0.2,1)')),
                ])
            ]
        }),
        __metadata("design:paramtypes", [Router,
            TranslateService,
            Router,
            OAuthService,
            MedikitExtensionService,
            MatSnackBar,
            MatDialog])
    ], AppComponent);
    return AppComponent;
}());
export { AppComponent };
//# sourceMappingURL=app.component.js.map