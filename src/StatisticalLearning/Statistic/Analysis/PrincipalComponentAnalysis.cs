// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using StatisticalLearning.Math.MatrixDecompositions;
using System.Linq;

namespace StatisticalLearning.Statistic.Analysis
{
    public class PrincipalComponentAnalysis
    {
        private bool _isStandardize = false;
        private int _maxNbComponents;

        public PrincipalComponentAnalysis()
        {
            _isStandardize = false;
            _maxNbComponents = 2;
        }

        public PrincipalComponentAnalysis(bool isStandardize, int maxNbComponents)
        {
            _isStandardize = isStandardize;
            _maxNbComponents = maxNbComponents;
        }

        public PrincipalComponent[] PrincipalComponents { get; private set; }
        public Matrix ComponentVectors { get; private set; }

        public PrincipalComponentAnalysis Compute(Matrix matrix)
        {
            int nbRows = matrix.NbRows;
            if (_isStandardize)
            {
                matrix = matrix.CenteredMeanReducedMatrix();
            }
            else
            {
                matrix = matrix.CenteredMeanMatrix();
            }

            matrix.Evaluate();
            var singularValueDecomposition = new SingularValueDecomposition();
            var svd = singularValueDecomposition.DecomposeGolubReinsch(matrix).Result;
            var singularValues = svd.S.GetDiagonal();
            ComponentVectors = svd.V.Transpose();
            singularValues = singularValues.ToList().OrderByDescending(_ => _).ToArray();
            var eigenValues = new Entity[singularValues.Length];
            Entity sum = 0.0;
            for (int i = 0; i < singularValues.Length; i++)
            {
                eigenValues[i] = singularValues[i] * singularValues[i] / (nbRows - 1);
                sum += eigenValues[i];
            }

            sum = 1 / sum;
            var min = System.Math.Min(_maxNbComponents, singularValues.Length);
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
                    Cumulative = componentCumulative.Eval(), 
                    Proportion = componentProportion.Eval(),
                    SingularValue = singularValues[i].Eval(),
                    EigenValue = eigenValues[i].Eval(),
                    Eigenvector = ComponentVectors.GetRowVector(i)
                };
                PrincipalComponents[i] = record;
            }

            return this;
        }

        public Matrix Transform(Matrix input)
        {
            int nbRows = input.NbRows;
            if (_isStandardize)
            {
                input = input.CenteredMeanReducedMatrix();
            }
            else
            {
                input = input.CenteredMeanMatrix();
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
                        result[i][pci] += input.GetValue(i, j).Eval().GetNumber() * vector.GetNumber();
                    }
                }
            }

            return result;
        }
    }
}
