import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '@app/infrastructure/material.module';
import { SharedModule } from '@app/infrastructure/shared.module';
import { GaussianNaiveBayesComponent } from './components/gaussianaivebayes/gaussianaivebayes.component';
import { HomeComponent } from './components/home.component';
import { LinearDiscriminantAnalysisComponent } from './components/lineardiscriminantanalysis/lineardiscriminantanalysis.component';
import { LogisticRegressionComponent, LogisticRegressionDialog } from './components/logisticregression/logisticregression.component';
import { MultipleLinearRegressionComponent, MultipleLinearRegressionDialog } from './components/multiplelinearegression/multiplelinearegression.component';
import { PrincipalComponentAnalysisComponent, PrincipalComponentAnalysisDialog } from './components/principalcomponentanalysis/principalcomponentanalysis.component';
import { SimpleLinearRegressionComponent, SimpleLinearRegressionDialog } from './components/simplelinearegression/simplelinearegression.component';
import { HomeRoutes } from './home.routes';

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
        GaussianNaiveBayesComponent,
        LinearDiscriminantAnalysisComponent
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
