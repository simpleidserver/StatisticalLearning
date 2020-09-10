export class GaussianNaiveBayesResult {
    constructor() {
        this.classes = [];
        this.probabilities = [];
    }

    classes: number[];
    probabilities: number[][];
}