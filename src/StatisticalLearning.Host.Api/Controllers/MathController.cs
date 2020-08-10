// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.AspNetCore.Mvc;
using StatisticalLearning.Api.Host.DTOs.Requests;
using StatisticalLearning.Api.Host.DTOs.Responses;
using StatisticalLearning.Api.Host.Extensions;
using StatisticalLearning.Math.Functions;
using StatisticalLearning.Math.MatrixDecompositions;

namespace StatisticalLearning.Api.Host.Controllers
{
    [Route("math")]
    public class MathController : Controller
    {
        [HttpPost("matrix/decompose/svd")]
        public IActionResult DecomposeSVD([FromBody] double[][] matrix)
        {
            var decomposition = new SingularValueDecomposition();
            var result = decomposition.Decompose(new Math.Matrix(matrix));
            return new OkObjectResult(result.ToDto());
        }

        [HttpPost("functions/beta")]
        public IActionResult ComputeBetaFunction([FromBody] GetBetaFunctionRequest request)
        {
            var beta = new BetaFunction();
            var result = beta.Evaluate(request.A, request.B);
            return new OkObjectResult(new FunctionResponse { Value = result });
        }

        [HttpPost("functions/gamma")]
        public IActionResult ComputeGammaFunction([FromBody] GetGammaRequest request)
        {
            var gamma = new GammaFunction();
            var result = gamma.EvaluateWithLanczos(request.A);
            return new OkObjectResult(new FunctionResponse { Value = result });
        }
    }
}
