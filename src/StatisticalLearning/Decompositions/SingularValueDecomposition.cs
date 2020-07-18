// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Entities;
using StatisticalLearning.Math;

namespace StatisticalLearning.Decompositions
{
    public class SingularValueDecomposition
    {
        public void Decompose(Matrix matrix)
        {
            var aat = matrix.Multiply(matrix.Transpose()).Solve();
            var ata = matrix.Transpose().Multiply(matrix).Solve();
            VariableEntity lambda = "λ";
            var aatIdentity = Matrix.BuildIdentityMatrix(aat.NbRows);
            var aatEquation = aat - (lambda * aatIdentity);
            var aatDeterminant = aatEquation.ComputeDeterminant().Evaluate(lambda);
            var eingenvalues = aatDeterminant.Solve(lambda);

        }
    }
}
