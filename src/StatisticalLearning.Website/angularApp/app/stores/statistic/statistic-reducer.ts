import * as fromActions from './statistic-actions';
import { SimpleLinearRegressionState } from './statistic-state';

export const initialSimpleLinearRegressionState: SimpleLinearRegressionState = {
    content : null
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