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
import * as fromActions from './patient-actions';
export var initialListState = {
    content: null
};
export var initialListByNissState = {
    content: null
};
export var initialGetState = {
    content: null
};
export function ListPatientsReducer(state, action) {
    if (state === void 0) { state = initialListState; }
    switch (action.type) {
        case fromActions.ActionTypes.PATIENTS_LOADED:
            state.content = action.patients;
            return __assign({}, state);
        default:
            return state;
    }
}
export function ListPatientsByNissReducer(state, action) {
    if (state === void 0) { state = initialListByNissState; }
    switch (action.type) {
        case fromActions.ActionTypes.PATIENTS_LOADED_BY_NISS:
            state.content = action.patients;
            return __assign({}, state);
        default:
            return state;
    }
}
export function GetPatientReducer(state, action) {
    if (state === void 0) { state = initialGetState; }
    switch (action.type) {
        case fromActions.ActionTypes.PATIENT_LOADED:
            state.content = action.patient;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=patient-reducer.js.map