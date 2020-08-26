// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using System.Linq;

namespace StatisticalLearning.Statistic.Regression
{
    public abstract class PartialLeastSquareRegression
    {
        protected MultipleLinearRegression _multipleLinearRegression;

        public PartialLeastSquareRegression()
        {
            _multipleLinearRegression = new MultipleLinearRegression(MatrixDecompositionAlgs.GOLUB_REINSCH);
        }

        /// <summary>
        // https://www.cs.ubc.ca/~nando/340-2012/lectures/l21.pdf
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        public void Regress(Matrix inputs, Vector outputs)
        {
            inputs = inputs.AddColumn(1);
            var theta = new Vector(inputs.NbColumns);
            var previousTheta = new Vector(inputs.NbColumns);
            for (int i = 0; i < inputs.NbColumns; i++)
            {
                previousTheta[i] = double.MaxValue;
            }

            while (theta.Substract(previousTheta).Abs().Max().GetNumber() >= 1e-6)
            {
                Vector a = inputs.Multiply(theta.SetHorizontal()).SumAllRows();
                var pi = a.Transform(_ => Inverse(_.GetNumber()));
                var sx = inputs.Multiply(pi.Substract(pi.Multiply(pi)).SetVertical()).Evaluate();
                var xsx = inputs.Transpose().Multiply(sx).Evaluate();
                Vector sxTheta = sx.Multiply(theta.SetHorizontal()).SumAllRows();
                var result = _multipleLinearRegression.Regress(xsx,
                    inputs.Transpose().Multiply(sxTheta.Sum(outputs.Substract(pi))).SumAllRows().SetHorizontal(),
                    false);
                previousTheta = theta;
                theta = result.LinearRegression.GetCoefficients();
            }

            _multipleLinearRegression.UpdateInformation(inputs, outputs, useCovariance: false);
        }

        public LinearRegressionResult LinearRegressionResult => _multipleLinearRegression.LinearRegression;

        public Vector Transform(Matrix inputs)
        {
            var vector = new Vector(inputs.NbRows);
            for(int row = 0; row < inputs.NbRows; row++)
            {
                vector[row] = Compute(inputs.GetRowVector(row).GetNumbers());
            }

            return vector;
        }

        public double Compute(double[] input)
        {
            var lst = input.ToList();
            lst.Insert(0, 1);
            var result = _multipleLinearRegression.Transform(lst.ToArray());
            return Inverse(result);
        }

        protected abstract double Transform(double value);

        protected abstract double Inverse(double value);
    }
}
