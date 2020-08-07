// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Extensions;
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using StatisticalLearning.Math.MatrixDecompositions;
using StatisticalLearning.Statistic.Probability.Repartition;
using System.Data;
using System.Linq;

namespace StatisticalLearning.Statistic.Regression
{
    public class SimpleLinearRegression
    {
        public SimpleLinearRegressionResult Regress(string inputName, string outputName, DataTable dataTable)
        {
            var inputs = dataTable.GetColumn(inputName);
            var outputs = dataTable.GetColumn(outputName);
            return Regress(inputs, outputs);
        }

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
            var regressionResult = new SimpleLinearRegressionResult
            {
                Intercept = (coefficients[0] as NumberEntity).Number.Value,
                Slope = (coefficients[1] as NumberEntity).Number.Value
            };
            var avgInputs = CalculateAvg(inputs);
            var ssInputs = CalculateSumOfSquares(inputs, avgInputs);
            regressionResult.ResidualSumOfSquares = GetResidualSumOfSquare(regressionResult, inputs, outputs);
            regressionResult.ResidualStandardError = GetResidualStandardError(regressionResult.ResidualSumOfSquares, inputs.Count());
            regressionResult.StandardErrorSlope = CalculateStandardErrorSlope(ssInputs, regressionResult.ResidualStandardError);
            regressionResult.StandardErrorIntercept = CalculateStandardErrorIntercept(ssInputs, avgInputs, regressionResult.ResidualStandardError, inputs.Count());
            regressionResult.TStatisticSlope = CalculateTStatistic(regressionResult.Slope, 0, regressionResult.StandardErrorSlope);
            regressionResult.TStatisticIntercept = CalculateTStatistic(regressionResult.Intercept, 0, regressionResult.StandardErrorIntercept);
            regressionResult.PValueSlope = CalculatePValue(regressionResult.TStatisticSlope, inputs.Count());
            regressionResult.PValueIntercept = CalculatePValue(regressionResult.TStatisticIntercept, inputs.Count());
            return regressionResult;
        }

        private double GetResidualSumOfSquare(SimpleLinearRegressionResult linearRegressionResult, double[] inputs, double[] outputs)
        {
            double result = 0;
            for(int i = 0; i < inputs.Length; i++)
            {
                var input = inputs[i];
                var output = outputs[i];
                var computedY = linearRegressionResult.Compute(input);
                result += System.Math.Pow(output - computedY, 2);
            }

            return result;
        }

        private double GetResidualStandardError(double rse, int n)
        {
            return System.Math.Sqrt((rse / (n - 2)));
        }

        private double CalculateStandardErrorIntercept(double ss, double avg, double rse, int n)
        {
            var divResult = ((System.Math.Pow(avg, 2) * n) + ss) / (n * ss);
            return System.Math.Sqrt(System.Math.Pow(rse, 2) * divResult);
        }

        private double CalculateStandardErrorSlope(double ssm, double rse)
        {
            var dividend = System.Math.Pow(rse, 2);
            return System.Math.Sqrt(dividend / ssm);
        }

        private double CalculateAvg(double[] values)
        {
            double totalSum = 0;
            foreach(var value in values)
            {
                totalSum += value;
            }

            return totalSum / values.Count();
        }

        private double CalculateSumOfSquares(double[] values, double avg)
        {
            double result = 0;
            foreach(var value in values)
            {
                result += System.Math.Pow((value - avg), 2);
            }

            return result;
        }

        private double CalculateTStatistic(double estimatedCoefficient, double referenceValue, double se)
        {
            return (estimatedCoefficient - referenceValue) / (se);
        }

        private double CalculatePValue(double tStatistic, int nb)
        {
            int degreeOfFreedom = nb - 2;
            var studentLaw = new StudentLaw();
            return  studentLaw.ComputeUpperCumulative(tStatistic, degreeOfFreedom) * 2;
        }
    }
}