var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { StatisticService } from './services/statistic-service';
import { ActionTypes } from './statistic-actions';
var StatisticEffects = (function () {
    function StatisticEffects(actions$, statisticService) {
        var _this = this;
        this.actions$ = actions$;
        this.statisticService = statisticService;
        this.computeSimpleLinearRegression$ = this.actions$
            .pipe(ofType(ActionTypes.COMPUTE_SIMPLE_LINEAR_REGRESSION), mergeMap(function (evt) {
            var inputs = [];
            evt.inputs.forEach(function (i) {
                inputs.push([i]);
            });
            return _this.statisticService.computeSimpleLinearRegression(inputs, evt.outputs)
                .pipe(map(function (simpleLinearRegressionResult) { return { type: ActionTypes.SIMPLE_LINEAR_REGRESSION_RESULT_LOADED, simpleLinearRegressionResult: simpleLinearRegressionResult }; }), catchError(function () { return of({ type: ActionTypes.ERROR_LOAD_SIMPLE_LINEAR_REGRESSION }); }));
        }));
        this.computeMultipleLinearRegression$ = this.actions$
            .pipe(ofType(ActionTypes.COMPUTE_MULTIPLE_LINEAR_REGRESSION), mergeMap(function (evt) {
            return _this.statisticService.computeSimpleLinearRegression(evt.inputs, evt.outputs)
                .pipe(map(function (multipleLinearRegressionResult) { return { type: ActionTypes.MULTIPLE_LINEAR_REGRESSION_RESULT_LOADED, multipleLinearRegressionResult: multipleLinearRegressionResult }; }), catchError(function () { return of({ type: ActionTypes.ERROR_LOAD_MULTIPLE_LINEAR_REGRESSION }); }));
        }));
        this.computePrincipalComponentAnalysis$ = this.actions$
            .pipe(ofType(ActionTypes.COMPUTE_PRINCIPAL_COMPONENT_ANALYSIS), mergeMap(function (evt) {
            return _this.statisticService.computePrincipalComponentAnalysis(evt.inputs)
                .pipe(map(function (principalComponentAnalysis) { return { type: ActionTypes.PRINCIPAL_COMPONENT_LOADED, principalComponentAnalysis: principalComponentAnalysis }; }), catchError(function () { return of({ type: ActionTypes.ERROR_LOAD_PRINCIPAL_COMPONENT_ANALYSIS }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], StatisticEffects.prototype, "computeSimpleLinearRegression$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], StatisticEffects.prototype, "computeMultipleLinearRegression$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], StatisticEffects.prototype, "computePrincipalComponentAnalysis$", void 0);
    StatisticEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            StatisticService])
    ], StatisticEffects);
    return StatisticEffects;
}());
export { StatisticEffects };
//# sourceMappingURL=statistic-effects.js.map