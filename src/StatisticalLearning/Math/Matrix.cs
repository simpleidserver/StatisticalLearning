// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Entities;
using StatisticalLearning.Numeric;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalLearning.Math
{
    public class Matrix : IEquatable<Matrix>, ICloneable
    {
        private readonly Entity[][] _arr;
        public static Matrix operator *(Matrix a, Matrix b) => a.Multiply(b);
        public static Matrix operator -(Matrix a, Matrix b) => a.Substract(b);
        public static Matrix operator *(Entity a, Matrix b) => b.Multiply(a);
        public static Matrix operator *(Matrix a, Entity b) => a.Multiply(b);

        public Matrix(Entity[][] arr)
        {
            _arr = arr;
        }

        public static Matrix BuildIdentityMatrix(int nbRowColumn)
        {
            var result = new Entity[nbRowColumn][];
            for(var row = 0; row < nbRowColumn; row++)
            {
                var rowContent = new Entity[nbRowColumn];
                for (var column = 0; column < nbRowColumn; column++)
                {
                    if (column == row)
                    {
                        rowContent[column] = Number.Create(1);
                    }
                    else
                    {
                        rowContent[column] = Number.Create(0);
                    }
                }

                result[row] = rowContent;
            }

            return new Matrix(result);
        }

        public Entity[][] Arr => _arr;
        public int NbRows => Arr.Length;
        public int NbColumns => Arr[0].Length;

        public Matrix Multiply(Entity entity)
        {
            var result = new Entity[NbRows][];
            for (var row = 0; row < NbRows; row++)
            {
                var values = new Entity[NbColumns];
                for (var column = 0; column < NbColumns; column++)
                {
                    values[column] = GetValue(row, column) * entity;
                }

                result[row] = values;
            }

            return new Matrix(result);
        }


        public Matrix Multiply(Matrix matrix)
        {
            var result = new Entity[NbRows][];
            for(var row = 0; row < NbRows; row++)
            {
                var values = new Entity[matrix.NbColumns];
                for(var targetColumn = 0; targetColumn < matrix.NbColumns; targetColumn++)
                {
                    var targetColumnVector = matrix.GetColumnVector(targetColumn);
                    Entity value = null;
                    for (var column = 0; column < NbColumns; column++)
                    {
                        var source = GetValue(row, column);
                        var target = targetColumnVector[column];
                        if (value == null)
                        {
                            value = source * target;
                        }
                        else
                        {
                            value += source * target;
                        }
                    }

                    values[targetColumn] = value;
                }

                result[row] = values;
            }

            return new Matrix(result);
        }
        
        public Matrix Substract(Matrix matrix)
        {
            var result = new Entity[NbRows][];
            for (var row = 0; row < NbRows; row++)
            {
                var values = new Entity[NbColumns];
                for (var column = 0; column < NbColumns; column++)
                {
                    values[column] = GetValue(row, column) - matrix.GetValue(row, column);
                }

                result[row] = values;
            }

            return new Matrix(result);
        }

        public Matrix Solve()
        {
            for(var row = 0; row < NbRows; row++)
            {
                for(var column = 0; column < NbColumns; column++)
                {
                    var value = GetValue(row, column);
                    SetValue(row, column, value.Eval());
                }
            }

            return this;
        }

        public Matrix Transpose()
        {
            var result = new Entity[NbColumns][];
            for(var column = 0; column < NbColumns; column++)
            {
                var columnVector = GetColumnVector(column);
                result[column] = columnVector;
            }

            return new Matrix(result);
        }

        public Entity GetValue(int row, int column)
        {
            return _arr[row][column];
        }

        public void SetValue(int row, int column, Entity entity)
        {
            _arr[row][column] = entity;
        }

        public Entity[] GetColumnVector(int column)
        {
            var result = new Entity[NbRows];
            for(var i = 0; i < NbRows; i++)
            {
                result[i] = _arr[i][column];
            }

            return result;
        }

        public Entity[] GetRowVector(int row)
        {
            return _arr[row];
        }

        public bool Equals(Matrix x)
        {
            if (x == null)
            {
                return false;
            }

            return x.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return NbRows.GetHashCode() + NbColumns.GetHashCode() + ToString().GetHashCode();
        }

        public override string ToString()
        {
            var lst = new List<string>();
            for (var row = 0; row < NbRows; row++)
            {
                var vector = GetRowVector(row);
                lst.Add($"[ {string.Join(",", vector.Select(_ => _.ToString())) } ]");
            }

            return $"[ {string.Join(",", lst.ToArray())} ]";
        }

        public object Clone()
        {
            return new Matrix(_arr.Select(_ => _.ToArray()).ToArray());
        }
    }
}
