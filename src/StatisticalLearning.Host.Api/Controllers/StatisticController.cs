// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.AspNetCore.Mvc;
using StatisticalLearning.Api.Host.DTOs.Requests;
using StatisticalLearning.Api.Host.DTOs.Responses;
using StatisticalLearning.Api.Host.Extensions;
using StatisticalLearning.Statistic.Analysis;
using StatisticalLearning.Statistic.Probability.Repartition;
using StatisticalLearning.Statistic.Regression;
using System.Linq;

namespace StatisticalLearning.Api.Host.Controllers
{
    [Route("statistic")]
    public class StatisticController : Controller
    {
        [HttpPost("regressions/linear")]
        public IActionResult GetLinarRegressionResult([FromBody] GetLinearRegressionRequest request)
        {
            var simpleLinearRegression = new MultipleLinearRegression();
            var result = simpleLinearRegression.Regress(request.Inputs, request.Outputs);
            return new OkObjectResult(result.ToDto());
        }

        [HttpPost("analysis/pca")]
        public IActionResult NaivePrincipalComponentAnalysis([FromBody] GetPrincipalComponentAnalysisRequest request)
        {
            var analysis = new PrincipalComponentAnalysis();
            var pcs = analysis.Compute(request.Matrix, true);
            var transformed = analysis.Transform(request.Matrix, true);
            var result = new PrincipalComponentAnalysisResponse
            {
                PrincipalComponents = pcs.Select(_ => _.ToDto()).ToArray(),
                Transformed = transformed.DoubleArr
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
