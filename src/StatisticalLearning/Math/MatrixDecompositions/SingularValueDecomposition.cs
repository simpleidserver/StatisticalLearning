// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math.Entities;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalLearning.Math.MatrixDecompositions
{
    public class SingularValueDecomposition
    {
        private static string VARIABLE_NAME = "x";

        public SingularValueDecompositionResult Result { get; private set; }

        public SingularValueDecomposition DecomposeNaive(Matrix matrix)
        {
            var ata = matrix.Transpose().Multiply(matrix).Evaluate();
            var ataNormalizedVectors = DecomposeMatrix(ata);
            var sum = Matrix.BuildEmptyMatrix(matrix.NbRows, matrix.NbColumns);
            for(int i = 0; i < ataNormalizedVectors.Count; i++)
            {
                if (sum.NbRows <= i || sum.NbColumns <= i)
                {
                    break;
                }

                sum.SetValue(i, i, MathEntity.Sqrt(ataNormalizedVectors[i].EingenValue).Eval());
            }

            var v = Matrix.BuildEmptyMatrix(ataNormalizedVectors[0].Vector.Length, ataNormalizedVectors.Count);
            var nbColumns = ataNormalizedVectors.Where(_ =>
            {
                if (_.EingenValue == 0)
                {
                    return false;
                }

                return true;
            }).Count();
            var u = Matrix.BuildEmptyMatrix(sum.NbRows, nbColumns);
            for (int column = 0; column < ataNormalizedVectors.Count; column++)
            {
                var eingenValue = (NumberEntity)ataNormalizedVectors[column].EingenValue;
                var vector = ataNormalizedVectors[column].Vector;
                var vectorMatrix = Matrix.BuildEmptyMatrix(vector.Length, 1);
                for (int row = 0; row < vector.Length; row++)
                {
                    v.SetValue(row, column, vector[row]);
                    vectorMatrix.SetValue(row, 0, vector[row]);
                }

                if (eingenValue.Number.Value == 0)
                {
                    continue;
                }

                var val = (Number.Create(1) / MathEntity.Sqrt(ataNormalizedVectors[column].EingenValue)).Eval();
                var uColumn = matrix.Multiply(val).Multiply(vectorMatrix);
                for(int row = 0; row < uColumn.NbRows; row++)
                {
                    u.SetValue(row, column, uColumn.GetValue(row, 0).Eval());
                }
            }

            Result = new SingularValueDecompositionResult
            {
                U = u,
                S = sum,
                V = v,
                Input = matrix
            };
            return this;
        }

        /// <summary>
        /// Use Gobul Reinsch to decompose the matrix.
        /// http://people.duke.edu/~hpgavin/SystemID/References/Golub+Reinsch-NM-1970.pdf
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public SingularValueDecomposition DecomposeGolubReinsch(Matrix a)
        {
            return DecomposeGolubReinsch(a.DoubleValues);
        }

        /// <summary>
        /// Use Gobul Reinsch to decompose the matrix.
        /// http://people.duke.edu/~hpgavin/SystemID/References/Golub+Reinsch-NM-1970.pdf
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public SingularValueDecomposition DecomposeGolubReinsch(double[][] a)
        {
            // http://people.duke.edu/~hpgavin/SystemID/References/Golub+Reinsch-NM-1970.pdf
            int maxIteration = 50;
            double s = 0.0, h = 0.0, f = 0.0, c = 0.0, z = 0.0, y = 0.0;
            int l = 0;
            var eps = double.Epsilon;
            var tol = 1.11022302462516E-49;
            int m = a.Length;
            int n = a[0].Length;
            var u = a.ToArray();
            double[] e = new double[n];
            double[] q = new double[n];
            double[][] v = new double[n][];
            for (int k = 0; k < n; k++)
            {
                v[k] = new double[n];
            }

            double g = 0.0, x = 0.0;
            for (int i = 0; i < n; i++)
            {
                s = 0.0;
                e[i] = g;
                l = i + 1;
                for (int j = i; j < m; j++)
                {
                    s += System.Math.Pow(u[j][i], 2);
                }

                if (s <= tol)
                {
                    g = 0.0;
                }
                else
                {
                    f = u[i][i];
                    if (f < 0.0)
                    {
                        g = System.Math.Sqrt(s);
                    }
                    else
                    {
                        g = -System.Math.Sqrt(s);
                    }

                    h = f * g - s;
                    u[i][i] = f - g;
                    for (int j = l; j < n; j++)
                    {
                        s = 0.0;
                        for (int k = i; k < m; k++)
                        {
                            s += u[k][i] * u[k][j];
                        }
                        f = s / h;
                        for (int k = i; k < m; k++)
                        {
                            u[k][j] = u[k][j] + f * u[k][i];
                        }
                    }
                }

                q[i] = g;
                s = 0.0;
                for (int j = l; j < n; j++)
                {
                    s = s + u[i][j] * u[i][j];
                }

                if (s <= tol)
                {
                    g = 0.0;
                }
                else
                {
                    f = u[i][i + 1];
                    if (f < 0.0)
                    {
                        g = System.Math.Sqrt(s);
                    }
                    else
                    {
                        g = -System.Math.Sqrt(s);
                    }

                    h = f * g - s;
                    u[i][i + 1] = f - g;
                    for (int j = l; j < n; j++)
                    {
                        e[j] = u[i][j] / h;
                    }

                    for (int j = l; j < m; j++)
                    {
                        s = 0.0;
                        for (int k = l; k < n; k++)
                        {
                            s = s + (u[j][k] * u[i][k]);
                        }

                        for (int k = l; k < n; k++)
                        {
                            u[j][k] = u[j][k] + (s * e[k]);
                        }
                    }
                }

                y = System.Math.Abs(q[i]) + System.Math.Abs(e[i]);
                if (y > x)
                {
                    x = y;
                }
            }

            // accumulation of right hand transformations.
            for (int i = n - 1; i >= 0; i--)
            {
                if (g != 0.0)
                {
                    h = g * u[i][i + 1];
                    for (int j = l; j < n; j++)
                    {
                        v[j][i] = u[i][j] / h;
                    }

                    for (int j = l; j < n; j++)
                    {
                        s = 0.0;
                        for (int k = l; k < n; k++)
                        {
                            s += u[i][k] * v[k][j];
                        }

                        for (int k = l; k < n; k++)
                        {
                            v[k][j] += (s * v[k][i]);
                        }
                    }
                }

                for (int j = l; j < n; j++)
                {
                    v[i][j] = 0.0;
                    v[j][i] = 0.0;
                }

                v[i][i] = 1.0;
                g = e[i];
                l = i;
            }

            // accumulation of the left hand transformations.
            for (int i = n - 1; i >= 0; i--)
            {
                l = i + 1;
                g = q[i];
                for (int j = l; j < n; j++)
                {
                    u[i][j] = 0.0;
                }

                if (g != 0.0)
                {
                    h = u[i][i] * g;
                    for (int j = l; j < n; j++)
                    {
                        s = 0.0;
                        for (int k = l; k < m; k++)
                        {
                            s += (u[k][i] * u[k][j]);
                        }

                        f = s / h;
                        for (int k = i; k < m; k++)
                        {
                            u[k][j] = u[k][j] + f * u[k][i];
                        }
                    }

                    for (int j = i; j < m; j++)
                    {
                        u[j][i] = u[j][i] / g;
                    }
                }
                else
                {
                    for (int j = i; j < m; j++)
                    {
                        u[j][i] = 0.0;
                    }
                }

                u[i][i] = u[i][i] + 1.0;
            }

            eps = eps * x;
            // diagonalization of the bidiagonal form.
            for (int k = n - 1; k >= 0; k--)
            {
                for (int iteration = 0; iteration < maxIteration; iteration++)
                {
                    bool testFConvergence = false;
                    // test f splitting.
                    for (l = k; l >= 0; l--)
                    {
                        if (System.Math.Abs(e[l]) <= eps)
                        {
                            testFConvergence = true;
                            break;
                        }

                        if (System.Math.Abs(q[l - 1]) <= eps)
                        {
                            break;
                        }
                    }

                    if (!testFConvergence)
                    {
                        c = 0.0;
                        s = 0.0;
                        var l1 = l - 1;
                        for (int i = l; i < k + 1; i++)
                        {
                            f = s * e[i];
                            e[i] = c * e[i];
                            if (System.Math.Abs(f) <= eps)
                            {
                                break;
                            }

                            g = q[i];
                            h = Hypotenuse(f, g);
                            q[i] = h;
                            c = g / h;
                            s = -f / h;
                            for (int j = 0; j < m; j++)
                            {
                                y = u[j][l1];
                                z = u[j][i];
                                u[j][l1] = y * c + z * s;
                                u[j][i] = -y * s + z * c;
                            }
                        }
                    }

                    // test f convergence.
                    z = q[k];
                    if (l == k)
                    {
                        if (z < 0.0)
                        {
                            q[k] = -z;
                            for (int j = 0; j < n; j++)
                            {
                                v[j][k] = -v[j][k];
                            }
                        }

                        break;
                    }

                    if (iteration >= maxIteration - 1)
                    {
                        break;
                    }

                    // shift from bottom 2x2 minor.
                    x = q[l];
                    y = q[k - 1];
                    g = e[k - 1];
                    h = e[k];
                    f = ((y - z) * (y + z) + (g - h) * (g + h)) / (2.0 * h * y);
                    g = Hypotenuse(f, 1.0);
                    if (f < 0)
                    {
                        f = ((x - z) * (x + z) + h * (y / (f - g) - h)) / x;
                    }
                    else
                    {
                        f = ((x - z) * (x + z) + h * (y / (f + g) - h)) / x;
                    }

                    // next QR transformation.
                    c = 1.0;
                    s = 1.0;
                    for (int i = l + 1; i < k + 1; i++)
                    {
                        g = e[i];
                        y = q[i];
                        h = s * g;
                        g = c * g;
                        z = Hypotenuse(f, h);
                        e[i - 1] = z;
                        c = f / z;
                        s = h / z;
                        f = x * c + g * s;
                        g = -x * s + g * c;
                        h = y * s;
                        y = y * c;
                        for (int j = 0; j < n; j++)
                        {
                            x = v[j][i - 1];
                            z = v[j][i];
                            v[j][i - 1] = x * c + z * s;
                            v[j][i] = -x * s + z * c;
                        }

                        z = Hypotenuse(f, h);
                        q[i - 1] = z;
                        c = f / z;
                        s = h / z;
                        f = c * g + s * y;
                        x = -s * g + c * y;
                        for (int j = 0; j < m; j++)
                        {
                            y = u[j][i - 1];
                            z = u[j][i];
                            u[j][i - 1] = y * c + z * s;
                            u[j][i] = -y * s + z * c;
                        }
                    }

                    e[l] = 0.0;
                    e[k] = f;
                    q[k] = x;
                }
            }

            Result = new SingularValueDecompositionResult
            {
                S = Matrix.BuildIdentityMatrix(q),
                U = u,
                V = v,
                Input = a
            };
            return this;
        }

        public Matrix Inverse()
        {
            return Result.V.Multiply(Result.S.Inverse())
                .Multiply(Result.U.Transpose())
                .Evaluate();
        }

        private static List<NormalizedVector> DecomposeMatrix(Matrix matrix)
        {
            var result = new List<NormalizedVector>();
            VariableEntity lambda = "λ";
            var aatIdentity = Matrix.BuildIdentityMatrix(matrix.NbRows);
            var aatEquation = matrix - (lambda * aatIdentity);
            var aatDeterminant = aatEquation.Determinant().Evaluate(lambda);
            var eingenvalues = aatDeterminant.Solve(lambda);
            foreach (var eingenvalue in eingenvalues.OrderByDescending(_ => _))
            {
                var vectorMatrix = matrix.Substract(Matrix.BuildIdentityMatrix(matrix.NbRows).Multiply(eingenvalue));
                var reducedForm = vectorMatrix.ReducedRowEchelonForm();
                var arr = new Entity[reducedForm.NbRows][];
                for (var variableIndex = 0; variableIndex < reducedForm.NbRows; variableIndex++)
                {
                    arr[variableIndex] = new VariableEntity[] { $"{VARIABLE_NAME}{variableIndex}" };
                }

                var variablesMatrix = new Matrix(arr);
                var matrixEquations = reducedForm.Multiply(variablesMatrix);
                var dic = new Dictionary<VariableEntity, Entity>();
                var vector = new Vector(reducedForm.NbRows);
                for (var currentRow = 0; currentRow < reducedForm.NbRows; currentRow++)
                {
                    var equation = matrixEquations.GetRowVector(currentRow)[0];
                    var pivotIndex = reducedForm.GetPivotColumnIndex(currentRow);
                    if (pivotIndex == null)
                    {
                        continue;
                    }

                    VariableEntity variable = GetVariableName(pivotIndex.Value);
                    var eqResult = equation.Solve(variable);
                    var equationResult = equation.Solve(variable).ElementAt(0);
                    dic.Add(variable, equationResult);
                }

                for(var variableIndex = reducedForm.NbRows - 1; variableIndex >= 0; variableIndex--)
                {
                    var variableName = GetVariableName(variableIndex);
                    var leftPart = dic.FirstOrDefault(_ => _.Key.Name == variableName);
                    var rightPart = dic.FirstOrDefault(_ =>
                    {
                        if (_.Value.IsVariableEntity(out VariableEntity r) && r.Name == variableName)
                        {
                            return true;
                        }

                        return false;
                    });
                    if (!leftPart.Equals(default(KeyValuePair<VariableEntity, Entity>)) && leftPart.Key != null)
                    {
                        var equation = leftPart.Value;
                        for(var index = reducedForm.NbRows -1; index > variableIndex; index--)
                        {
                            equation.Assign(GetVariableName(index), vector[index] as NumberEntity);
                        }

                        vector[variableIndex] = equation.Eval();
                    }
                    else if(!rightPart.Equals(default(KeyValuePair<VariableEntity, Entity>)) && rightPart.Key != null)
                    {
                        vector[variableIndex] = 1;
                    }
                    else
                    {
                        vector[variableIndex] = 0;
                    }
                }

                var length = MathEntity.Sqrt(vector.Sum(_ => MathEntity.Pow(_, 2)));
                var normalizedVector = new double[reducedForm.NbRows];
                for(int i = 0; i < reducedForm.NbRows; i++)
                {
                    if (length.Eval().IsZero())
                    {
                        normalizedVector[i] = 0;
                    }
                    else
                    {
                        normalizedVector[i] = (vector[i] / length).Eval().GetNumber();
                    }
                }


                result.Add(new NormalizedVector(eingenvalue.Eval().GetNumber(), normalizedVector));
            }

            return result;
        }

        private static string GetVariableName(int pivotIndex)
        {
            return $"{VARIABLE_NAME}{pivotIndex}";
        }

        private static double Hypotenuse(double a, double b)
        {
            double r = 0.0;
            double absA = System.Math.Abs(a);
            double absB = System.Math.Abs(b);

            if (absA > absB)
            {
                r = b / a;
                r = absA * System.Math.Sqrt(1 + r * r);
            }
            else if (b != 0)
            {
                r = a / b;
                r = absB * System.Math.Sqrt(1 + r * r);
            }

            return r;
        }

        private class NormalizedVector
        {
            public NormalizedVector(double eingenvalue, double[] vector)
            {
                EingenValue = eingenvalue;
                Vector = vector;
            }

            public double EingenValue { get; set; }
            public double[] Vector { get; set; }
        }
    }
}
