// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
namespace StatisticalLearning.Statistic.Regression
{
    public class CoefficientResult
    {
        public double Value { get; set; }
        public double StandardError { get; set; }
        public double TStatistic { get; set; }
        public double PValue { get; set; }
    }
}
