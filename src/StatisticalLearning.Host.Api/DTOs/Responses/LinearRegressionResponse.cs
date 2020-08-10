// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using System.Collections.Generic;

namespace StatisticalLearning.Api.Host.DTOs.Responses
{
    public class LinearRegressionResponse
    {
        public LinearRegressionResponse()
        {
            Slope = new List<CoefficientResponse>();
        }

        public CoefficientResponse Intercept { get; set; }
        public List<CoefficientResponse> Slope { get; set; }
        public double ResidualSumOfSquares { get; set; }
        public double ResidualStandardError { get; set; }
        public double RSquare { get; set; }
    }
}
