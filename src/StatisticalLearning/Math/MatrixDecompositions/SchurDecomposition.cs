// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using System;

namespace StatisticalLearning.Math.MatrixDecompositions
{
    public class SchurDecomposition
    {
        private SchurDecomposition(Vector eigenValues, Matrix eigenVectors)
        {
            EigenValues = eigenValues;
            EigenVectors = eigenVectors;
        }

        public Vector EigenValues { get; set; }
        public Matrix EigenVectors { get; set; }

        /// <summary>
        /// http://web.math.ucsb.edu/~padraic/ucsb_2013_14/math108b_w2014/math108b_w2014_lecture5.pdf
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static SchurDecomposition Decompose(Matrix matrix, double min = 1E-6)
        {
            var a = (Matrix)matrix.Clone();
            var eingenVectors = Matrix.BuildIdentityMatrix(matrix.NbColumns);
            while(true)
            {
                var res = QRDecomposition.Decompose(a);
                a = res.R.Multiply(res.Q).Evaluate();
                eingenVectors = eingenVectors.Multiply(res.Q).Evaluate();
                if (CheckConvergence(a, min))
                {
                    break;
                }
            }

            return new SchurDecomposition(a.GetDiagonal(), eingenVectors);
        }

        private static bool CheckConvergence(Matrix matrix, double min)
        {
            for (int column = 0; column < matrix.NbColumns; column++)
            {
                for(int row = column + 1; row < matrix.NbRows; row++)
                {
                    if (matrix.GetValue(row, column).GetNumber() > min)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
