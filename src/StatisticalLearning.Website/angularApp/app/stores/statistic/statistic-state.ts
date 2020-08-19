import { LinearRegressionResult } from './models/linear-regression.result';
import { PrincipalComponentAnalysisResult } from './models/pca.result';

export interface SimpleLinearRegressionState {
    content: LinearRegressionResult
}

export interface MultipleLinearRegressionState {
    content: LinearRegressionResult
}

export interface PrincipalComponentAnalysisState {
    content: PrincipalComponentAnalysisResult;
}