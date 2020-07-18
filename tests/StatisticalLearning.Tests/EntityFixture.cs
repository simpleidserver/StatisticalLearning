// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Entities;
using StatisticalLearning.Numeric;
using System.Linq;
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
            var result = eq.Solve(x).ElementAt(0) as NumberEntity;
            Assert.NotNull(result);
            Assert.Equal(0, result.Number.Value);
        }

        [Fact]
        public void When_Solve_Second_Degree_Equation()
        {
            VariableEntity x = "x";
            Entity eq =  x * x  - Number.Create(34) * x + Number.Create(225);
            eq = eq.Evaluate(x);
            var result = eq.Solve(x);
            var firstNumber = result.ElementAt(0) as NumberEntity;
            var secondNumber = result.ElementAt(1) as NumberEntity;
            Assert.NotNull(firstNumber);
            Assert.NotNull(secondNumber);
            Assert.Equal(9, firstNumber.Number.Value);
            Assert.Equal(25, secondNumber.Number.Value);
        }

        [Fact]
        public void When_Solve_Complex_Equation()
        {
            VariableEntity x = "x";
            var res = (Number.Create(17) - x) * (Number.Create(17) - x);
            res.Evaluate(x);
        }
    }
}