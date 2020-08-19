import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { LinearRegressionResult } from "../models/linear-regression.result";
import { PrincipalComponentAnalysisResult } from "../models/pca.result";

@Injectable({
    providedIn: 'root'
})
export class StatisticService {
    constructor(private http: HttpClient) { }

    computeSimpleLinearRegression(inputs : Number[][], outputs : Number[]): Observable<LinearRegressionResult> {
        const request = JSON.stringify({ inputs: inputs, outputs: outputs});
        let headers = new HttpHeaders();
        let targetUrl = process.env.API_URL + "/statistic/regressions/linear";
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        return this.http.post<LinearRegressionResult>(targetUrl, request, { headers: headers });
    }

    computePrincipalComponentAnalysis(inputs: number[][]): Observable<PrincipalComponentAnalysisResult> {
        const request = JSON.stringify({ matrix: inputs });
        let headers = new HttpHeaders();
        let targetUrl = process.env.API_URL + "/statistic/analysis/pca";
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        return this.http.post<PrincipalComponentAnalysisResult>(targetUrl, request, { headers: headers });
    }
}