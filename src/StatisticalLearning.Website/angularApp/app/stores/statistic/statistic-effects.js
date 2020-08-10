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
import { ActionTypes } from './patient-actions';
import { PatientService } from './services/patient-service';
var PatientEffects = (function () {
    function PatientEffects(actions$, patientService) {
        var _this = this;
        this.actions$ = actions$;
        this.patientService = patientService;
        this.searchPatients$ = this.actions$
            .pipe(ofType(ActionTypes.SEARCH_PATIENTS), mergeMap(function (evt) {
            return _this.patientService.search(evt.firstname, evt.lastname, evt.niss, evt.startIndex, evt.count, evt.active, evt.direction)
                .pipe(map(function (patients) { return { type: ActionTypes.PATIENTS_LOADED, patients: patients }; }), catchError(function () { return of({ type: ActionTypes.ERROR_SEARCH_PATIENTS }); }));
        }));
        this.searchPatientsByNiss$ = this.actions$
            .pipe(ofType(ActionTypes.SEARCH_PATIENTS_BY_NISS), mergeMap(function (evt) {
            return _this.patientService.search(null, null, evt.niss, 0, 0)
                .pipe(map(function (patients) { return { type: ActionTypes.PATIENTS_LOADED_BY_NISS, patients: patients }; }), catchError(function () { return of({ type: ActionTypes.ERROR_SEARCH_PATIENTS_BY_NISS }); }));
        }));
        this.getPatientById$ = this.actions$
            .pipe(ofType(ActionTypes.GET_PATIENT_BY_ID), mergeMap(function (evt) {
            return _this.patientService.getById(evt.id)
                .pipe(map(function (patient) { return { type: ActionTypes.PATIENT_LOADED, patient: patient }; }), catchError(function () { return of({ type: ActionTypes.ERROR_GET_PATIENT }); }));
        }));
        this.getPatientByNiss$ = this.actions$
            .pipe(ofType(ActionTypes.GET_PATIENT_BY_NISS), mergeMap(function (evt) {
            return _this.patientService.getByNiss(evt.niss)
                .pipe(map(function (patient) { return { type: ActionTypes.PATIENT_LOADED, patient: patient }; }), catchError(function () { return of({ type: ActionTypes.ERROR_GET_PATIENT }); }));
        }));
        this.addPatient$ = this.actions$
            .pipe(ofType(ActionTypes.ADD_PATIENT), mergeMap(function (evt) {
            return _this.patientService.add(evt.patient)
                .pipe(map(function () { return { type: ActionTypes.ADD_PATIENT_SUCCESS }; }), catchError(function () { return of({ type: ActionTypes.ADD_PATIENT_ERROR }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], PatientEffects.prototype, "searchPatients$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], PatientEffects.prototype, "searchPatientsByNiss$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], PatientEffects.prototype, "getPatientById$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], PatientEffects.prototype, "getPatientByNiss$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], PatientEffects.prototype, "addPatient$", void 0);
    PatientEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            PatientService])
    ], PatientEffects);
    return PatientEffects;
}());
export { PatientEffects };
//# sourceMappingURL=patient-effects.js.map