import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { LinearRegressionResult } from '@app/stores/statistic/models/linear-regression.result';
import * as fromStatisticActions from '@app/stores/statistic/statistic-actions';
import { select, Store } from '@ngrx/store';
import { CoefficientResult } from '../../../stores/statistic/models/coefficient.result';
var Plotly = require('plotly.js-dist');

@Component({
    selector: 'simplelinearegression-component',
    templateUrl: './simplelinearegression.component.html',
    styleUrls: ['./simplelinearegression.component.scss']
})
export class SimpleLinearRegressionComponent implements OnInit {
    simpleLinearRegressionResult: LinearRegressionResult;
    inputs: number[] = [ 1, 2, 3, 6, 10 ];
    outputs: number[] = [1, 2, 3, 8, 11];

    constructor(private store: Store<fromAppState.AppState>, private dialog: MatDialog) { }

    ngOnInit(): void {
        this.store.pipe(select(fromAppState.selectSimpleLinearRegressionResult)).subscribe((simpleLinearRegressionResult: LinearRegressionResult) => {
            if (!simpleLinearRegressionResult) {
                return;
            }

            var scatterTrace : any =
            {
                x: [],
                y: [],
                mode: 'markers',
                type: 'scatter'
            };
            var lineTrace: any = {
                x: [],
                y: [],
                mode: 'lines',
                type: 'scatter'
            };
            this.simpleLinearRegressionResult = simpleLinearRegressionResult;
            var slope: number = simpleLinearRegressionResult.slope[0].value;
            var intercept: number = simpleLinearRegressionResult.intercept.value;
            var i = 0;
            for (var i = 0; i < this.inputs.length; i++) {
                var x: number = this.inputs[i];
                var y: number = this.outputs[i];
                var evalY: Number = (x * slope) + intercept;
                scatterTrace.x.push(x);
                scatterTrace.y.push(evalY);
                lineTrace.x.push(x);
                lineTrace.y.push(y);
            }

            Plotly.newPlot('chartContainer', [scatterTrace, lineTrace]);
        });
        this.refresh();
    }

    displayInfo() {
        this.dialog.open(SimpleLinearRegressionDialog, {
            width: '800px',
            data: this.simpleLinearRegressionResult
        });
    }

    refresh() {
        this.store.dispatch(new fromStatisticActions.ComputeSimpleLinearRegression(this.inputs, this.outputs));
    }

    roundDecimal() {
        this.simpleLinearRegressionResult.residualStandardError = this.round(this.simpleLinearRegressionResult.residualStandardError);
        this.simpleLinearRegressionResult.residualSumOfSquares = this.round(this.simpleLinearRegressionResult.residualSumOfSquares);
        this.simpleLinearRegressionResult.rSquare = this.round(this.simpleLinearRegressionResult.rSquare);
    }

    round(num : number) : number {
        return parseFloat(num.toFixed(4));
    }
}

class CoefficientResultView extends CoefficientResult {
    name: string;
}

@Component({
    selector: 'simplelinearegression-dialog',
    templateUrl: 'simplelinearegression-dialog.component.html',
})
export class SimpleLinearRegressionDialog {
    displayedColumns: string[] = ["name", "standardError", "tStatistic", "pValue"];
    coefficients: CoefficientResultView[] = [];
    constructor(public dialogRef: MatDialogRef<SimpleLinearRegressionDialog>, @Inject(MAT_DIALOG_DATA) public data: LinearRegressionResult) {
        const self = this;
        var i: number = 0;
        self.coefficients.push(self.convert(data.intercept, "Intercept"));
        data.slope.forEach(function (coef: CoefficientResult) {
            self.coefficients.push(self.convert(coef, "X" + i));
            i++;
        });
    }

    close() {
        this.dialogRef.close();
    }

    get equation() {
        var result : string = this.data.intercept.value.toString();
        var i: number = 0;
        this.data.slope.forEach(function (s: CoefficientResult) {
            result += " + " + s.value + "*X" + i;
        });

        return result;
    }

    private convert(coef : CoefficientResult, name: string): CoefficientResultView {
        let result = new CoefficientResultView();
        result.name = name;
        result.pValue = coef.pValue;
        result.standardError = coef.standardError;
        result.tStatistic = coef.tStatistic;
        result.value = coef.value;
        return result;
    }
}