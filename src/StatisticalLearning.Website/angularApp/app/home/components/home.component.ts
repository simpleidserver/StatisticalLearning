import { Component, OnInit } from '@angular/core';
import * as fromAppState from '@app/stores/appstate';
import { Store, select } from '@ngrx/store';
import { LinearRegressionResult } from '@app/stores/statistic/models/linear-regression.result';
import * as fromStatisticActions from '@app/stores/statistic/statistic-actions';
declare var CanvasJS: any;

@Component({
    selector: 'app-home-component',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
    inputs: number[] = [ 1, 2, 3, 6, 10 ];
    outputs: number[] = [1, 2, 3, 8, 11];

    constructor(private store: Store<fromAppState.AppState>) {

    }

    ngOnInit(): void {
        this.store.pipe(select(fromAppState.selectSimpleLinearRegressionResult)).subscribe((simpleLinearRegressionResult: LinearRegressionResult) => {
            if (!simpleLinearRegressionResult) {
                return;
            }

            var scatterPoints = [];
            var linearEquationPoints = [];
            var slope: number = simpleLinearRegressionResult.slope[0].value;
            var intercept: number = simpleLinearRegressionResult.intercept.value;
            var i = 0;
            for (var i = 0; i < this.inputs.length; i++) {
                var x: number = this.inputs[i];
                var y: number = this.outputs[i];

                var evalY : Number = (x * slope) + intercept;
                scatterPoints.push({ x: x, y: y });
                linearEquationPoints.push({ x: x, y: evalY});
            }


            var chart = new CanvasJS.Chart("chartContainer", {
                animationEnabled: true,
                axisX: {
                    title: "TV"
                },
                axisY: {
                    title: "sales"
                },
                legend: {
                    cursor: "pointer",
                    fontSize: 16
                },
                toolTip: {
                    shared: true
                },
                data: [{
                    name: "Data points",
                    type: "scatter",
                    showInLegend: false,
                    dataPoints: scatterPoints
                }, {
                    name: "Simple linear regression",
                    type: "spline",
                    showInLegend: false,
                    dataPoints: linearEquationPoints
                }]
            });
            chart.render();
        });
        this.refresh();
    }

    refresh() {
        this.store.dispatch(new fromStatisticActions.ComputeSimpleLinearRegression(this.inputs, this.outputs));
    }
}
