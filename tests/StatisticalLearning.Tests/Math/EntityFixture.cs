// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using System.Linq;
using Xunit;

namespace StatisticalLearning.Tests.Math
{
    public class EntityFixture
    {
        [Fact]
        public void When_Derive_Equation()
        {
            VariableEntity x = "x";
            Entity firstEquation = Number.Create(1) * x + Number.Create(2) * Number.Create(5);
            var firstResult = firstEquation.Derive().Eval();
            Assert.NotNull(firstResult);
            Assert.Equal("1", firstResult.ToString());
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
            var equation = (Number.Create(13) - x) *
                ((Number.Create(13) - x) * (Number.Create(8) - x) - Number.Create(4)) - Number.Create(12)  *
                (Number.Create(12) * (Number.Create(8) - x) + Number.Create(4)) +  Number.Create(2) * 
                (Number.Create(12) * Number.Create(-2) - (Number.Create(13) - x) * Number.Create(2));
            var result = equation.Evaluate(x);
            var solved = result.Solve(x);
            Assert.NotNull(solved);
        }

        [Fact]
        public void When_Solve_Equation_With_Variables()
        {
            VariableEntity x1 = "x1";
            VariableEntity x2 = "x2";
            VariableEntity x3 = "x3";
            var firstEquation = x1 - x2 + Number.Create(0) * x3;
            var secondEquation = x1 + x2;
            var firstResult = firstEquation.Solve(x1);
            var secondResult = secondEquation.Solve(x1);
            Assert.Equal("x2", firstResult.ElementAt(0).ToString());
            Assert.Equal("-1x2", secondResult.ElementAt(0).ToString());
        }

        [Fact]
        public void When_Solve_Addition()
        {
            VariableEntity x1 = "x1";
            var equation = Number.Create(-61) + x1 + Number.Create(60) + x1;
            var result = equation.Eval();
            Assert.Equal("2x1 + -1", result.ToString());
        }
    }
}