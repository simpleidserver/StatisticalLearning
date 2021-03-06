﻿import { createSelector } from '@ngrx/store';
import * as fromStatisticReducers from './statistic/statistic-reducer';
import * as fromStatisticStates from './statistic/statistic-state';

export interface AppState {
    simpleLinearRegression: fromStatisticStates.SimpleLinearRegressionState,
    multipleLinearRegression: fromStatisticStates.MultipleLinearRegressionState,
    principalComponentAnalysis: fromStatisticStates.PrincipalComponentAnalysisState,
    logisticRegression: fromStatisticStates.LogisticRegressionState,
    gaussianNaiveBayes: fromStatisticStates.GaussianNaiveBayesState,
    linearDiscriminantAnalysis : fromStatisticStates.LinearDiscriminantAnalysisState
}

export const selectSimpleLinearRegression = (state: AppState) => state.simpleLinearRegression;
export const selectMultipleLinearRegression = (state: AppState) => state.multipleLinearRegression;
export const selectPrincipalComponentAnalysis = (state: AppState) => state.principalComponentAnalysis;
export const selectLogisticRegression = (state: AppState) => state.logisticRegression;
export const selectGaussianNaiveBayes = (state: AppState) => state.gaussianNaiveBayes;
export const selectLinearDiscriminantAnalysis = (state: AppState) => state.linearDiscriminantAnalysis;

export const selectSimpleLinearRegressionResult = createSelector(
    selectSimpleLinearRegression,
    (state: fromStatisticStates.SimpleLinearRegressionState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectMultipleLinearRegressionResult = createSelector(
    selectMultipleLinearRegression,
    (state: fromStatisticStates.MultipleLinearRegressionState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectPrincipalComponentAnalysisResult = createSelector(
    selectPrincipalComponentAnalysis,
    (state: fromStatisticStates.PrincipalComponentAnalysisState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectLogisticRegressionResult = createSelector(
    selectLogisticRegression,
    (state: fromStatisticStates.LogisticRegressionState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectGaussianNaiveBayesResult = createSelector(
    selectGaussianNaiveBayes,
    (state: fromStatisticStates.GaussianNaiveBayesState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);


export const selectLinearDiscriminantAnalysisResult = createSelector(
    selectLinearDiscriminantAnalysis,
    (state: fromStatisticStates.LinearDiscriminantAnalysisState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);


export const appReducer = {
    simpleLinearRegression: fromStatisticReducers.SimpleLinearRegressionReducer,
    multipleLinearRegression: fromStatisticReducers.MultipleLinearRegressionReducer,
    principalComponentAnalysis: fromStatisticReducers.PrincipalComponentAnalysisReducer,
    logisticRegression: fromStatisticReducers.LogisticRegressionReducer,
    gaussianNaiveBayes: fromStatisticReducers.GaussianNaiveBayesReducer,
    linearDiscriminantAnalysis: fromStatisticReducers.LinearDiscriminantAnalysisReducer
};