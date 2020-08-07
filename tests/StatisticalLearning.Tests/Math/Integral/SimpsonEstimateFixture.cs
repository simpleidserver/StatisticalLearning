// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math.Entities;
using StatisticalLearning.Math.Integral;
using Xunit;

namespace StatisticalLearning.Tests.Math.Integral
{
    public class SimpsonEstimateFixture
    {
        [Fact]
        public void When_Solve_Integral_With_SimpsonEstimate()
        {
            VariableEntity x = "x";
            var function = MathEntity.Sqrt(x);
            var estimate = new SimpsonEstimate();
            var estimation = estimate.Estimate(function, x, 4, 0, 8);
            Assert.Equal(14.855493563580854, estimation);
        }
    }
}
