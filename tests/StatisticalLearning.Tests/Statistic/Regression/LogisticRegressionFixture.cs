// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Statistic.Regression;
using Xunit;

namespace StatisticalLearning.Tests.Statistic.Regression
{
    public class LogisticRegressionFixture
    {
        [Fact]
        public void When_Compute_Logistic_Regression()
        {
            double[][] inputs =
            {
                new double[] { 55, 0 },
                new double[] { 28, 0 },
                new double[] { 65, 1 },
                new double[] { 46, 0 },
                new double[] { 86, 1 },
                new double[] { 56, 1 },
                new double[] { 85, 0 },
                new double[] { 33, 0 },
                new double[] { 21, 1 },
                new double[] { 42, 1 }
            };
            double[] outputs =
            {   
                0, 0, 0, 1, 1, 1, 0, 0, 0, 1
            };
            var logisticRegression = new LogisticRegression();
            logisticRegression.Regress(inputs, outputs);
            var result = logisticRegression.Compute(new double[] { 87, 1 });
            var oddsRatio = logisticRegression.GetOddsRatio();
            var standardErrors = logisticRegression.GetStandardErrors();
            Assert.Equal(0.75143272858390264, result);

            Assert.Equal(0.085627701183141239, oddsRatio[0]);
            Assert.Equal(1.0208597029292656, oddsRatio[1]);
            Assert.Equal(5.8584748981778869, oddsRatio[2]);

            Assert.Equal(2.1590686019476122, standardErrors[0]);
            Assert.Equal(0.0337904223210436, standardErrors[1]);
            Assert.Equal(1.4729903935788495, standardErrors[2]);
        }
    }
}