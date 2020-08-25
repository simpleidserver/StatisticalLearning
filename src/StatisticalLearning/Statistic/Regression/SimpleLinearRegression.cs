// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Extensions;
using System.Data;
using System.Linq;

namespace StatisticalLearning.Statistic.Regression
{
    public class SimpleLinearRegression
    {
        private readonly MultipleLinearRegression _multipleLinearRegression;
        private LinearRegressionResult _linearRegression;

        public SimpleLinearRegression(MatrixDecompositionAlgs alg)
        {
            _multipleLinearRegression = new MultipleLinearRegression(alg);
        }

        public SimpleLinearRegression() : this(MatrixDecompositionAlgs.NAIVE) { }

        public LinearRegressionResult LinearRegression => _linearRegression;

        public SimpleLinearRegression Regress(string inputName, string outputName, DataTable dataTable)
        {
            var inputs = dataTable.GetColumn(inputName);
            var outputs = dataTable.GetColumn(outputName);
            return Regress(inputs, outputs);
        }

        public SimpleLinearRegression Regress(double[] inputs, double[] outputs)
        {
            var newInputs = new double[inputs.Length][];
            for(int i = 0; i < inputs.Length; i++)
            {
                newInputs[i] = new double[] { inputs[i] };
            }
                
            var result = _multipleLinearRegression.Regress(newInputs, outputs);
            _linearRegression = result.LinearRegression;
            return this;
        }
    }
}