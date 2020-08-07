// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
namespace StatisticalLearning.Statistic.Regression
{
    public class SimpleLinearRegressionResult
    {
        public double Slope { get; set; }
        public double StandardErrorSlope { get; set; }
        public double TStatisticSlope { get; set; }
        public double PValueSlope { get; set; }
        public double Intercept { get; set; }
        public double StandardErrorIntercept { get; set; }
        public double TStatisticIntercept { get; set; }
        public double PValueIntercept { get; set; }
        public double ResidualSumOfSquares { get; set; }
        public double ResidualStandardError { get; set; }

        public double Compute(double x)
        {
            return Intercept + Slope * x;
        }
    }
}
