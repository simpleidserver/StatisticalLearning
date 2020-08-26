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
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { LinearRegressionResult } from '@app/stores/statistic/models/linear-regression.result';
import * as fromStatisticActions from '@app/stores/statistic/statistic-actions';
import { select, Store } from '@ngrx/store';
import { CoefficientResult } from '../../../stores/statistic/models/coefficient.result';
var Plotly = require('plotly.js-dist');
var SimpleLinearRegressionComponent = (function () {
    function SimpleLinearRegressionComponent(store, dialog) {
        this.store = store;
        this.dialog = dialog;
        this.inputs = [1, 2, 3, 6, 10];
        this.outputs = [1, 2, 3, 8, 11];
    }
    SimpleLinearRegressionComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectSimpleLinearRegressionResult)).subscribe(function (simpleLinearRegressionResult) {
            if (!simpleLinearRegressionResult) {
                return;
            }
            var scatterTrace = {
                x: [],
                y: [],
                mode: 'markers',
                type: 'scatter'
            };
            var lineTrace = {
                x: [],
                y: [],
                mode: 'lines',
                type: 'scatter'
            };
            _this.simpleLinearRegressionResult = simpleLinearRegressionResult;
            var slope = simpleLinearRegressionResult.slope[0].value;
            var intercept = simpleLinearRegressionResult.intercept.value;
            var i = 0;
            for (var i = 0; i < _this.inputs.length; i++) {
                var x = _this.inputs[i];
                var y = _this.outputs[i];
                var evalY = (x * slope) + intercept;
                scatterTrace.x.push(x);
                scatterTrace.y.push(evalY);
                lineTrace.x.push(x);
                lineTrace.y.push(y);
            }
            Plotly.newPlot('chartContainer', [scatterTrace, lineTrace]);
        });
        this.refresh();
    };
    SimpleLinearRegressionComponent.prototype.displayInfo = function () {
        this.dialog.open(SimpleLinearRegressionDialog, {
            width: '800px',
            data: this.simpleLinearRegressionResult
        });
    };
    SimpleLinearRegressionComponent.prototype.refresh = function () {
        this.store.dispatch(new fromStatisticActions.ComputeSimpleLinearRegression(this.inputs, this.outputs));
    };
    SimpleLinearRegressionComponent.prototype.roundDecimal = function () {
        this.simpleLinearRegressionResult.residualStandardError = this.round(this.simpleLinearRegressionResult.residualStandardError);
        this.simpleLinearRegressionResult.residualSumOfSquares = this.round(this.simpleLinearRegressionResult.residualSumOfSquares);
        this.simpleLinearRegressionResult.rSquare = this.round(this.simpleLinearRegressionResult.rSquare);
    };
    SimpleLinearRegressionComponent.prototype.round = function (num) {
        return parseFloat(num.toFixed(4));
    };
    SimpleLinearRegressionComponent = __decorate([
        Component({
            selector: 'simplelinearegression-component',
            templateUrl: './simplelinearegression.component.html',
            styleUrls: ['./simplelinearegression.component.scss']
        }),
        __metadata("design:paramtypes", [Store, MatDialog])
    ], SimpleLinearRegressionComponent);
    return SimpleLinearRegressionComponent;
}());
export { SimpleLinearRegressionComponent };
var CoefficientResultView = (function (_super) {
    __extends(CoefficientResultView, _super);
    function CoefficientResultView() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return CoefficientResultView;
}(CoefficientResult));
var SimpleLinearRegressionDialog = (function () {
    function SimpleLinearRegressionDialog(dialogRef, data) {
        this.dialogRef = dialogRef;
        this.data = data;
        this.displayedColumns = ["name", "standardError", "tStatistic", "pValue"];
        this.coefficients = [];
        var self = this;
        var i = 0;
        self.coefficients.push(self.convert(data.intercept, "Intercept"));
        data.slope.forEach(function (coef) {
            self.coefficients.push(self.convert(coef, "X" + i));
            i++;
        });
    }
    SimpleLinearRegressionDialog.prototype.close = function () {
        this.dialogRef.close();
    };
    Object.defineProperty(SimpleLinearRegressionDialog.prototype, "equation", {
        get: function () {
            var result = this.data.intercept.value.toString();
            var i = 0;
            this.data.slope.forEach(function (s) {
                result += " + " + s.value + "*X" + i;
            });
            return result;
        },
        enumerable: false,
        configurable: true
    });
    SimpleLinearRegressionDialog.prototype.convert = function (coef, name) {
        var result = new CoefficientResultView();
        result.name = name;
        result.pValue = coef.pValue;
        result.standardError = coef.standardError;
        result.tStatistic = coef.tStatistic;
        result.value = coef.value;
        return result;
    };
    SimpleLinearRegressionDialog = __decorate([
        Component({
            selector: 'simplelinearegression-dialog',
            templateUrl: 'simplelinearegression-dialog.component.html',
        }),
        __param(1, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [MatDialogRef, LinearRegressionResult])
    ], SimpleLinearRegressionDialog);
    return SimpleLinearRegressionDialog;
}());
export { SimpleLinearRegressionDialog };
//# sourceMappingURL=simplelinearegression.component.js.map