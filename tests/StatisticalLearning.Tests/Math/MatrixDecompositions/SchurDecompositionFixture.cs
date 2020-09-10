// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math.MatrixDecompositions;
using Xunit;

namespace StatisticalLearning.Tests.Math.MatrixDecompositions
{
    public class SchurDecompositionFixture
    {
        [Fact]
        public void When_Find_EigenValues_With_Schur_Decomposition()
        {
            var secondMatrix = new double[][]
            {
                new double[] { 1, 2, 2, 6, 4 },
                new double[] { 5, 4, 8, 9, 7 },
                new double[] { 5, 3, 9, 7, 8 },
                new double[] { 2, 1, 1, 5, 5 },
                new double[] { 2, 4, 7, 7, 9 }
            };
            var firstResult = SchurDecomposition.Decompose(secondMatrix);
            Assert.Equal(24.42272928, System.Math.Round(firstResult.EigenValues.Values[0].GetNumber(), 8));
            Assert.Equal(0.21755278, System.Math.Round(firstResult.EigenVectors.GetValue(3, 0).GetNumber(), 8));
            Assert.Equal(0.52661734, System.Math.Round(firstResult.EigenVectors.GetValue(4, 0).GetNumber(), 8));
        }
    }
}
