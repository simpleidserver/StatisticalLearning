var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '@app/infrastructure/material.module';
import { SharedModule } from '@app/infrastructure/shared.module';
import { HomeComponent } from './components/home.component';
import { SimpleLinearRegressionComponent, SimpleLinearRegressionDialog } from './components/simplelinearegression/simplelinearegression.component';
import { MultipleLinearRegressionComponent, MultipleLinearRegressionDialog } from './components/multiplelinearegression/multiplelinearegression.component';
import { HomeRoutes } from './home.routes';
import { PrincipalComponentAnalysisDialog, PrincipalComponentAnalysisComponent } from './components/principalcomponentanalysis/principalcomponentanalysis.component';
var HomeModule = (function () {
    function HomeModule() {
    }
    HomeModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                HomeRoutes,
                MaterialModule,
                SharedModule
            ],
            declarations: [
                HomeComponent,
                SimpleLinearRegressionComponent,
                SimpleLinearRegressionDialog,
                MultipleLinearRegressionComponent,
                MultipleLinearRegressionDialog,
                PrincipalComponentAnalysisDialog,
                PrincipalComponentAnalysisComponent
            ],
            exports: [
                HomeComponent
            ],
            entryComponents: [
                SimpleLinearRegressionDialog,
                MultipleLinearRegressionDialog,
                PrincipalComponentAnalysisDialog
            ],
            providers: []
        })
    ], HomeModule);
    return HomeModule;
}());
export { HomeModule };
//# sourceMappingURL=home.module.js.map