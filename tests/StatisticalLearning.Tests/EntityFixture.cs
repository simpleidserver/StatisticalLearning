// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Entities;
using StatisticalLearning.Numeric;
using Xunit;

namespace StatisticalLearning.Tests
{
    public class EntityFixture
    {
        [Fact]
        public void When_Derive_Equation()
        {
            VariableEntity x = "x";
            Entity result = Number.Create(1) * x + Number.Create(2) * Number.Create(5);
            var derived = result.Derive();
            Assert.NotNull(derived);
            Assert.Equal("0 * x + 1 * 1 + 0", derived.ToString());
        }

        [Fact]
        public void When_Solve_Monomial()
        {
            VariableEntity x = "x";
            Entity eq = Number.Create(2) * x + Number.Create(3) * x;
            var result = eq.Solve(x) as NumberEntity;
            Assert.NotNull(result);
            Assert.Equal(0, result.Number.Value);
        }
    }
}