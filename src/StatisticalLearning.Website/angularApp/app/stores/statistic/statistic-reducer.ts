﻿import * as fromActions from './statistic-actions';
import { LinearDiscriminantAnalysisState, LogisticRegressionState, GaussianNaiveBayesState, MultipleLinearRegressionState, PrincipalComponentAnalysisState, SimpleLinearRegressionState } from './statistic-state';

export const initialSimpleLinearRegressionState: SimpleLinearRegressionState = {
    content : null
};

export const initialMultipleLinearRegressionState: MultipleLinearRegressionState = {
    content: null
};

export const initialPrincipalComponentAnalysisState: PrincipalComponentAnalysisState = {
    content: null
};

export const initialLogisticRegressionState: LogisticRegressionState = {
    content: null
};

export const initialGaussianNaiveBayesState: GaussianNaiveBayesState = {
    content: null
};

export const initialLinearDiscriminantAnalysisState: LinearDiscriminantAnalysisState = {
    content: null
};

export function SimpleLinearRegressionReducer(state = initialSimpleLinearRegressionState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.SIMPLE_LINEAR_REGRESSION_RESULT_LOADED:
            state.content = action.simpleLinearRegressionResult;
            return { ...state };
        default:
            return state;
    }
}

export function MultipleLinearRegressionReducer(state = initialMultipleLinearRegressionState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.MULTIPLE_LINEAR_REGRESSION_RESULT_LOADED:
            state.content = action.multipleLinearRegressionResult;
            return { ...state };
        default:
            return state;
    }
}

export function PrincipalComponentAnalysisReducer(state = initialPrincipalComponentAnalysisState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.PRINCIPAL_COMPONENT_LOADED:
            state.content = action.principalComponentAnalysis;
            return { ...state };
        default:
            return state;
    }
}

export function LogisticRegressionReducer(state = initialLogisticRegressionState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.LOGISTIC_REGRESSION_LOADED:
            state.content = action.logisticRegression;
            return { ...state };
        default:
            return state;
    }
}

export function GaussianNaiveBayesReducer(state = initialGaussianNaiveBayesState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.GAUSSIAN_NAIVEBAYES_LOADED:
            state.content = action.gaussianNaiveBayes;
            return { ...state };
        default:
            return state;
    }
}

export function LinearDiscriminantAnalysisReducer(state = initialLinearDiscriminantAnalysisState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.LINEAR_DISCRIMINANT_ANALYSIS_LOADED:
            state.content = action.lda;
            return { ...state };
        default:
            return state;
    }
}