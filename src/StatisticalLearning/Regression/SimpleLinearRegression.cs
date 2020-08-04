// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Decompositions;
using StatisticalLearning.Entities;
using StatisticalLearning.Math;
using StatisticalLearning.Numeric;
using System.Linq;

namespace StatisticalLearning.Regression
{
    public class SimpleLinearRegression
    {
        public SimpleLinearRegressionResult Regress(double[] inputs, double[] outputs)
        {
            var arr = new Entity[inputs.Length][];
            for(int i = 0; i < inputs.Length; i++)
            {
                arr[i] = new Entity[] { Number.Create(1), Number.Create(inputs[i]) };
            }

            var matrix = new Matrix(arr);
            var decomposition = new SingularValueDecomposition();
            var result = decomposition.Decompose(matrix);
            var coefficients = result.V.Multiply(result.S.Inverse()).Multiply(result.U.Transpose()).Multiply(outputs.Select(_ => (NumberEntity)Number.Create(_)).ToArray()).Eval().GetColumnVector(0);
            return new SimpleLinearRegressionResult
            {
                Intercept = (coefficients[1] as NumberEntity).Number.Value,
                Slope = (coefficients[0] as NumberEntity).Number.Value
            };
        }
    }
}