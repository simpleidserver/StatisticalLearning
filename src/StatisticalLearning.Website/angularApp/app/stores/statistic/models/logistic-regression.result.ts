import { LinearRegressionResult } from "./linear-regression.result";

export class LogisticRegressionResult {
    oddsRatio: number[];
    standardErrors: number[];
    regression: LinearRegressionResult;
}