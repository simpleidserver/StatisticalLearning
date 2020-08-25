// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using Xunit;

namespace StatisticalLearning.Tests.Math.MatrixEchelons
{
    public class RowEchelonFormFixture
    {
        [Fact]
        public void When_Reduced_Row_Echelon_Form()
        {
            Matrix firstMatrix = new double[][]
            {
                new double[] { 0,2 },
                new double[] { 1,0 }
            };
            Matrix secondMatrix = new double[][]
            {
                new double[] { 8, 8 },
                new double[] { 8, 8 }
            };
            Matrix thirdMatrix = new double[][]
            {
                new double[] { -12, 12, 2 },
                new double[] { 12, -12, -2 },
                new double[] { 2, -2, -17 },
            };
            Matrix fourthMatrix = new double[][]
            {
                new double[] {13, 12, 2 },
                new double[] {12, 13, -2 },
                new double[] {2, -2, 8 }
            };
            Matrix fifthMatrix = new double[][]
            {
                new double[] { -11398.5092, 200 },
                new double[] { 200, -3.5092 }
            };
            var firstResult = firstMatrix.ReducedRowEchelonForm();
            var secondResult = secondMatrix.ReducedRowEchelonForm();
            var thirdResult = thirdMatrix.ReducedRowEchelonForm();
            var fourthResult = fourthMatrix.ReducedRowEchelonForm();
            var fifthResult = fifthMatrix.ReducedRowEchelonForm();

            Assert.Equal(1, firstResult.GetValue(0, 0).GetNumber());
            Assert.Equal(0, firstResult.GetValue(0, 1).GetNumber());
            Assert.Equal(1, secondResult.GetValue(0, 0).GetNumber());
            Assert.Equal(1, secondResult.GetValue(0, 1).GetNumber());
            Assert.Equal(1, thirdResult.GetValue(0, 0).GetNumber());
            Assert.Equal(-1, thirdResult.GetValue(0, 1).GetNumber());
            Assert.Equal(0, thirdResult.GetValue(0, 2).GetNumber());
            Assert.Equal(1, fourthResult.GetValue(0, 0).GetNumber());
            Assert.Equal(0, fourthResult.GetValue(0, 1).GetNumber());
            Assert.Equal(2, fourthResult.GetValue(0, 2).GetNumber());
            Assert.Equal(1, fifthResult.GetValue(0, 0).GetNumber());
        }
    }
}
