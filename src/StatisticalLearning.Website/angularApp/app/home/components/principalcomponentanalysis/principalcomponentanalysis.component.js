var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromStatisticActions from '@app/stores/statistic/statistic-actions';
import { select, Store } from '@ngrx/store';
import { PrincipalComponent } from '../../../stores/statistic/models/pca.result';
var Plotly = require('plotly.js-dist');
var PrincipalComponentAnalysisComponent = (function () {
    function PrincipalComponentAnalysisComponent(store, dialog) {
        this.store = store;
        this.dialog = dialog;
        this.names = ["100m", "Long jump", "Short jump", "High jump", "400m", "100m hurdle", "Discus", "Pole vault", "Javeline", "1500m"];
        this.playerNames = [
            "SEBRLE",
            "CLAY",
            "KARPOV",
            "BERNARD",
            "YURKOV",
            "WARNERS",
            "ZSIVOCZKY",
            "McMULLEN",
            "MARTINEAU",
            "HERNU",
            "BARRAS",
            "NOOL",
            "BOURGUIGNON",
            "Sebrle",
            "Clay",
            "Karpov",
            "Macey",
            "Warners",
            "Zsivoczky",
            "Hernu",
            "Nool",
            "Bernard",
            "Schwarzl",
            "Pogorelov",
            "Schoenbeck",
            "Barras",
            "Smith",
            "Averyanov",
            "Ojaniemi",
            "Smirnov",
            "Qi",
            "Drews",
            "Parkhomenko",
            "Terek",
            "Gomez",
            "Turi",
            "Lorenzo",
            "Karlivans",
            "Korkizoglou",
            "Uldal",
            "Casarsa"
        ];
        this.inputs = [
            [11.04, 7.58, 14.83, 2.07, 49.81, 14.69, 43.75, 5.02, 63.19, 291.70],
            [10.76, 7.40, 14.26, 1.86, 49.37, 14.05, 50.72, 4.92, 60.15, 301.50],
            [11.02, 7.30, 14.77, 2.04, 48.37, 14.09, 48.95, 4.92, 50.31, 300.20],
            [11.02, 7.23, 14.25, 1.92, 48.93, 14.99, 40.87, 5.32, 62.77, 280.10],
            [11.34, 7.09, 15.19, 2.10, 50.42, 15.31, 46.26, 4.72, 63.44, 276.40],
            [11.11, 7.60, 14.31, 1.98, 48.68, 14.23, 41.10, 4.92, 51.77, 278.10],
            [11.13, 7.30, 13.48, 2.01, 48.62, 14.17, 45.67, 4.42, 55.37, 268.00],
            [10.83, 7.31, 13.76, 2.13, 49.91, 14.38, 44.41, 4.42, 56.37, 285.10],
            [11.64, 6.81, 14.57, 1.95, 50.14, 14.93, 47.60, 4.92, 52.33, 262.10],
            [11.37, 7.56, 14.41, 1.86, 51.10, 15.06, 44.99, 4.82, 57.19, 285.10],
            [11.33, 6.97, 14.09, 1.95, 49.48, 14.48, 42.10, 4.72, 55.40, 282.00],
            [11.33, 7.27, 12.68, 1.98, 49.20, 15.29, 37.92, 4.62, 57.44, 266.60],
            [11.36, 6.80, 13.46, 1.86, 51.16, 15.67, 40.49, 5.02, 54.68, 291.70],
            [10.85, 7.84, 16.36, 2.12, 48.36, 14.05, 48.72, 5.00, 70.52, 280.01],
            [10.44, 7.96, 15.23, 2.06, 49.19, 14.13, 50.11, 4.90, 69.71, 282.00],
            [10.50, 7.81, 15.93, 2.09, 46.81, 13.97, 51.65, 4.60, 55.54, 278.11],
            [10.89, 7.47, 15.73, 2.15, 48.97, 14.56, 48.34, 4.40, 58.46, 265.42],
            [10.62, 7.74, 14.48, 1.97, 47.97, 14.01, 43.73, 4.90, 55.39, 278.05],
            [10.91, 7.14, 15.31, 2.12, 49.40, 14.95, 45.62, 4.70, 63.45, 269.54],
            [10.97, 7.19, 14.65, 2.03, 48.73, 14.25, 44.72, 4.80, 57.76, 264.35],
            [10.80, 7.53, 14.26, 1.88, 48.81, 14.80, 42.05, 5.40, 61.33, 276.33],
            [10.69, 7.48, 14.80, 2.12, 49.13, 14.17, 44.75, 4.40, 55.27, 276.31],
            [10.98, 7.49, 14.01, 1.94, 49.76, 14.25, 42.43, 5.10, 56.32, 273.56],
            [10.95, 7.31, 15.10, 2.06, 50.79, 14.21, 44.60, 5.00, 53.45, 287.63],
            [10.90, 7.30, 14.77, 1.88, 50.30, 14.34, 44.41, 5.00, 60.89, 278.82],
            [11.14, 6.99, 14.91, 1.94, 49.41, 14.37, 44.83, 4.60, 64.55, 267.09],
            [10.85, 6.81, 15.24, 1.91, 49.27, 14.01, 49.02, 4.20, 61.52, 272.74],
            [10.55, 7.34, 14.44, 1.94, 49.72, 14.39, 39.88, 4.80, 54.51, 271.02],
            [10.68, 7.50, 14.97, 1.94, 49.12, 15.01, 40.35, 4.60, 59.26, 275.71],
            [10.89, 7.07, 13.88, 1.94, 49.11, 14.77, 42.47, 4.70, 60.88, 263.31],
            [11.06, 7.34, 13.55, 1.97, 49.65, 14.78, 45.13, 4.50, 60.79, 272.63],
            [10.87, 7.38, 13.07, 1.88, 48.51, 14.01, 40.11, 5.00, 51.53, 274.21],
            [11.14, 6.61, 15.69, 2.03, 51.04, 14.88, 41.90, 4.80, 65.82, 277.94],
            [10.92, 6.94, 15.15, 1.94, 49.56, 15.12, 45.62, 5.30, 50.62, 290.36],
            [11.08, 7.26, 14.57, 1.85, 48.61, 14.41, 40.95, 4.40, 60.71, 269.70],
            [11.08, 6.91, 13.62, 2.03, 51.67, 14.26, 39.83, 4.80, 59.34, 290.01],
            [11.10, 7.03, 13.22, 1.85, 49.34, 15.38, 40.22, 4.50, 58.36, 263.08],
            [11.33, 7.26, 13.30, 1.97, 50.54, 14.98, 43.34, 4.50, 52.92, 278.67],
            [10.86, 7.07, 14.81, 1.94, 51.16, 14.96, 46.07, 4.70, 53.05, 317.00],
            [11.23, 6.99, 13.53, 1.85, 50.95, 15.09, 43.01, 4.50, 60.00, 281.70],
            [11.36, 6.68, 14.92, 1.94, 53.20, 15.39, 48.66, 4.40, 58.62, 296.12]
        ];
    }
    PrincipalComponentAnalysisComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectPrincipalComponentAnalysisResult)).subscribe(function (principalComponentResult) {
            if (!principalComponentResult) {
                return;
            }
            _this.principalComponentAnalysisResult = principalComponentResult;
            var pc1 = principalComponentResult.principalComponents[0];
            var pc2 = principalComponentResult.principalComponents[1];
            var correlationLayout = {
                title: 'Correlation circle',
                xaxis: { title: 'PC1 (' + parseFloat((pc1.proportion * 100).toFixed(4)) + '%)' },
                yaxis: { title: 'PC2 (' + parseFloat((pc2.proportion * 100).toFixed(4)) + '%)' },
                shapes: [
                    {
                        x0: -1,
                        x1: 1,
                        y0: -1,
                        y1: 1,
                        type: 'circle'
                    }
                ]
            };
            var invidLayout = {
                title: 'Individuals graph'
            };
            var correlationData = [];
            var indivData = [];
            var self = _this;
            for (var i = 0; i < _this.names.length; i++) {
                correlationData.push({
                    node: 'markers',
                    name: self.names[i],
                    x: [pc1.eigenvector[i]],
                    y: [pc2.eigenvector[i]]
                });
                correlationLayout.shapes.push({
                    type: 'line',
                    x0: 0,
                    x1: pc1.eigenvector[i],
                    y0: 0,
                    y1: pc2.eigenvector[i]
                });
            }
            for (var j = 0; j < _this.playerNames.length; j++) {
                var name_1 = _this.playerNames[j];
                indivData.push({
                    node: 'markers',
                    name: name_1,
                    x: [_this.principalComponentAnalysisResult.transformed[j][0]],
                    y: [_this.principalComponentAnalysisResult.transformed[j][1]]
                });
            }
            Plotly.newPlot('pcaCorrelationChartContainer', correlationData, correlationLayout);
            Plotly.newPlot('pcaIndivChartContainer', indivData, invidLayout);
        });
        this.refresh();
    };
    PrincipalComponentAnalysisComponent.prototype.displayInfo = function () {
        var data = new PrincipalComponentDialogView();
        var i = 0;
        this.principalComponentAnalysisResult.principalComponents.forEach(function (p) {
            var record = new PrincipalComponentView();
            record.cumulative = p.cumulative;
            record.eigenValue = p.eigenValue;
            record.eigenvector = p.eigenvector;
            record.proportion = p.proportion;
            record.singularValue = p.singularValue;
            record.name = "P" + i;
            data.components.push(record);
            i++;
        });
        data.data = this.inputs;
        this.dialog.open(PrincipalComponentAnalysisDialog, {
            width: '800px',
            data: data
        });
    };
    PrincipalComponentAnalysisComponent.prototype.refresh = function () {
        this.store.dispatch(new fromStatisticActions.ComputePrincipalComponentAnalysis(this.inputs));
    };
    PrincipalComponentAnalysisComponent.prototype.round = function (num) {
        return parseFloat(num.toFixed(4));
    };
    PrincipalComponentAnalysisComponent = __decorate([
        Component({
            selector: 'principalcomponentanalysis-component',
            templateUrl: './principalcomponentanalysis.component.html',
            styleUrls: ['./principalcomponentanalysis.component.scss']
        }),
        __metadata("design:paramtypes", [Store, MatDialog])
    ], PrincipalComponentAnalysisComponent);
    return PrincipalComponentAnalysisComponent;
}());
export { PrincipalComponentAnalysisComponent };
var PrincipalComponentView = (function (_super) {
    __extends(PrincipalComponentView, _super);
    function PrincipalComponentView() {
        return _super.call(this) || this;
    }
    return PrincipalComponentView;
}(PrincipalComponent));
var PrincipalComponentDialogView = (function () {
    function PrincipalComponentDialogView() {
        this.components = [];
    }
    return PrincipalComponentDialogView;
}());
var PrincipalComponentAnalysisDialog = (function () {
    function PrincipalComponentAnalysisDialog(dialogRef, data) {
        this.dialogRef = dialogRef;
        this.data = data;
        this.displayedColumns = ["name", "cumulative", "eigenValue", "proportion"];
        this.matrixDisplayedColumns = ["100m", "longJump", "shortJump", "highJump", "400m", "100mHurdle", "discus", "poleVault", "javeline", "1500m"];
    }
    PrincipalComponentAnalysisDialog.prototype.close = function () {
        this.dialogRef.close();
    };
    PrincipalComponentAnalysisDialog = __decorate([
        Component({
            selector: 'principalcomponentanalysis-dialog',
            templateUrl: 'principalcomponentanalysis-dialog.component.html',
        }),
        __param(1, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [MatDialogRef, PrincipalComponentDialogView])
    ], PrincipalComponentAnalysisDialog);
    return PrincipalComponentAnalysisDialog;
}());
export { PrincipalComponentAnalysisDialog };
//# sourceMappingURL=principalcomponentanalysis.component.js.map