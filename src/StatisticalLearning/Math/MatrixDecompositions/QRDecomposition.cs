// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math.Entities;
using System.Data;

namespace StatisticalLearning.Math.MatrixDecompositions
{
    public class QRDecomposition
    {
        private QRDecomposition(Matrix q, Matrix r)
        {
            Q = q;
            R = r;
        }

        public Matrix Q { get; set; }
        public Matrix R { get; set; }

        /// <summary>
        /// Use the householder method to decompose the matrix.
        /// https://fr.wikipedia.org/wiki/D%C3%A9composition_QR#M%C3%A9thode_de_Householder
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static QRDecomposition Decompose(Matrix matrix)
        {
            Entity[][] qArr = new Entity[matrix.NbRows][];
            Entity[][] rArr = new Entity[matrix.NbColumns][];
            for (int row = 0; row < matrix.NbRows; row++)
            {
                qArr[row] = new Entity[matrix.NbColumns];
                for(int column = 0; column < matrix.NbColumns; column++)
                {
                    qArr[row][column] = 0;
                }
            }

            for (int row = 0; row < matrix.NbColumns; row++)
            {
                rArr[row] = new Entity[matrix.NbColumns];
                for (int column = 0; column < matrix.NbColumns; column++)
                {
                    rArr[row][column] = 0;
                }
            }

            for (int i = 0; i < matrix.NbColumns; i++)
            {
                var a = matrix.GetColumnVector(i);
                a.SetVertical();
                var am = new Matrix(a);
                Matrix substractQ = null;
                for(int j = 0; j < i; j++)
                {
                    var qi = new Matrix(new Matrix(qArr).GetColumnVector(j).SetVertical()).Evaluate();
                    var r = qi.Transpose().Multiply(am).Evaluate();
                    var num = r.GetValue(0, 0);
                    var tmp = qi.Multiply(num);
                    if (num.IsNumberEntity(out NumberEntity ne) && ne.Number.Value < 0)
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
                    substractQ = substractQ.Evaluate();
                    qp = qp.Substract(substractQ);
                }

                qp = qp.Evaluate();
                Entity sum = 0;
                foreach (var val in qp.GetColumnVector(0).Values)
                {
                    sum += MathEntity.Pow(val, 2);
                }

                var lastR = MathEntity.Sqrt(sum);
                rArr[i][i] = lastR;
                var q = qp.Multiply(1 / lastR);
                var columnVector = q.GetColumnVector(0);
                for(int z = 0; z < columnVector.Length; z++)
                {
                    qArr[z][i] = columnVector[z];
                }
            }

            var qMatrix = new Matrix(qArr);
            var rMatrix = new Matrix(rArr);
            qMatrix.Evaluate();
            rMatrix.Evaluate();
            return new QRDecomposition(qMatrix, rMatrix);
        }
    }
}
