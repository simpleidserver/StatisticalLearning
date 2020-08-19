export class PrincipalComponent {
    cumulative: number;
    eigenValue: number;
    eigenvector: number[];
    proportion: number;
    singularValue: number;
}

export class PrincipalComponentAnalysisResult {
    transformed: number[][];
    principalComponents: PrincipalComponent[];
}