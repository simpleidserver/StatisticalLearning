import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '@app/infrastructure/material.module';
import { SharedModule } from '@app/infrastructure/shared.module';
import { HomeComponent } from './components/home.component';
import { LogisticRegressionComponent, LogisticRegressionDialog } from './components/logisticregression/logisticregression.component';
import { MultipleLinearRegressionComponent, MultipleLinearRegressionDialog } from './components/multiplelinearegression/multiplelinearegression.component';
import { PrincipalComponentAnalysisComponent, PrincipalComponentAnalysisDialog } from './components/principalcomponentanalysis/principalcomponentanalysis.component';
import { SimpleLinearRegressionComponent, SimpleLinearRegressionDialog } from './components/simplelinearegression/simplelinearegression.component';
import { HomeRoutes } from './home.routes';
import { GaussianNaiveBayesComponent } from './components/gaussianaivebayes/gaussianaivebayes.component';

@NgModule({
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
        PrincipalComponentAnalysisComponent,
        LogisticRegressionDialog,
        LogisticRegressionComponent,
        GaussianNaiveBayesComponent
    ],

    exports: [
        HomeComponent
    ],

    entryComponents: [
        SimpleLinearRegressionDialog,
        MultipleLinearRegressionDialog,
        PrincipalComponentAnalysisDialog,
        LogisticRegressionDialog
    ],

    providers: [ ]
})

export class HomeModule { }
