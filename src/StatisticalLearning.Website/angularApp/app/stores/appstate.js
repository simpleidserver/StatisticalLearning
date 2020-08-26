import { createSelector } from '@ngrx/store';
import * as fromStatisticReducers from './statistic/statistic-reducer';
export var selectSimpleLinearRegression = function (state) { return state.simpleLinearRegression; };
export var selectMultipleLinearRegression = function (state) { return state.multipleLinearRegression; };
export var selectPrincipalComponentAnalysis = function (state) { return state.principalComponentAnalysis; };
export var selectSimpleLinearRegressionResult = createSelector(selectSimpleLinearRegression, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectMultipleLinearRegressionResult = createSelector(selectMultipleLinearRegression, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectPrincipalComponentAnalysisResult = createSelector(selectPrincipalComponentAnalysis, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var appReducer = {
    simpleLinearRegression: fromStatisticReducers.SimpleLinearRegressionReducer,
    multipleLinearRegression: fromStatisticReducers.MultipleLinearRegressionReducer,
    principalComponentAnalysis: fromStatisticReducers.PrincipalComponentAnalysisReducer
};
//# sourceMappingURL=appstate.js.map