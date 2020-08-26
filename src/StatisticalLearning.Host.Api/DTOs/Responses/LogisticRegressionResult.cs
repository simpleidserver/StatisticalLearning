// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StatisticalLearning.Api.Host.DTOs.Responses
{
    public class LogisticRegressionResult
    {
        public double[] OddsRatio { get; set; }
        public double[] StandardErrors { get; set; }
        public double[] Transformed { get; set; }
        public LinearRegressionResponse Regression { get; set; }
    }
}
