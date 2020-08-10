import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { StatisticService } from './services/statistic-service';
import { ActionTypes, ComputeMultipleLinearRegression, ComputeSimpleLinearRegression } from './statistic-actions';

@Injectable()
export class StatisticEffects {
    constructor(
        private actions$: Actions,
        private statisticService: StatisticService,
    ) { }

    @Effect()
    computeSimpleLinearRegression$ = this.actions$
        .pipe(
            ofType(ActionTypes.COMPUTE_SIMPLE_LINEAR_REGRESSION),
            mergeMap((evt: ComputeSimpleLinearRegression) => {
                var inputs : any = [];
                evt.inputs.forEach(function (i: number) {
                    inputs.push([i]);
                });
                return this.statisticService.computeSimpleLinearRegression(inputs, evt.outputs)
                    .pipe(
                        map(simpleLinearRegressionResult => { return { type: ActionTypes.SIMPLE_LINEAR_REGRESSION_RESULT_LOADED, simpleLinearRegressionResult: simpleLinearRegressionResult }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_LOAD_SIMPLE_LINEAR_REGRESSION }))
                    );
            }
            )
    );

    @Effect()
    computeMultipleLinearRegression$ = this.actions$
        .pipe(
            ofType(ActionTypes.COMPUTE_MULTIPLE_LINEAR_REGRESSION),
            mergeMap((evt: ComputeMultipleLinearRegression) => {
                return this.statisticService.computeSimpleLinearRegression(evt.inputs, evt.outputs)
                    .pipe(
                        map(multipleLinearRegressionResult => { return { type: ActionTypes.MULTIPLE_LINEAR_REGRESSION_RESULT_LOADED, multipleLinearRegressionResult: multipleLinearRegressionResult }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_LOAD_MULTIPLE_LINEAR_REGRESSION }))
                    );
            }
            )
        );
}