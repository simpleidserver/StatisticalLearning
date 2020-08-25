// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.MatrixDecompositions;
using Xunit;

namespace StatisticalLearning.Tests.Math.MatrixDecompositions
{
    public class LUDecompositionFixture
    {
        [Fact]
        public void When_Decompose_Matrix_With_LU()
        {
            Matrix matrix = new double[][]
            {
                new double[] { 6, 8, 2 },
                new double[] { -6, 7, -4 },
                new double[] { 2, -4, 3 }
            };
            var decompose = LUDecomposition.Decompose(matrix);
            var result = (decompose.L * decompose.U).Evaluate();
            Assert.Equal(6, result.GetValue(0, 0).GetNumber());
            Assert.Equal(8, result.GetValue(0, 1).GetNumber());
            Assert.Equal(2, result.GetValue(0, 2).GetNumber());
            Assert.Equal(-6, result.GetValue(1, 0).GetNumber());
            Assert.Equal(7, result.GetValue(1, 1).GetNumber());
            Assert.Equal(-4, result.GetValue(1, 2).GetNumber());
            Assert.Equal(2, result.GetValue(2, 0).GetNumber());
            Assert.Equal(-4, System.Math.Round(result.GetValue(2, 1).GetNumber()));
            Assert.Equal(3, result.GetValue(2, 2).GetNumber());
        }
    }
}
