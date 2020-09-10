import { LinearRegressionResult } from './models/linear-regression.result';
import { PrincipalComponentAnalysisResult } from './models/pca.result';
import { LogisticRegressionResult } from './models/logistic-regression.result';
import { GaussianNaiveBayesResult } from './models/gaussian-naivebayes.result';
import { LinearDiscriminantAnalysisResult } from './models/lda.result';

export interface SimpleLinearRegressionState {
    content: LinearRegressionResult
}

export interface MultipleLinearRegressionState {
    content: LinearRegressionResult
}

export interface PrincipalComponentAnalysisState {
    content: PrincipalComponentAnalysisResult;
}

export interface LogisticRegressionState {
    content: LogisticRegressionResult;
}

export interface GaussianNaiveBayesState {
    content: GaussianNaiveBayesResult;
}

export interface LinearDiscriminantAnalysisState {
    content: LinearDiscriminantAnalysisResult;
}