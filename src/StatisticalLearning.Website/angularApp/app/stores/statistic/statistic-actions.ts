import { Action } from '@ngrx/store';
import { LinearRegressionResult } from './models/linear-regression.result';
import { PrincipalComponentAnalysisResult } from './models/pca.result';

export enum ActionTypes {
    COMPUTE_SIMPLE_LINEAR_REGRESSION = "[Statistic] COMPUTE_SIMPLE_LINEAR_REGRESSION",
    SIMPLE_LINEAR_REGRESSION_RESULT_LOADED = "[Statistic] LINEAR_REGRESSION_RESULT_LOADED",
    ERROR_LOAD_SIMPLE_LINEAR_REGRESSION = "[Statistic] ERROR_LOAD_SIMPLE_LINEAR_REGRESSION",
    COMPUTE_MULTIPLE_LINEAR_REGRESSION = "[Statistic] COMPUTE_MULTIPLE_LINEAR_REGRESSION",
    MULTIPLE_LINEAR_REGRESSION_RESULT_LOADED = "[Statistic] MULTIPLE_LINEAR_REGRESSION_RESULT_LOADED",
    ERROR_LOAD_MULTIPLE_LINEAR_REGRESSION = "[Statistic] ERROR_LOAD_MULTIPLE_LINEAR_REGRESSION",
    COMPUTE_PRINCIPAL_COMPONENT_ANALYSIS = "[Statistic] COMPUTE_PRINCIPAL_COMPONENT_ANALYSIS",
    PRINCIPAL_COMPONENT_LOADED = "[Statistic] PRINCIPAL_COMPONENT_LOADED",
    ERROR_LOAD_PRINCIPAL_COMPONENT_ANALYSIS = "[Statistic] ERROR_LOAD_PRINCIPAL_COMPONENT_ANALYSIS"
}

export class ComputeSimpleLinearRegression implements Action {
    readonly type = ActionTypes.COMPUTE_SIMPLE_LINEAR_REGRESSION;
    constructor(public inputs : Number[], public outputs: Number[]) { }
}

export class SimpleLinearRegressionLoaded implements Action {
    readonly type = ActionTypes.SIMPLE_LINEAR_REGRESSION_RESULT_LOADED;
    constructor(public simpleLinearRegressionResult: LinearRegressionResult) { }
}

export class ComputeMultipleLinearRegression implements Action {
    readonly type = ActionTypes.COMPUTE_MULTIPLE_LINEAR_REGRESSION;
    constructor(public inputs: Number[][], public outputs: Number[]) { }
}

export class MultipleLinearRegressionLoaded implements Action {
    readonly type = ActionTypes.MULTIPLE_LINEAR_REGRESSION_RESULT_LOADED;
    constructor(public multipleLinearRegressionResult: LinearRegressionResult) { }
}

export class ComputePrincipalComponentAnalysis implements Action {
    readonly type = ActionTypes.COMPUTE_PRINCIPAL_COMPONENT_ANALYSIS;
    constructor(public inputs: number[][]) { }
}

export class PrincipalComponentLoaded implements Action {
    readonly type = ActionTypes.PRINCIPAL_COMPONENT_LOADED;
    constructor(public principalComponentAnalysis: PrincipalComponentAnalysisResult) { }
}

export type ActionsUnion =
    ComputeSimpleLinearRegression |
    SimpleLinearRegressionLoaded | 
    ComputeMultipleLinearRegression |
    MultipleLinearRegressionLoaded |
    ComputePrincipalComponentAnalysis |
    PrincipalComponentLoaded;