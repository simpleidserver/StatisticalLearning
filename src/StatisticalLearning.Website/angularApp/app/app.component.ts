import { Component, ViewEncapsulation } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

const statisticalLearningLanguageName: string = "statisticalLearningLanguage";

@Component({
    selector: 'app-component',
    templateUrl: './app.component.html',
    styleUrls: [
        './app.component.scss'
    ],
    encapsulation: ViewEncapsulation.None
})
export class AppComponent {
    activeLanguage: string = 'en';

    constructor(private translate: TranslateService) {
        this.activeLanguage = sessionStorage.getItem(statisticalLearningLanguageName);
        if (!this.activeLanguage) {
            this.activeLanguage = 'en';
        }

        translate.setDefaultLang(this.activeLanguage);
        translate.use(this.activeLanguage);
    }

    chooseLanguage(lng: string) {
        this.translate.use(lng);
        sessionStorage.setItem(statisticalLearningLanguageName, lng);
        this.activeLanguage = lng;
    }
}
