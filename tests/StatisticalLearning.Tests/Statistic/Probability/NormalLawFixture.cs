// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Statistic.Probability.Repartition;
using Xunit;

namespace StatisticalLearning.Tests.Statistic.Probability
{
    public class NormalLawFixture
    {
        [Fact]
        public void When_Compute_Probabiliy_NormalLaw()
        {
            var normalLaw = new NormalLaw();
            var firstProbability = normalLaw.ComputeProbability(0, 1, 1.56); // <= 1.56
            var secondProbability = normalLaw.ComputeProbability(0, 1, 0.5); // <= 0.5
            Assert.Equal(0.94064850654741039, firstProbability);
            Assert.Equal(0.691662613143159, secondProbability);
        }
    }
}
