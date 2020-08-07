// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Statistic.Probability.Repartition;
using Xunit;

namespace StatisticalLearning.Tests.Statistic.Probability
{
    public class StudentLawFixture
    {
        [Fact]
        public void When_Compute_Upper_Cumulative()
        {
            var studentLaw = new StudentLaw();
            var result = studentLaw.ComputeUpperCumulative(2.29, 99);
            Assert.Equal(0.012133616433972372, result);
        }
    }
}
