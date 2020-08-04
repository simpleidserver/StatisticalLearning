// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Regression;
using Xunit;

namespace StatisticalLearning.Tests
{
    public class LinearRegressionFixture
    {
        [Fact]
        public void When_Test_Linear_Regression()
        {
            double[] inputs = { 80, 60, 10, 20, 30 };
            double[] outputs = { 20, 40, 30, 50, 60 };
            var linearRegression = new SimpleLinearRegression();
            var result = linearRegression.Regress(inputs, outputs);
            Assert.Equal(-0.26470588235299969, result.Intercept);
            Assert.Equal(50.588235294119016, result.Slope);
        }
    }
}
