export class LinearDiscriminantClass {
    eigenValue: number;
    eigenvector: number[];
}

export class LinearDiscriminantAnalysisResult {
    transformed: number[][];
    principalComponents: LinearDiscriminantClass[];
}