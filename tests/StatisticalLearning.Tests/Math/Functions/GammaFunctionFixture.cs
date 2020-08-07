// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math.Functions;
using Xunit;

namespace StatisticalLearning.Tests.Math.Functions
{
    public class GammaFunctionFixture
    {
        [Fact]
        public void When_Solve_Gamma_Function_With_Lanczos_Approximation()
        {
            var gammaFunction = new GammaFunction();
            var firstResult = gammaFunction.EvaluateWithLanczos(2.5);
            var secondResult = gammaFunction.EvaluateWithLanczos(-0.5);
            Assert.Equal(1.3293403881791388, firstResult);
            Assert.Equal(-3.5449077018110287, secondResult);
        }
    }
}
