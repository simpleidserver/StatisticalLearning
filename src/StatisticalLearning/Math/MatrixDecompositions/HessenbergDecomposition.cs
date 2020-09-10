// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math.Entities;
using System.Linq;

namespace StatisticalLearning.Math.MatrixDecompositions
{
    /// <summary>
    /// https://www3.math.tu-berlin.de/Vorlesungen/SS11/NumMath2/Materials/hessenberg_eng.pdf
    /// </summary>
    public class HessenbergDecomposition
    {
        private HessenbergDecomposition(Matrix a, Matrix h, Matrix p)
        {
            A = a;
            H = h;
            P = p;
        }

        public Matrix A { get; set; }
        public Matrix H { get; set; }
        public Matrix P { get; set; }

        /// <summary>
        /// @Documentation : // https://rstudio-pubs-static.s3.amazonaws.com/267386_b1c34cc8d086475a8888881b79baca04.html
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static HessenbergDecomposition Decompose(Matrix matrix)
        {
            int n = matrix.NbColumns;
            var a = (Matrix)matrix.Clone();
            var pResult = Matrix.BuildIdentityMatrix(n);
            for (int k = 0; k < n - 2; k++)
            {
                // W = I-2vv'
                Vector x = a.GetColumnVector(k).Values.Skip(1 + k).ToArray();
                var w = new Vector(x.Length);
                var transformedColumn = new Vector(x.Length);
                w[0] = x.Norm(true);
                transformedColumn[0] = w[0];
                for(int i = 1; i < x.Length; i++)
                {
                    transformedColumn[i] = x[i];
                }

                var v = w.Substract(x);
                var vHorizontalMatrix = new Matrix(v);
                v.SetVertical();
                var vVerticalMatrix = new Matrix(v);
                var vvt = vVerticalMatrix.Multiply(vHorizontalMatrix).Evaluate();
                var vtv = vHorizontalMatrix.Multiply(vVerticalMatrix).Evaluate();
                var p = vvt.Div(vtv).Evaluate();
                var identity = Matrix.BuildIdentityMatrix(p.NbColumns);
                var houseHolderMatrix = identity.Substract(p.Multiply(2)).Evaluate();
                var newHouseHolderMatrix = Matrix.BuildIdentityMatrix(n);
                newHouseHolderMatrix.Replace(houseHolderMatrix, k + 1, k + 1);
                pResult = pResult.Multiply(newHouseHolderMatrix).Evaluate(); ;
                a = newHouseHolderMatrix.Multiply(a).Multiply(newHouseHolderMatrix.Inverse()).Evaluate();
            }

            return new HessenbergDecomposition(matrix, a, pResult);
        }
    }
}
