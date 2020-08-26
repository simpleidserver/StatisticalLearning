var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
import * as fromActions from './statistic-actions';
export var initialSimpleLinearRegressionState = {
    content: null
};
export var initialMultipleLinearRegressionState = {
    content: null
};
export var initialPrincipalComponentAnalysisState = {
    content: null
};
export function SimpleLinearRegressionReducer(state, action) {
    if (state === void 0) { state = initialSimpleLinearRegressionState; }
    switch (action.type) {
        case fromActions.ActionTypes.SIMPLE_LINEAR_REGRESSION_RESULT_LOADED:
            state.content = action.simpleLinearRegressionResult;
            return __assign({}, state);
        default:
            return state;
    }
}
export function MultipleLinearRegressionReducer(state, action) {
    if (state === void 0) { state = initialMultipleLinearRegressionState; }
    switch (action.type) {
        case fromActions.ActionTypes.MULTIPLE_LINEAR_REGRESSION_RESULT_LOADED:
            state.content = action.multipleLinearRegressionResult;
            return __assign({}, state);
        default:
            return state;
    }
}
export function PrincipalComponentAnalysisReducer(state, action) {
    if (state === void 0) { state = initialPrincipalComponentAnalysisState; }
    switch (action.type) {
        case fromActions.ActionTypes.PRINCIPAL_COMPONENT_LOADED:
            state.content = action.principalComponentAnalysis;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=statistic-reducer.js.map