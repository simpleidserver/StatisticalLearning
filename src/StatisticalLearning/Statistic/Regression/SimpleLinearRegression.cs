// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Extensions;
using System.Data;

namespace StatisticalLearning.Statistic.Regression
{
    public class SimpleLinearRegression
    {
        public LinearRegressionResult Regress(string inputName, string outputName, DataTable dataTable)
        {
            var inputs = dataTable.GetColumn(inputName);
            var outputs = dataTable.GetColumn(outputName);
            return Regress(inputs, outputs);
        }

        public LinearRegressionResult Regress(double[] inputs, double[] outputs)
        {
            var multipleLinearRegression = new MultipleLinearRegression();
            var newInputs = new double[inputs.Length][];
            for(int i = 0; i < inputs.Length; i++)
            {
                newInputs[i] = new double[] { inputs[i] };
            }

            return multipleLinearRegression.Regress(newInputs, outputs);
        }
    }
}