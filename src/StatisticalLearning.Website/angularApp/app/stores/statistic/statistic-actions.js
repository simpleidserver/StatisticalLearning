export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["COMPUTE_SIMPLE_LINEAR_REGRESSION"] = "[Statistic] COMPUTE_SIMPLE_LINEAR_REGRESSION";
    ActionTypes["SIMPLE_LINEAR_REGRESSION_RESULT_LOADED"] = "[Statistic] LINEAR_REGRESSION_RESULT_LOADED";
    ActionTypes["ERROR_LOAD_SIMPLE_LINEAR_REGRESSION"] = "[Statistic] ERROR_LOAD_SIMPLE_LINEAR_REGRESSION";
    ActionTypes["COMPUTE_MULTIPLE_LINEAR_REGRESSION"] = "[Statistic] COMPUTE_MULTIPLE_LINEAR_REGRESSION";
    ActionTypes["MULTIPLE_LINEAR_REGRESSION_RESULT_LOADED"] = "[Statistic] MULTIPLE_LINEAR_REGRESSION_RESULT_LOADED";
    ActionTypes["ERROR_LOAD_MULTIPLE_LINEAR_REGRESSION"] = "[Statistic] ERROR_LOAD_MULTIPLE_LINEAR_REGRESSION";
    ActionTypes["COMPUTE_PRINCIPAL_COMPONENT_ANALYSIS"] = "[Statistic] COMPUTE_PRINCIPAL_COMPONENT_ANALYSIS";
    ActionTypes["PRINCIPAL_COMPONENT_LOADED"] = "[Statistic] PRINCIPAL_COMPONENT_LOADED";
    ActionTypes["ERROR_LOAD_PRINCIPAL_COMPONENT_ANALYSIS"] = "[Statistic] ERROR_LOAD_PRINCIPAL_COMPONENT_ANALYSIS";
})(ActionTypes || (ActionTypes = {}));
var ComputeSimpleLinearRegression = (function () {
    function ComputeSimpleLinearRegression(inputs, outputs) {
        this.inputs = inputs;
        this.outputs = outputs;
        this.type = ActionTypes.COMPUTE_SIMPLE_LINEAR_REGRESSION;
    }
    return ComputeSimpleLinearRegression;
}());
export { ComputeSimpleLinearRegression };
var SimpleLinearRegressionLoaded = (function () {
    function SimpleLinearRegressionLoaded(simpleLinearRegressionResult) {
        this.simpleLinearRegressionResult = simpleLinearRegressionResult;
        this.type = ActionTypes.SIMPLE_LINEAR_REGRESSION_RESULT_LOADED;
    }
    return SimpleLinearRegressionLoaded;
}());
export { SimpleLinearRegressionLoaded };
var ComputeMultipleLinearRegression = (function () {
    function ComputeMultipleLinearRegression(inputs, outputs) {
        this.inputs = inputs;
        this.outputs = outputs;
        this.type = ActionTypes.COMPUTE_MULTIPLE_LINEAR_REGRESSION;
    }
    return ComputeMultipleLinearRegression;
}());
export { ComputeMultipleLinearRegression };
var MultipleLinearRegressionLoaded = (function () {
    function MultipleLinearRegressionLoaded(multipleLinearRegressionResult) {
        this.multipleLinearRegressionResult = multipleLinearRegressionResult;
        this.type = ActionTypes.MULTIPLE_LINEAR_REGRESSION_RESULT_LOADED;
    }
    return MultipleLinearRegressionLoaded;
}());
export { MultipleLinearRegressionLoaded };
var ComputePrincipalComponentAnalysis = (function () {
    function ComputePrincipalComponentAnalysis(inputs) {
        this.inputs = inputs;
        this.type = ActionTypes.COMPUTE_PRINCIPAL_COMPONENT_ANALYSIS;
    }
    return ComputePrincipalComponentAnalysis;
}());
export { ComputePrincipalComponentAnalysis };
var PrincipalComponentLoaded = (function () {
    function PrincipalComponentLoaded(principalComponentAnalysis) {
        this.principalComponentAnalysis = principalComponentAnalysis;
        this.type = ActionTypes.PRINCIPAL_COMPONENT_LOADED;
    }
    return PrincipalComponentLoaded;
}());
export { PrincipalComponentLoaded };
//# sourceMappingURL=statistic-actions.js.map