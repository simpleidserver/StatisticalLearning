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
import { LinearRegressionResult } from '@app/stores/statistic/models/linear-regression.result';
import * as fromStatisticActions from '@app/stores/statistic/statistic-actions';
import { select, Store } from '@ngrx/store';
import { CoefficientResult } from '../../../stores/statistic/models/coefficient.result';
var Plotly = require('plotly.js-dist');
var MultipleLinearRegressionComponent = (function () {
    function MultipleLinearRegressionComponent(store, dialog) {
        this.store = store;
        this.dialog = dialog;
        this.inputs = [
            [2.75, 5.3],
            [2.5, 5.3],
            [2.5, 5.3],
            [2.5, 5.3],
            [2.5, 5.4],
            [2.5, 5.6],
            [2.5, 5.5],
            [2.25, 5.5],
            [2.25, 5.5],
            [2.25, 5.6],
            [2, 5.7],
            [2, 5.9],
            [2, 6],
            [1.75, 5.9],
            [1.75, 5.8],
            [1.75, 6.1],
            [1.75, 6.2],
            [1.75, 6.1],
            [1.75, 6.1],
            [1.75, 6.1],
            [1.75, 5.9],
            [1.75, 6.2],
            [1.75, 6.2],
            [1.75, 6.1]
        ];
        this.outputs = [
            1464,
            1394,
            1357,
            1293,
            1256,
            1254,
            1234,
            1195,
            1159,
            1167,
            1130,
            1075,
            1047,
            965,
            943,
            958,
            971,
            949,
            884,
            866,
            876,
            822,
            704,
            719
        ];
    }
    MultipleLinearRegressionComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectMultipleLinearRegressionResult)).subscribe(function (multipleLinearRegressionResult) {
            if (!multipleLinearRegressionResult) {
                return;
            }
            var scatterTrace = {
                x: [],
                y: [],
                z: [],
                mode: 'markers',
                type: 'scatter3d'
            };
            var lineTrace = {
                x: [],
                y: [],
                z: [],
                type: 'mesh3d'
            };
            _this.multipleLinearRegressionResult = multipleLinearRegressionResult;
            var intercept = multipleLinearRegressionResult.intercept.value;
            var i = 0;
            for (var i = 0; i < _this.inputs.length; i++) {
                var observation = _this.inputs[i];
                var evalZ = intercept;
                for (var y = 0; y < observation.length; y++) {
                    var slope = multipleLinearRegressionResult.slope[y].value;
                    evalZ += observation[y] * slope;
                }
                lineTrace.x.push(observation[0]);
                lineTrace.y.push(observation[1]);
                lineTrace.z.push(evalZ);
                scatterTrace.x.push(observation[0]);
                scatterTrace.y.push(observation[1]);
                scatterTrace.z.push(_this.outputs[i]);
            }
            Plotly.newPlot('multipleLinearRegression', [scatterTrace, lineTrace]);
        });
        this.refresh();
    };
    MultipleLinearRegressionComponent.prototype.displayInfo = function () {
        this.dialog.open(MultipleLinearRegressionDialog, {
            width: '800px',
            data: this.multipleLinearRegressionResult
        });
    };
    MultipleLinearRegressionComponent.prototype.refresh = function () {
        this.store.dispatch(new fromStatisticActions.ComputeMultipleLinearRegression(this.inputs, this.outputs));
    };
    MultipleLinearRegressionComponent.prototype.round = function (num) {
        return parseFloat(num.toFixed(4));
    };
    MultipleLinearRegressionComponent = __decorate([
        Component({
            selector: 'multiplelinearegression-component',
            templateUrl: './multiplelinearegression.component.html',
            styleUrls: ['./multiplelinearegression.component.scss']
        }),
        __metadata("design:paramtypes", [Store, MatDialog])
    ], MultipleLinearRegressionComponent);
    return MultipleLinearRegressionComponent;
}());
export { MultipleLinearRegressionComponent };
var CoefficientResultView = (function (_super) {
    __extends(CoefficientResultView, _super);
    function CoefficientResultView() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return CoefficientResultView;
}(CoefficientResult));
var MultipleLinearRegressionDialog = (function () {
    function MultipleLinearRegressionDialog(dialogRef, data) {
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
    MultipleLinearRegressionDialog.prototype.close = function () {
        this.dialogRef.close();
    };
    Object.defineProperty(MultipleLinearRegressionDialog.prototype, "equation", {
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
    MultipleLinearRegressionDialog.prototype.convert = function (coef, name) {
        var result = new CoefficientResultView();
        result.name = name;
        result.pValue = coef.pValue;
        result.standardError = coef.standardError;
        result.tStatistic = coef.tStatistic;
        result.value = coef.value;
        return result;
    };
    MultipleLinearRegressionDialog = __decorate([
        Component({
            selector: 'multiplelinearegression-dialog',
            templateUrl: 'multiplelinearegression-dialog.component.html',
        }),
        __param(1, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [MatDialogRef, LinearRegressionResult])
    ], MultipleLinearRegressionDialog);
    return MultipleLinearRegressionDialog;
}());
export { MultipleLinearRegressionDialog };
//# sourceMappingURL=multiplelinearegression.component.js.map