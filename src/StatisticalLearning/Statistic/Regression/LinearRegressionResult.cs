// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math.Entities;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalLearning.Statistic.Regression
{
    public class LinearRegressionResult
    {
        public LinearRegressionResult()
        {
            SlopeLst = new List<CoefficientResult>();
        }

        public double[] GetCoefficients()
        {
            var result = new double[1 + SlopeLst.Count];
            result[0] = Intercept.Value;
            for(int i = 0; i < SlopeLst.Count; i++)
            {
                result[i + 1] = SlopeLst.ElementAt(i).Value;
            }

            return result;
        }

        public CoefficientResult Intercept { get; set; }
        public ICollection<CoefficientResult> SlopeLst { get; set; }
        public double ResidualSumOfSquare { get; set; }
        public double MeanSquareError { get; set; }
        public double ResidualStandardError { get; set; }
        public double RSquare { get; set; }
    }
}
