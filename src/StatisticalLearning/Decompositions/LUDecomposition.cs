// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;

namespace StatisticalLearning.Decompositions
{
    public class LUDecomposition
    {
        private LUDecomposition(Matrix l, Matrix u)
        {
            L = l;
            U = u;
        }

        public Matrix L { get; private set; }
        public Matrix U { get; private set; }

        public static LUDecomposition Decompose(Matrix matrix)
        {
            var lMatrix = Matrix.BuildIdentityMatrix(matrix.NbRows);
            var uMatrix = matrix.Clone() as Matrix;
            for(int pivot = 0; pivot < uMatrix.NbRows - 1; pivot++)
            {
                var pivotValue = uMatrix.GetValue(pivot, pivot);
                for(int row = pivot + 1; row < uMatrix.NbRows; row++)
                {
                    var referenceValue = uMatrix.GetValue(row, pivot);
                    var div = referenceValue / pivotValue;
                    for (int column = pivot; column < uMatrix.NbColumns; column++)
                    {
                        uMatrix.SetValue(row, column, uMatrix.GetValue(row, column) - div * uMatrix.GetValue(pivot, column));
                        if (pivot == column)
                        {
                            lMatrix.SetValue(row, column, div);
                        }
                    }
                }
            }

            lMatrix.Solve();
            uMatrix.Solve();
            return new LUDecomposition(lMatrix, uMatrix);
        }
    }
}
