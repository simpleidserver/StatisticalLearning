// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.MatrixDecompositions;
using System.Linq;

namespace StatisticalLearning.Statistic.Analysis
{
    public class PrincipalComponentAnalysis
    {
        public PrincipalComponent[] PrincipalComponents { get; private set; }
        public Matrix ComponentVectors { get; private set; }

        public PrincipalComponent[] Compute(double[][] arr, bool isStandardize = false, int maxNbComponents = 2)
        {
            return Compute(new Matrix(arr), isStandardize, maxNbComponents);
        }

        public PrincipalComponent[] Compute(Matrix matrix, bool isStandardize = false, int maxNbComponents = 2)
        {
            int nbRows = matrix.NbRows;
            if (isStandardize)
            {
                matrix = matrix.ComputeMeanCenteredReducedMatrix();
            }
            else
            {
                matrix = matrix.ComputeMeanCenteredMatrix();
            }

            var singularValueDecomposition = new SingularValueDecomposition();
            var svd = singularValueDecomposition.DecomposeGolubReinsch(matrix);
            var singularValues = svd.S.GetDiagonalNumbers();
            ComponentVectors = svd.V.Transpose();
            singularValues = singularValues.ToList().OrderByDescending(_ => _).ToArray();
            var eigenValues = new double[singularValues.Length];
            var sum = 0.0;
            for (int i = 0; i < singularValues.Length; i++)
            {
                eigenValues[i] = singularValues[i] * singularValues[i] / (nbRows - 1);
                sum += eigenValues[i];
            }

            sum = 1 / sum;
            var min = System.Math.Min(maxNbComponents, singularValues.Length);
            PrincipalComponents = new PrincipalComponent[min];
            for (int i = 0; i < min; i++)
            {
                var componentProportion = eigenValues[i] * sum;
                var componentCumulative = componentProportion;
                if (i > 0)
                {
                    componentCumulative = PrincipalComponents[i - 1].Cumulative + componentProportion;
                }

                var record = new PrincipalComponent 
                { 
                    Cumulative = componentCumulative, 
                    Proportion = componentProportion,
                    SingularValue = singularValues[i],
                    EigenValue = eigenValues[i],
                    Eigenvector = ComponentVectors.GetNumberRowVector(i)
                };
                PrincipalComponents[i] = record;
            }

            return PrincipalComponents;
        }

        public Matrix Transform(double[][] arr, bool isStandardize = false)
        {
            return Transform(new Matrix(arr), isStandardize);
        }

        public Matrix Transform(Matrix input, bool isStandardize = false)
        {
            int nbRows = input.NbRows;
            int nbColumns = input.NbColumns;
            if (isStandardize)
            {
                input = input.ComputeMeanCenteredReducedMatrix();
            }
            else
            {
                input = input.ComputeMeanCenteredMatrix();
            }

            var min = PrincipalComponents.Length;
            var result = new double[nbRows][];
            for (int i = 0; i < nbRows; i++)
            {
                result[i] = new double[min];
                for (int pci = 0; pci < PrincipalComponents.Length; pci++)
                {
                    var principalComponent = PrincipalComponents[pci];
                    for(int j = 0; j < principalComponent.Eigenvector.Length; j++)
                    {
                        var vector = principalComponent.Eigenvector[j];
                        result[i][pci] += input.GetNumberValue(i, j) * vector;
                    }
                }
            }

            return new Matrix(result);
        }
    }
}
