// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using StatisticalLearning.Math.MatrixDecompositions;
using Xunit;

namespace StatisticalLearning.Tests.Math.MatrixDecompositions
{
    public class LUDecompositionFixture
    {
        [Fact]
        public void When_Decompose_Matrix_With_LU()
        {
            var matrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(6), Number.Create(8), Number.Create(2) },
                new Entity[] { Number.Create(-6), Number.Create(7), Number.Create(-4) },
                new Entity[] { Number.Create(2), Number.Create(-4), Number.Create(3) }
            });
            var decompose = LUDecomposition.Decompose(matrix);
            var result = (decompose.L * decompose.U).Solve();
            Assert.True(result.Equals(matrix));
        }
    }
}
