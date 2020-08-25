// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Statistic.Regression;
using System.Linq;
using Xunit;

namespace StatisticalLearning.Tests.Statistic.Regression
{
    public class MultipleLinearRegressionFixture
    {
        [Fact]
        public void When_Compute_MultipleLinearRegression()
        {
            // https://datatofish.com/multiple-linear-regression-python/
            // another example : http://reliawiki.org/index.php/Multiple_Linear_Regression_Analysis#Coefficient_of_Multiple_Determination.2C_R2
            var inputs = new double[][] // Interest_Rate + Unemployment_Rate
            {
                new double[] { 2.75, 5.3 },
                new double[] { 2.5, 5.3 },
                new double[] { 2.5, 5.3 },
                new double[] { 2.5, 5.3 },
                new double[] { 2.5, 5.4 },
                new double[] { 2.5, 5.6 },
                new double[] { 2.5, 5.5 },
                new double[] { 2.25, 5.5 },
                new double[] { 2.25, 5.5 },
                new double[] { 2.25, 5.6 },
                new double[] { 2, 5.7 },
                new double[] { 2, 5.9 },
                new double[] { 2, 6 },
                new double[] { 1.75, 5.9 },
                new double[] { 1.75, 5.8 },
                new double[] { 1.75, 6.1 },
                new double[] { 1.75, 6.2 },
                new double[] { 1.75, 6.1 },
                new double[] { 1.75, 6.1 },
                new double[] { 1.75, 6.1 },
                new double[] { 1.75, 5.9 },
                new double[] { 1.75, 6.2 },
                new double[] { 1.75, 6.2 },
                new double[] { 1.75, 6.1 }
            };            
            var outputs = new double[] // // Stock index price.
            {
                1464,
                1394,
                1357,
                1293,
                1256,
                1254,
                1234,
                1195,
                1159,
                1167,
                1130,
                1075,
                1047,
                965,
                943,
                958,
                971,
                949,
                884,
                866,
                876,
                822,
                704,
                719
            };
            var multipleLinearRegression = new MultipleLinearRegression();
            var result = multipleLinearRegression.Regress(inputs, outputs);
            Assert.Equal(1798.4, System.Math.Round(result.LinearRegression.Intercept.Value, 1));
            Assert.Equal(345.5, System.Math.Round(result.LinearRegression.SlopeLst.ElementAt(0).Value, 1));
            Assert.Equal(-250.1, System.Math.Round(result.LinearRegression.SlopeLst.ElementAt(1).Value, 1));

            Assert.Equal(899.2, System.Math.Round(result.LinearRegression.Intercept.StandardError, 1));
            Assert.Equal(111.4, System.Math.Round(result.LinearRegression.SlopeLst.ElementAt(0).StandardError, 1));
            Assert.Equal(117.9, System.Math.Round(result.LinearRegression.SlopeLst.ElementAt(1).StandardError, 1));

            Assert.Equal(2, System.Math.Round(result.LinearRegression.Intercept.TStatistic, 1));
            Assert.Equal(3.103, System.Math.Round(result.LinearRegression.SlopeLst.ElementAt(0).TStatistic, 3));
            Assert.Equal(-2.121, System.Math.Round(result.LinearRegression.SlopeLst.ElementAt(1).TStatistic, 3));
        }
    }
}
