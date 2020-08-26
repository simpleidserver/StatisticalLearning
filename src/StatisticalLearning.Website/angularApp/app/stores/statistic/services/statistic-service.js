var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
var StatisticService = (function () {
    function StatisticService(http) {
        this.http = http;
    }
    StatisticService.prototype.computeSimpleLinearRegression = function (inputs, outputs) {
        var request = JSON.stringify({ inputs: inputs, outputs: outputs });
        var headers = new HttpHeaders();
        var targetUrl = process.env.API_URL + "/statistic/regressions/linear";
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        return this.http.post(targetUrl, request, { headers: headers });
    };
    StatisticService.prototype.computePrincipalComponentAnalysis = function (inputs) {
        var request = JSON.stringify({ matrix: inputs });
        var headers = new HttpHeaders();
        var targetUrl = process.env.API_URL + "/statistic/analysis/pca";
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        return this.http.post(targetUrl, request, { headers: headers });
    };
    StatisticService = __decorate([
        Injectable({
            providedIn: 'root'
        }),
        __metadata("design:paramtypes", [HttpClient])
    ], StatisticService);
    return StatisticService;
}());
export { StatisticService };
//# sourceMappingURL=statistic-service.js.map