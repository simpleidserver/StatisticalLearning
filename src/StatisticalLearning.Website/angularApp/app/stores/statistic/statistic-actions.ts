import { Action } from '@ngrx/store';
import { LinearRegressionResult } from './models/linear-regression.result';

export enum ActionTypes {
    COMPUTE_SIMPLE_LINEAR_REGRESSION = "[Statistic] COMPUTE_SIMPLE_LINEAR_REGRESSION",
    SIMPLE_LINEAR_REGRESSION_RESULT_LOADED = "[Statistic] LINEAR_REGRESSION_RESULT_LOADED",
    ERROR_LOAD_SIMPLE_LINEAR_REGRESSION = "[Statistic] ERROR_LOAD_SIMPLE_LINEAR_REGRESSION"
}

export class ComputeSimpleLinearRegression implements Action {
    readonly type = ActionTypes.COMPUTE_SIMPLE_LINEAR_REGRESSION;
    constructor(public inputs : Number[], public outputs: Number[]) { }
}

export class SimpleLinearRegressionLoaded implements Action {
    readonly type = ActionTypes.SIMPLE_LINEAR_REGRESSION_RESULT_LOADED;
    constructor(public simpleLinearRegressionResult: LinearRegressionResult) { }
}

export type ActionsUnion =
    ComputeSimpleLinearRegression |
    SimpleLinearRegressionLoaded;