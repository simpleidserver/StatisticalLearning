﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.AspNetCore.Mvc;
using StatisticalLearning.Api.Host.DTOs.Requests;
using StatisticalLearning.Api.Host.DTOs.Responses;
using StatisticalLearning.Api.Host.Extensions;
using StatisticalLearning.Statistic.Analysis;
using StatisticalLearning.Statistic.Probability.Classifier;
using StatisticalLearning.Statistic.Probability.Repartition;
using StatisticalLearning.Statistic.Regression;
using System.Linq;

namespace StatisticalLearning.Api.Host.Controllers
{
    [Route("statistic")]
    public class StatisticController : Controller
    {
        // Retourner linear discriminant analysis + Afficher en angular.
        [HttpPost("regressions/linear")]
        public IActionResult GetLinarRegressionResult([FromBody] GetLinearRegressionRequest request)
        {
            var simpleLinearRegression = new MultipleLinearRegression(MatrixDecompositionAlgs.GOLUB_REINSCH);
            var result = simpleLinearRegression.Regress(request.Inputs, request.Outputs);
            return new OkObjectResult(result.LinearRegression.ToDto());
        }

        [HttpPost("regressions/logistic")]
        public IActionResult GetLogisticRegressionResult([FromBody] GetLogisticRegressionRequest request)
        {
            var logisticRegression = new LogisticRegression();
            logisticRegression.Regress(request.Inputs, request.Outputs);
            return new OkObjectResult(logisticRegression.ToDto());
        }

        [HttpPost("classifiers/gaussiannaivebayes")]
        public IActionResult GetGaussianNaiveBayesResult([FromBody] GetGaussianNaiveBayesRequest request)
        {
            var bayes = new GaussianNaiveBayes();
            bayes.Estimate(request.Inputs, request.Outputs);
            var result = bayes.PredictProbability(request.Predict);
            var prediction = bayes.Predict(request.Predict);
            return new OkObjectResult(new GaussianNaiveBayesResult
            {
                Classes = prediction.GetNumbers(),
                Probabilities = result.DoubleValues
            });
        }

        [HttpPost("analysis/pca")]
        public IActionResult NaivePrincipalComponentAnalysis([FromBody] GetPrincipalComponentAnalysisRequest request)
        {
            var analysis = new PrincipalComponentAnalysis(true, 2);
            var pcs = analysis.Compute(request.Matrix);
            var transformed = analysis.Transform(request.Matrix);
            var result = new PrincipalComponentAnalysisResponse
            {
                PrincipalComponents = pcs.PrincipalComponents.Select(_ => _.ToDto()).ToArray(),
                Transformed = transformed.DoubleValues
            };
            return new OkObjectResult(result);
        }

        [HttpPost("analysis/lda")]
        public IActionResult GetLinearDiscriminantAnalysisResult([FromBody] GetLinearDiscriminantAnalysisRequest request)
        {
            var linearDiscriminantAnalysis = new LinearDiscriminantAnalysis();
            linearDiscriminantAnalysis.Compute(request.Input, request.Output);
            var transformed = linearDiscriminantAnalysis.Transform(request.Input);
            var result = new LinearDiscriminantAnalysisResponse
            {
                Transformed = transformed.DoubleValues,
                LinearDiscriminantClasses = linearDiscriminantAnalysis.Result.Classes.Select(_ => _.ToDto()).ToArray()
            };
            return new OkObjectResult(result);
        }

        [HttpPost("probability/normalaw/lower")]
        public IActionResult LowerNormalLaw([FromBody] GetNormalLawRequest request)
        {
            var normalLaw = new NormalLaw();
            var result = normalLaw.ComputeLowerCumulative(request.Average, request.StandardDeviation, request.Value);
            return new OkObjectResult(new ProbabilityResponse { Probability = result });
        }

        [HttpPost("probability/normalaw/upper")]
        public IActionResult UpperNormalLaw([FromBody] GetNormalLawRequest request)
        {
            var normalLaw = new NormalLaw();
            var result = normalLaw.ComputeUpperCumulative(request.Average, request.StandardDeviation, request.Value);
            return new OkObjectResult(new ProbabilityResponse { Probability = result });
        }

        [HttpPost("probability/studentlaw/lower")]
        public IActionResult LowerStudentLaw([FromBody] GetStudentLawRequest request)
        {
            var studentLaw = new StudentLaw();
            var result = studentLaw.ComputeLowerCumulative(request.TStat, request.DegreeOfFreedom);
            return new OkObjectResult(new ProbabilityResponse { Probability = result });
        }


        [HttpPost("probability/studentlaw/upper")]
        public IActionResult UpperStudentLaw([FromBody] GetStudentLawRequest request)
        {
            var studentLaw = new StudentLaw();
            var result = studentLaw.ComputeUpperCumulative(request.TStat, request.DegreeOfFreedom);
            return new OkObjectResult(new ProbabilityResponse { Probability = result });
        }
    }
}
