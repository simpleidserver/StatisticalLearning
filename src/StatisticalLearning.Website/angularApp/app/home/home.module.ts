import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '@app/infrastructure/material.module';
import { SharedModule } from '@app/infrastructure/shared.module';
import { HomeComponent } from './components/home.component';
import { SimpleLinearRegressionComponent, SimpleLinearRegressionDialog } from './components/simplelinearegression/simplelinearegression.component';
import { MultipleLinearRegressionComponent, MultipleLinearRegressionDialog } from './components/multiplelinearegression/multiplelinearegression.component';
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
        MultipleLinearRegressionDialog
    ],

    exports: [
        HomeComponent
    ],

    entryComponents: [
        SimpleLinearRegressionDialog,
        MultipleLinearRegressionDialog
    ],

    providers: [ ]
})

export class HomeModule { }
