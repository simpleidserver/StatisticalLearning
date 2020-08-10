export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["SEARCH_PATIENTS_BY_NISS"] = "[Patient] SEARCH_PATIENTS_BY_NISS";
    ActionTypes["PATIENTS_LOADED_BY_NISS"] = "[Patient] PATIENTS_LOADED_BY_NISS";
    ActionTypes["ERROR_SEARCH_PATIENTS_BY_NISS"] = "[Patient] ERROR_SEARCH_PATIENTS_BY_NISS";
    ActionTypes["SEARCH_PATIENTS"] = "[Patient] SEARCH_PATIENTS";
    ActionTypes["PATIENTS_LOADED"] = "[Patient] PATIENTS_LOADED";
    ActionTypes["ERROR_SEARCH_PATIENTS"] = "[Patient] ERROR_SEARCH_PATIENTS";
    ActionTypes["GET_PATIENT_BY_ID"] = "[Patient] GET_PATIENT_BY_ID";
    ActionTypes["GET_PATIENT_BY_NISS"] = "[Patient] GET_PATIENT_BY_NISS";
    ActionTypes["PATIENT_LOADED"] = "[Patient] PATIENT_LOADED";
    ActionTypes["ERROR_GET_PATIENT"] = "[Patient] ERROR_GET_PATIENT";
    ActionTypes["ADD_PATIENT"] = "[Patient] ADD_PATIENT";
    ActionTypes["ADD_PATIENT_SUCCESS"] = "[Patient] ADD_PATIENT_SUCCESS";
    ActionTypes["ADD_PATIENT_ERROR"] = "[Patient] ADD_PATIENT_ERROR";
})(ActionTypes || (ActionTypes = {}));
var SearchPatientsByNiss = (function () {
    function SearchPatientsByNiss(niss) {
        this.niss = niss;
        this.type = ActionTypes.SEARCH_PATIENTS_BY_NISS;
    }
    return SearchPatientsByNiss;
}());
export { SearchPatientsByNiss };
var SearchPatients = (function () {
    function SearchPatients(niss, firstname, lastname, startIndex, count, active, direction) {
        if (active === void 0) { active = null; }
        if (direction === void 0) { direction = null; }
        this.niss = niss;
        this.firstname = firstname;
        this.lastname = lastname;
        this.startIndex = startIndex;
        this.count = count;
        this.active = active;
        this.direction = direction;
        this.type = ActionTypes.SEARCH_PATIENTS;
    }
    return SearchPatients;
}());
export { SearchPatients };
var GetPatientById = (function () {
    function GetPatientById(id) {
        this.id = id;
        this.type = ActionTypes.GET_PATIENT_BY_ID;
    }
    return GetPatientById;
}());
export { GetPatientById };
var GetPatientByNiss = (function () {
    function GetPatientByNiss(niss) {
        this.niss = niss;
        this.type = ActionTypes.GET_PATIENT_BY_NISS;
    }
    return GetPatientByNiss;
}());
export { GetPatientByNiss };
var AddPatient = (function () {
    function AddPatient(patient) {
        this.patient = patient;
        this.type = ActionTypes.ADD_PATIENT;
    }
    return AddPatient;
}());
export { AddPatient };
var PatientsLoaded = (function () {
    function PatientsLoaded(patients) {
        this.patients = patients;
        this.type = ActionTypes.PATIENTS_LOADED;
    }
    return PatientsLoaded;
}());
export { PatientsLoaded };
var PatientLoaded = (function () {
    function PatientLoaded(patient) {
        this.patient = patient;
        this.type = ActionTypes.PATIENT_LOADED;
    }
    return PatientLoaded;
}());
export { PatientLoaded };
var PatientsByNissLoaded = (function () {
    function PatientsByNissLoaded(patients) {
        this.patients = patients;
        this.type = ActionTypes.PATIENTS_LOADED_BY_NISS;
    }
    return PatientsByNissLoaded;
}());
export { PatientsByNissLoaded };
//# sourceMappingURL=patient-actions.js.map