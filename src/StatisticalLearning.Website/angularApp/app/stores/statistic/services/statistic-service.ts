import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { LinearRegressionResult } from "../models/linear-regression.result";
import { PrincipalComponentAnalysisResult } from "../models/pca.result";
import { LogisticRegressionResult } from "../models/logistic-regression.result";
import { GaussianNaiveBayesResult } from '../models/gaussian-naivebayes.result';

@Injectable({
    providedIn: 'root'
})
export class StatisticService {
    constructor(private http: HttpClient) { }

    computeSimpleLinearRegression(inputs: number[][], outputs: number[]): Observable<LinearRegressionResult> {
        const request = JSON.stringify({ inputs: inputs, outputs: outputs});
        let headers = new HttpHeaders();
        const targetUrl = process.env.API_URL + "/statistic/regressions/linear";
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        return this.http.post<LinearRegressionResult>(targetUrl, request, { headers: headers });
    }

    computePrincipalComponentAnalysis(inputs: number[][]): Observable<PrincipalComponentAnalysisResult> {
        const request = JSON.stringify({ matrix: inputs });
        let headers = new HttpHeaders();
        const targetUrl = process.env.API_URL + "/statistic/analysis/pca";
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        return this.http.post<PrincipalComponentAnalysisResult>(targetUrl, request, { headers: headers });
    }

    computeLogisticRegression(inputs: number[][], outputs : number[]): Observable<LogisticRegressionResult> {
        const request = JSON.stringify({ inputs: inputs, outputs: outputs });
        let headers = new HttpHeaders();
        const targetUrl = process.env.API_URL + "/statistic/regressions/logistic";
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        return this.http.post<LogisticRegressionResult>(targetUrl, request, { headers: headers });
    }

    computeGaussianNaiveBayes(inputs: number[][], predict: number[][], outputs: number[]) : Observable<GaussianNaiveBayesResult> {
        const request = JSON.stringify({ inputs: inputs, outputs: outputs, predict : predict });
        let headers = new HttpHeaders();
        const targetUrl = process.env.API_URL + "/statistic/classifiers/gaussiannaivebayes";
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        return this.http.post<GaussianNaiveBayesResult>(targetUrl, request, { headers: headers });
    }
}