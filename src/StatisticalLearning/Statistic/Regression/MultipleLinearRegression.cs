// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using StatisticalLearning.Math.MatrixDecompositions;
using StatisticalLearning.Statistic.Probability.Repartition;
using System.Linq;

namespace StatisticalLearning.Statistic.Regression
{
    public class MultipleLinearRegression
    {
        private readonly SingularValueDecomposition _singularValueDecomposition;
        private readonly MatrixDecompositionAlgs _alg;
        private LinearRegressionResult _linearRegressionResult;

        public MultipleLinearRegression(MatrixDecompositionAlgs alg)
        {
            _alg = alg;
            _singularValueDecomposition = new SingularValueDecomposition();
        }

        public MultipleLinearRegression() : this(MatrixDecompositionAlgs.NAIVE) { }

        public LinearRegressionResult LinearRegression => _linearRegressionResult;

        public SingularValueDecomposition SingularValueDecomposition => _singularValueDecomposition;

        public MultipleLinearRegression Regress(Matrix matrix, Vector output, bool addOne = true)
        {
            if(addOne)
            {
                matrix = matrix.AddColumn(1);
            }

            output.SetVertical();
            if (_alg == MatrixDecompositionAlgs.NAIVE)
            {
                _singularValueDecomposition.DecomposeNaive(matrix);
            }
            else
            {
                _singularValueDecomposition.DecomposeGolubReinsch(matrix);
            }

            var coefficients = _singularValueDecomposition.Inverse()
                .Multiply((Matrix)output)
                .Evaluate()
                .GetColumnVector(0);
            var result = new LinearRegressionResult();
            for(int i = 0; i < coefficients.Length; i++)
            {
                var coefficient = coefficients[i].GetNumber();
                var coefficientResult = new CoefficientResult
                {
                    Value = coefficient
                };
                if (i ==  0)
                {
                    result.Intercept = coefficientResult;
                }
                else
                {
                    result.SlopeLst.Add(coefficientResult);
                }
            }

            _linearRegressionResult = result;
            UpdateInformation(matrix, output, addOne);
            return this;
        }

        public void UpdateInformation(Matrix matrix, Vector output, bool addOne = true, bool useCovariance = true)
        {
            int n = matrix.NbRows;
            int k = matrix.NbColumns;
            if (addOne)
            {
                k = k - 1;
            }

            var avgOutputs = output.Avg();
            var ssOutputs = output.SumOfSquares(avgOutputs);
            var coefficients = new double[1 + _linearRegressionResult.SlopeLst.Count()];
            coefficients[0] = _linearRegressionResult.Intercept.Value;
            for(int  i = 0; i < _linearRegressionResult.SlopeLst.Count(); i++)
            {
                coefficients[i + 1] = _linearRegressionResult.SlopeLst.ElementAt(i).Value;
            }

            var rss = ResidualSumOfSquare(coefficients, matrix.Values, output);
            var mse = MeanSquareError(rss, n, k);
            var rse = ResidualStandardError(mse);
            _linearRegressionResult.SlopeLst.Clear();
            Matrix informationMatrix = null;
            if (useCovariance)
            {
                informationMatrix = matrix.Transpose().Multiply(matrix).Inverse().Multiply(mse).Evaluate();
            }
            else
            {
                informationMatrix = _singularValueDecomposition.Inverse();
            }

            UpdateStandardErrors(coefficients, informationMatrix, n, k);
            _linearRegressionResult.ResidualSumOfSquare = rss;
            _linearRegressionResult.MeanSquareError = mse;
            _linearRegressionResult.ResidualStandardError = rse;
            _linearRegressionResult.RSquare = RSquare(rss, ssOutputs);
        }

        public double Transform(double[] input)
        {
            double result = 0.0;
            for (int i = 0; i < input.Length; i++)
            {
                if (i == 0)
                {
                    result += input[i] * _linearRegressionResult.Intercept.Value;
                }
                else
                {
                    result += input[i] * _linearRegressionResult.SlopeLst.ElementAt(i - 1).Value;
                }
            }

            return result;
        }

        private void UpdateStandardErrors(double[] coefficients, Matrix informationMatrix, int n, int k)
        {
            for (int index = 0; index < coefficients.Length; index++)
            {
                var coefficient = coefficients[index];
                var standardError = System.Math.Sqrt(informationMatrix.GetValue(index, index).GetNumber());
                var tStat = TStatistic(coefficient, standardError);
                var pValue = PValue(tStat, n, k);
                var coefficientResult = new CoefficientResult
                {
                    Value = coefficient,
                    StandardError = standardError,
                    TStatistic = tStat,
                    PValue = pValue
                };
                if (index == 0)
                {
                    _linearRegressionResult.Intercept = coefficientResult;
                }
                else
                {
                    _linearRegressionResult.SlopeLst.Add(coefficientResult);
                }
            }
        }

        private static double ResidualSumOfSquare(Vector coefficients, Entity[][] inputs, Vector outputs)
        {
            Entity result = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                var input = inputs[i];
                var output = outputs[i];
                Entity computedY = 0.0;
                for (int y = 0; y < input.Length; y++)
                {
                    computedY += coefficients[y] * input[y];
                }

                result = (result + MathEntity.Pow(computedY - output, 2)).Eval();
            }

            return result.Eval().GetNumber();
        }

        private static double MeanSquareError(double rse, int n, int p)
        {
            return rse / (n - (p + 1));
        }

        private static double ResidualStandardError(double mse)
        {
            return System.Math.Sqrt(mse);
        }

        private static double TStatistic(double estimatedCoefficient, double se)
        {
            return estimatedCoefficient / se;
        }

        private static double PValue(double tStatistic, int n, int k)
        {
            int degreeOfFreedom = n - k - 1;
            var studentLaw = new StudentLaw();
            return studentLaw.ComputeUpperCumulative(tStatistic, degreeOfFreedom) * 2;
        }

        private static double RSquare(Entity rss, Entity tss)
        {
            return (1 - (rss / tss)).GetNumber();
        }
    }
}
