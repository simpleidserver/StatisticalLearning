var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewEncapsulation } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
var statisticalLearningLanguageName = "statisticalLearningLanguage";
var AppComponent = (function () {
    function AppComponent(translate) {
        this.translate = translate;
        this.activeLanguage = 'en';
        this.activeLanguage = sessionStorage.getItem(statisticalLearningLanguageName);
        if (!this.activeLanguage) {
            this.activeLanguage = 'en';
        }
        translate.setDefaultLang(this.activeLanguage);
        translate.use(this.activeLanguage);
    }
    AppComponent.prototype.chooseLanguage = function (lng) {
        this.translate.use(lng);
        sessionStorage.setItem(statisticalLearningLanguageName, lng);
        this.activeLanguage = lng;
    };
    AppComponent = __decorate([
        Component({
            selector: 'app-component',
            templateUrl: './app.component.html',
            styleUrls: [
                './app.component.scss'
            ],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [TranslateService])
    ], AppComponent);
    return AppComponent;
}());
export { AppComponent };
//# sourceMappingURL=app.component.js.map