// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math.Functions;
using Xunit;

namespace StatisticalLearning.Tests.Math.Functions
{
    public class BetaFunctionFixture
    {
        [Fact]
        public void When_Compute_Beta_Function()
        {
            var betaFunction = new BetaFunction();
            var result = betaFunction.Evaluate(1.5, 0.2);
            Assert.True(4.47 < result);
        }
    }
}
