// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Entities;
using StatisticalLearning.Math;
using StatisticalLearning.Numeric;
using Xunit;

namespace StatisticalLearning.Tests
{
    public class MatrixFixture
    {
        [Fact]
        public void When_Build_Identity_Matrix()
        {
            var result = Matrix.BuildIdentityMatrix(2);
            var exceptedMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(0) },
                new Entity[] { Number.Create(0), Number.Create(1) }
            });
            Assert.True(result.Equals(exceptedMatrix));
        }

        [Fact]
        public void When_Multiply_Two_Matrix()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(2), Number.Create(3) },
                new Entity[] { Number.Create(4), Number.Create(5), Number.Create(6) },
            });
            var secondMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(7), Number.Create(8) },
                new Entity[] { Number.Create(9), Number.Create(10) },
                new Entity[] { Number.Create(11), Number.Create(12) },
            });
            var exceptedMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(58), Number.Create(64) },
                new Entity[] { Number.Create(139), Number.Create(154) }
            });
            var result = firstMatrix.Multiply(secondMatrix);
            result.Solve();
            Assert.True(result.Equals(exceptedMatrix));
        }

        [Fact]
        public void When_Substract_Two_Matrix()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(2), Number.Create(3) },
                new Entity[] { Number.Create(4), Number.Create(5), Number.Create(6) },
            });
            var secondMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(0), Number.Create(1), Number.Create(2) },
                new Entity[] { Number.Create(3), Number.Create(4), Number.Create(5) },
            });
            var exceptedMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(1), Number.Create(1) },
                new Entity[] { Number.Create(1), Number.Create(1), Number.Create(1) }
            });
            var result = firstMatrix.Substract(secondMatrix);
            result.Solve();
            Assert.True(result.Equals(exceptedMatrix));
        }

        [Fact]
        public void When_Transpose_Matrix()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(2), Number.Create(3) },
                new Entity[] { Number.Create(4), Number.Create(5), Number.Create(6) }
            });
            var exceptedMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(4) },
                new Entity[] { Number.Create(2), Number.Create(5) },
                new Entity[] { Number.Create(3), Number.Create(6) }
            });
            var result = firstMatrix.Transpose();
            var secondResult = result.Transpose();
            Assert.True(result.Equals(exceptedMatrix));
            Assert.True(secondResult.Equals(firstMatrix));
        }

        [Fact]
        public void When_Compute_Determinant()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(5), Number.Create(4), Number.Create(2) },
                new Entity[] { Number.Create(1), Number.Create(8), Number.Create(2) },
                new Entity[] { Number.Create(3), Number.Create(1), Number.Create(2) }
            });
            var secondMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(2), Number.Create(3), Number.Create(4) },
                new Entity[] { Number.Create(5), Number.Create(6), Number.Create(7), Number.Create(8) },
                new Entity[] { Number.Create(9), Number.Create(10), Number.Create(11), Number.Create(12) },
                new Entity[] { Number.Create(13), Number.Create(14), Number.Create(15), Number.Create(16) },
            });
            var firstDeterminant = firstMatrix.ComputeDeterminant().Eval() as NumberEntity;
            var secondDeterminant = secondMatrix.ComputeDeterminant().Eval() as NumberEntity;
            Assert.NotNull(firstDeterminant);
            Assert.NotNull(secondDeterminant);
            Assert.Equal(40, firstDeterminant.Number.Value);
            Assert.Equal(0, secondDeterminant.Number.Value);
        }
    }
}