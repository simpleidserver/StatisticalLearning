import { CoefficientResult } from "./coefficient.result";

export class LinearRegressionResult {
    slope: CoefficientResult[];
    intercept: CoefficientResult;
    residualStandardError: number;
    residualSumOfSquares: number;
    rSquare: number;
}