import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { LinearRegressionResult } from '@app/stores/statistic/models/linear-regression.result';
import * as fromStatisticActions from '@app/stores/statistic/statistic-actions';
import { select, Store } from '@ngrx/store';
import { CoefficientResult } from '../../../stores/statistic/models/coefficient.result';
var Plotly = require('plotly.js-dist');

@Component({
    selector: 'multiplelinearegression-component',
    templateUrl: './multiplelinearegression.component.html',
    styleUrls: ['./multiplelinearegression.component.scss']
})
export class MultipleLinearRegressionComponent implements OnInit {
    multipleLinearRegressionResult: LinearRegressionResult;
    inputs: number[][] =
    [
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
    outputs: number[] =
    [
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

    constructor(private store: Store<fromAppState.AppState>, private dialog: MatDialog) { }

    ngOnInit(): void {
        this.store.pipe(select(fromAppState.selectMultipleLinearRegressionResult)).subscribe((multipleLinearRegressionResult: LinearRegressionResult) => {
            if (!multipleLinearRegressionResult) {
                return;
            }

            var scatterTrace: any =
            {
                x: [],
                y: [],
                z: [],
                mode: 'markers',
                type: 'scatter3d'
            };
            var lineTrace: any = {
                x: [],
                y: [],
                z: [],
                type: 'mesh3d'
            };
            this.multipleLinearRegressionResult = multipleLinearRegressionResult;
            var intercept: number = multipleLinearRegressionResult.intercept.value;
            var i = 0;
            for (var i = 0; i < this.inputs.length; i++) {
                var observation: number[] = this.inputs[i];
                var evalZ: number = intercept;
                for (var y = 0; y < observation.length; y++)
                {
                    var slope = multipleLinearRegressionResult.slope[y].value;
                    evalZ += observation[y] * slope;
                }

                lineTrace.x.push(observation[0]);
                lineTrace.y.push(observation[1]);
                lineTrace.z.push(evalZ);
                scatterTrace.x.push(observation[0]);
                scatterTrace.y.push(observation[1]);
                scatterTrace.z.push(this.outputs[i]);
            }

            Plotly.newPlot('multipleLinearRegression', [scatterTrace, lineTrace]);
        });
        this.refresh();
    }

    displayInfo() {
        this.dialog.open(MultipleLinearRegressionDialog, {
            width: '800px',
            data: this.multipleLinearRegressionResult
        });
    }

    refresh() {
        this.store.dispatch(new fromStatisticActions.ComputeMultipleLinearRegression(this.inputs, this.outputs));
    }

    round(num : number) : number {
        return parseFloat(num.toFixed(4));
    }
}

class CoefficientResultView extends CoefficientResult {
    name: string;
}

@Component({
    selector: 'multiplelinearegression-dialog',
    templateUrl: 'multiplelinearegression-dialog.component.html',
})
export class MultipleLinearRegressionDialog {
    displayedColumns: string[] = ["name", "standardError", "tStatistic", "pValue"];
    coefficients: CoefficientResultView[] = [];
    constructor(public dialogRef: MatDialogRef<MultipleLinearRegressionDialog>, @Inject(MAT_DIALOG_DATA) public data: LinearRegressionResult) {
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