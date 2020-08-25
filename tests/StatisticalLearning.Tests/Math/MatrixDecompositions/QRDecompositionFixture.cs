// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.MatrixDecompositions;
using Xunit;

namespace StatisticalLearning.Tests.Math.MatrixDecompositions
{
    public class QRDecompositionFixture
    {
        [Fact]
        public void When_Decompose_With_QR()
        {
            Matrix matrix = new double[][]
            {
                new double[] { 1, -1, 4 },
                new double[] { 1, 4, -2 },
                new double[] { 1, 4, 2 },
                new double[] { 1, -1, 0 }
            };
            var result = QRDecomposition.Decompose(matrix);
            Assert.Equal(2, result.R.GetValue(0, 0).GetNumber());
            Assert.Equal(3, result.R.GetValue(0, 1).GetNumber());
            Assert.Equal(2, result.R.GetValue(0, 2).GetNumber());
        }
    }
}
