import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { StatisticService } from './services/statistic-service';
import { ActionTypes, ComputeLogisticRegression, ComputeMultipleLinearRegression, ComputePrincipalComponentAnalysis, ComputeSimpleLinearRegression, ComputeGaussianNaiveBayes } from './statistic-actions';

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
                const inputs: any[] = [];
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

    @Effect()
    computePrincipalComponentAnalysis$ = this.actions$
        .pipe(
            ofType(ActionTypes.COMPUTE_PRINCIPAL_COMPONENT_ANALYSIS),
            mergeMap((evt: ComputePrincipalComponentAnalysis) => {
                return this.statisticService.computePrincipalComponentAnalysis(evt.inputs)
                    .pipe(
                        map(principalComponentAnalysis => { return { type: ActionTypes.PRINCIPAL_COMPONENT_LOADED, principalComponentAnalysis: principalComponentAnalysis }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_LOAD_PRINCIPAL_COMPONENT_ANALYSIS }))
                    );
            }
            )
    );

    @Effect()
    computeLogisticRegression$ = this.actions$
        .pipe(
            ofType(ActionTypes.COMPUTE_LOGISTIC_REGRESSION),
            mergeMap((evt: ComputeLogisticRegression) => {
                return this.statisticService.computeLogisticRegression(evt.inputs, evt.outputs)
                    .pipe(
                        map(logisticRegression => { return { type: ActionTypes.LOGISTIC_REGRESSION_LOADED, logisticRegression: logisticRegression }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_LOAD_LOGISTIC_REGRESSION }))
                    );
            }
            )
    );

    @Effect()
    computeGaussianNaiveBayesRegression$ = this.actions$
        .pipe(
            ofType(ActionTypes.COMPUTE_GAUSSIAN_NAIVEBAYES),
            mergeMap((evt: ComputeGaussianNaiveBayes) => {
                return this.statisticService.computeGaussianNaiveBayes(evt.inputs, evt.predict, evt.outputs)
                    .pipe(
                        map(gaussianNaiveBayes => { return { type: ActionTypes.GAUSSIAN_NAIVEBAYES_LOADED, gaussianNaiveBayes: gaussianNaiveBayes }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_LOAD_GAUSSIAN_NAIVEBAYES }))
                    );
            }
            )
        );

}