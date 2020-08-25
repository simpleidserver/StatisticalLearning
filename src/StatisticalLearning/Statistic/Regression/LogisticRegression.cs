// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using System.Linq;

namespace StatisticalLearning.Statistic.Regression
{
    public class LogisticRegression : PartialLeastSquareRegression
    {
        public double[] GetOddsRatio()
        {
            var result = new double[1 + _multipleLinearRegression.LinearRegression.SlopeLst.Count];
            result[0] = System.Math.Exp(_multipleLinearRegression.LinearRegression.Intercept.Value);
            for (int i = 0; i < _multipleLinearRegression.LinearRegression.SlopeLst.Count; i++)
            {
                result[i + 1] = System.Math.Exp(_multipleLinearRegression.LinearRegression.SlopeLst.ElementAt(i).Value);
            }

            return result;
        }

        public double[] GetStandardErrors()
        {
            var result = new double[1 + _multipleLinearRegression.LinearRegression.SlopeLst.Count];
            result[0] = _multipleLinearRegression.LinearRegression.Intercept.StandardError;
            for (int i = 0; i < _multipleLinearRegression.LinearRegression.SlopeLst.Count; i++)
            {
                result[i + 1] = _multipleLinearRegression.LinearRegression.SlopeLst.ElementAt(i).StandardError;
            }

            return result;
        }

        protected override double Transform(double value)
        {
            return System.Math.Log(value / 1 - value);
        }

        protected override double Inverse(double value)
        {
            return 1.0 / (1.0 + System.Math.Exp(-value));
        }
    }
}
