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
        public void When_Decompose_SVD_Matrix()
        {
            var matrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(3), Number.Create(2), Number.Create(2) },
                new Entity[] { Number.Create(2), Number.Create(3), Number.Create(-2) }
            });
            var secondMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(4), Number.Create(0) },
                new Entity[] { Number.Create(3), Number.Create(-5) }
            });
            var thirdMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(80) },
                new Entity[] { Number.Create(1), Number.Create(60) },
                new Entity[] { Number.Create(1), Number.Create(10) },
                new Entity[] { Number.Create(1), Number.Create(20) },
                new Entity[] { Number.Create(1), Number.Create(30) }
            });
            var decomposition = new SingularValueDecomposition();
            var result = decomposition.Decompose(matrix);
            var secondResult = decomposition.Decompose(secondMatrix);
            var thirdResult = decomposition.Decompose(thirdMatrix);
            var exceptedMatrix = result.U.Multiply(result.S).Multiply(result.V.Transpose()).Eval();
            var secondExceptedMatrix = secondResult.U.Multiply(secondResult.S).Multiply(secondResult.V.Transpose()).Eval();
            var thirdExceptedMatrix = thirdResult.U.Multiply(thirdResult.S).Multiply(thirdResult.V.Transpose()).Eval();
            Assert.True(matrix.Equals(exceptedMatrix));
            Assert.True(secondMatrix.Equals(secondExceptedMatrix));
        }
    }
}
