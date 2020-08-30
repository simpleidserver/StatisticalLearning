import { Action } from '@ngrx/store';
import { LinearRegressionResult } from './models/linear-regression.result';
import { PrincipalComponentAnalysisResult } from './models/pca.result';
import { LogisticRegressionResult } from './models/logistic-regression.result';
import { GaussianNaiveBayesResult } from './models/gaussian-naivebayes.result';

export enum ActionTypes {
    COMPUTE_SIMPLE_LINEAR_REGRESSION = "[Statistic] COMPUTE_SIMPLE_LINEAR_REGRESSION",
    SIMPLE_LINEAR_REGRESSION_RESULT_LOADED = "[Statistic] LINEAR_REGRESSION_RESULT_LOADED",
    ERROR_LOAD_SIMPLE_LINEAR_REGRESSION = "[Statistic] ERROR_LOAD_SIMPLE_LINEAR_REGRESSION",
    COMPUTE_MULTIPLE_LINEAR_REGRESSION = "[Statistic] COMPUTE_MULTIPLE_LINEAR_REGRESSION",
    MULTIPLE_LINEAR_REGRESSION_RESULT_LOADED = "[Statistic] MULTIPLE_LINEAR_REGRESSION_RESULT_LOADED",
    ERROR_LOAD_MULTIPLE_LINEAR_REGRESSION = "[Statistic] ERROR_LOAD_MULTIPLE_LINEAR_REGRESSION",
    COMPUTE_PRINCIPAL_COMPONENT_ANALYSIS = "[Statistic] COMPUTE_PRINCIPAL_COMPONENT_ANALYSIS",
    PRINCIPAL_COMPONENT_LOADED = "[Statistic] PRINCIPAL_COMPONENT_LOADED",
    ERROR_LOAD_PRINCIPAL_COMPONENT_ANALYSIS = "[Statistic] ERROR_LOAD_PRINCIPAL_COMPONENT_ANALYSIS",
    COMPUTE_LOGISTIC_REGRESSION = "[Statistic] COMPUTE_LOGISTIC_REGRESSION",
    LOGISTIC_REGRESSION_LOADED = "[Statistic] LOGISTIC_REGRESSION_LOADED",
    ERROR_LOAD_LOGISTIC_REGRESSION = "[Statistic] ERROR_LOAD_LOGISTIC_REGRESSION",
    COMPUTE_GAUSSIAN_NAIVEBAYES = "[Statistic] COMPUTE_GAUSSIAN_NAIVEBAYES",
    GAUSSIAN_NAIVEBAYES_LOADED = "[Statistic] GAUSSIAN_NAIVEBAYES_LOADED",
    ERROR_LOAD_GAUSSIAN_NAIVEBAYES = "[Statistic] ERROR_LOAD_GAUSSIAN_NAIVEBAYES"
}

export class ComputeSimpleLinearRegression implements Action {
    readonly type = ActionTypes.COMPUTE_SIMPLE_LINEAR_REGRESSION;
    constructor(public inputs: number[], public outputs: number[]) { }
}

export class SimpleLinearRegressionLoaded implements Action {
    readonly type = ActionTypes.SIMPLE_LINEAR_REGRESSION_RESULT_LOADED;
    constructor(public simpleLinearRegressionResult: LinearRegressionResult) { }
}

export class ComputeMultipleLinearRegression implements Action {
    readonly type = ActionTypes.COMPUTE_MULTIPLE_LINEAR_REGRESSION;
    constructor(public inputs: number[][], public outputs: number[]) { }
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

export class ComputeLogisticRegression implements Action {
    readonly type = ActionTypes.COMPUTE_LOGISTIC_REGRESSION;
    constructor(public inputs: number[][], public outputs : number[]) { }
}

export class LogisticRegressionLoaded implements Action {
    readonly type = ActionTypes.LOGISTIC_REGRESSION_LOADED;
    constructor(public logisticRegression: LogisticRegressionResult) { }
}

export class ComputeGaussianNaiveBayes implements Action {
    readonly type = ActionTypes.COMPUTE_GAUSSIAN_NAIVEBAYES;
    constructor(public inputs: number[][], public predict : number[], public outputs: number[]) { }
}

export class GaussianNaiveBayesLoaded implements Action {
    readonly type = ActionTypes.GAUSSIAN_NAIVEBAYES_LOADED;
    constructor(public gaussianNaiveBayes: GaussianNaiveBayesResult) { }
}

export type ActionsUnion =
    ComputeSimpleLinearRegression |
    SimpleLinearRegressionLoaded | 
    ComputeMultipleLinearRegression |
    MultipleLinearRegressionLoaded |
    ComputePrincipalComponentAnalysis |
    PrincipalComponentLoaded |
    ComputeLogisticRegression |
    LogisticRegressionLoaded |
    ComputeGaussianNaiveBayes |
    GaussianNaiveBayesLoaded;