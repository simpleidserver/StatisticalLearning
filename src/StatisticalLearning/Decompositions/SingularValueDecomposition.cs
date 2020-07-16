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
            var equation = aat - (lambda * aatIdentity);

            // pouvoir faire des opérations avec des variables.
            var sss = "";
        }
    }
}
