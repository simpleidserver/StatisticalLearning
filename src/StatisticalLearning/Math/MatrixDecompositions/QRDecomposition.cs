// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
namespace StatisticalLearning.Math.MatrixDecompositions
{
    public class QRDecomposition
    {
        public QRDecompositionResult Decompose(Matrix matrix)
        {
            // Use the householder method to decompose the matrix
            // https://fr.wikipedia.org/wiki/D%C3%A9composition_QR#M%C3%A9thode_de_Householder
            double[][] qArr = new double[matrix.NbRows][];
            double[][] rArr = new double[matrix.NbColumns][];
            for (int row = 0; row < matrix.NbRows; row++)
            {
                qArr[row] = new double[matrix.NbColumns];
            }

            for (int i = 0; i < matrix.NbColumns; i++)
            {
                rArr[i] = new double[matrix.NbColumns];
                var a = matrix.GetNumberColumnVector(i);
                var am = new Matrix(a);
                Matrix substractQ = null;
                for(int j = 0; j < i; j++)
                {
                    var qi = new Matrix(new Matrix(qArr).GetNumberColumnVector(j));
                    var r = qi.Transpose().Multiply(am).Eval();
                    var num = r.GetNumberValue(0, 0);
                    var tmp = qi.Multiply(Number.Create(num));
                    if (num < 0)
                    {
                        tmp = tmp.Multiply(Number.Create(-1));
                    }

                    if (substractQ == null)
                    {
                        substractQ = tmp;
                    }
                    else
                    {
                        substractQ = substractQ.Substract(tmp);
                    }

                    rArr[j][i] = num;
                }

                var qp = am;
                if (substractQ != null)
                {
                    substractQ = substractQ.Eval();
                    qp = qp.Substract(substractQ);
                }

                qp = qp.Eval();
                double sum = 0;
                foreach (double val in qp.GetNumberColumnVector(0))
                {
                    sum += System.Math.Pow(val, 2);
                }

                var lastR = System.Math.Sqrt(sum);
                rArr[i][i] = lastR;
                var q = qp.Multiply(Number.Create(1) / Number.Create(lastR));
                var columnVector = q.GetNumberColumnVector(0);
                for(int z = 0; z < columnVector.Length; z++)
                {
                    qArr[z][i] = columnVector[z];
                }
            }

            return new QRDecompositionResult
            {
                Q = new Matrix(qArr),
                R = new Matrix(rArr)
            };
        }
    }
}
