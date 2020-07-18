// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Decompositions;
using StatisticalLearning.Entities;
using StatisticalLearning.Math;
using StatisticalLearning.Numeric;
using Xunit;

namespace StatisticalLearning.Tests
{
    public class SingularValueDecompositionFixture
    {
        [Fact]
        public void When_Decompose_Matrix()
        {
            var matrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(3), Number.Create(2), Number.Create(2) },
                new Entity[] { Number.Create(2), Number.Create(3), Number.Create(-2) }
            });
            var decomposition = new SingularValueDecomposition();
            // TROUVER LE PROBLEME.
            decomposition.Decompose(matrix);
        }
    }
}
