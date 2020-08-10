// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using System.Collections.Generic;

namespace StatisticalLearning.Statistic.Regression
{
    public class LinearRegressionResult
    {
        public LinearRegressionResult()
        {
            SlopeLst = new List<CoefficientResult>();
        }

        public CoefficientResult Intercept { get; set; }
        public ICollection<CoefficientResult> SlopeLst { get; set; }
        public double ResidualSumOfSquare { get; set; }
        public double MeanSquareError { get; set; }
        public double ResidualStandardError { get; set; }
        public double RSquare { get; set; }
    }
}
