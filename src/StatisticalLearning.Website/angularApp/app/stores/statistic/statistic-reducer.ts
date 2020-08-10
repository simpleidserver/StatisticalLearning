import * as fromActions from './statistic-actions';
import { MultipleLinearRegressionState, SimpleLinearRegressionState } from './statistic-state';

export const initialSimpleLinearRegressionState: SimpleLinearRegressionState = {
    content : null
};

export const initialMultipleLinearRegressionState: MultipleLinearRegressionState = {
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