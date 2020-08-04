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
        private Entity[][] _arr;
        public static Matrix operator *(Matrix a, Matrix b) => a.Multiply(b);
        public static Matrix operator -(Matrix a, Matrix b) => a.Substract(b);
        public static Matrix operator *(Entity a, Matrix b) => b.Multiply(a);
        public static Matrix operator *(Matrix a, Entity b) => a.Multiply(b);

        public Matrix(Entity[][] arr)
        {
            _arr = arr;
        }

        public int? GetPivotColumnIndex(int row)
        {
            var rowVector = GetRowVector(row);
            for (int i = 0; i < rowVector.Length; i++)
            {
                if (rowVector[i].IsNumberEntity(out NumberEntity n) && n.Number.Value == 1)
                {
                    return i;
                }
            }

            return null;
        }

        public Entity ComputeDeterminant()
        {
            if (NbRows == 2 && NbColumns == 2)
            {
                return GetValue(0, 0) * GetValue(1, 1) - GetValue(0, 1) * GetValue(1, 0);
            }

            int nbDeterminant = 1;
            Entity result = null;
            for(int column = 0; column < NbColumns; column++)
            {
                var determinant = GetValue(0, column) * GetDeterminantMatrix(0, column).ComputeDeterminant();
                if (result == null)
                {
                    result = determinant;
                }
                else
                {
                    if (nbDeterminant % 2 == 0)
                    {
                        result -= determinant;
                    }
                    else
                    {
                        result += determinant;
                    }
                }

                nbDeterminant++;
            }

            return result;
        }

        public Matrix Eval()
        {
            for (int row = 0; row < NbRows; row++)
            {
                for(int column = 0; column < NbColumns; column++)
                {
                    var record = GetValue(row, column);
                    _arr[row][column] = record.Eval();
                }
            }

            return this;
        }

        public Matrix GetDeterminantMatrix(int excludedRow, int excludedColumn)
        {
            var result = new Entity[NbRows - 1][];
            int rowIndex = 0;
            for(int row = 0; row < NbRows; row++)
            {
                if (row == excludedRow)
                {
                    continue;
                }

                var columns = new Entity[NbColumns - 1];
                int columnIndex = 0;
                for(int column = 0; column < NbColumns; column++)
                {
                    if (column == excludedColumn)
                    {
                        continue;
                    }

                    columns[columnIndex] = GetValue(row, column);
                    columnIndex++;
                }

                result[rowIndex] = columns;
                rowIndex++;
            }

            return new Matrix(result);
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

        public static Matrix BuildEmptyMatrix(int nbRow, int nbColumn)
        {
            var result = new Entity[nbRow][];
            for (var row = 0; row < nbRow; row++)
            {
                var rowContent = new Entity[nbColumn];
                for (var column = 0; column < nbColumn; column++)
                {
                    rowContent[column] = Number.Create(0);
                }

                result[row] = rowContent;
            }

            return new Matrix(result);
        }

        public Entity[][] Arr => _arr;
        public int NbRows => Arr.Length;
        public int NbColumns => Arr[0].Length;

        public Matrix Transpose()
        {
            var result = new Entity[NbColumns][];
            for (var column = 0; column < NbColumns; column++)
            {
                var columnVector = GetColumnVector(column);
                result[column] = columnVector;
            }

            return new Matrix(result);
        }

        public Matrix Inverse()
        {
            var clone = (Matrix)Clone();
            var identity = Matrix.BuildIdentityMatrix(NbRows);
            var rowIndex = 0;
            for (int pivotColumn = 0; pivotColumn < NbColumns; pivotColumn++)
            {
                var previousNullRow = FindPreviousNullRow(rowIndex);
                if (previousNullRow != null)
                {
                    clone.SwapLines(previousNullRow.Value, rowIndex);
                    rowIndex = previousNullRow.Value;
                    pivotColumn--;
                    continue;
                }

                var pivot = clone.FindPivotColumn(rowIndex, pivotColumn);
                if (pivot == null)
                {
                    rowIndex++;
                    continue;
                }

                if (pivot.Value.Key != rowIndex)
                {
                    clone.SwapLines(rowIndex, pivot.Value.Key);
                }

                var pivotRowVector = clone.GetRowVector(rowIndex);
                var pivotIdentityRowVector = identity.GetRowVector(rowIndex);
                var div = pivotRowVector[pivotColumn];
                for (int vectorIndex = 0; vectorIndex < pivotRowVector.Length; vectorIndex++)
                {
                    pivotRowVector[vectorIndex] = (pivotRowVector[vectorIndex] / div).Eval();
                    pivotIdentityRowVector[vectorIndex] = (pivotIdentityRowVector[vectorIndex] / div).Eval();
                }

                for (int row = 0; row < clone.NbRows; row++)
                {
                    if (row == rowIndex)
                    {
                        continue;
                    }

                    var rowVector = clone.GetRowVector(row);
                    var mulTimes = rowVector[pivotColumn];
                    for (int col = 0; col < clone.NbColumns; col++)
                    {
                        identity.SetValue(row, col, (identity.GetValue(row, col) - mulTimes * pivotIdentityRowVector[col]).Eval());
                        clone.SetValue(row, col, (clone.GetValue(row, col) - mulTimes * pivotRowVector[col]).Eval());
                    }
                }

                rowIndex++;
            }

            return identity;
        }

        public void Clean()
        {
            for(int row = NbRows - 1; row >= 0; row--)
            {
                var vector = GetRowVector(row);
                if (vector.All(_ =>
                {
                    if (_.IsNumberEntity(out NumberEntity ne) && ne.Number.Value == 0)
                    {
                        return true;
                    }

                    return false;
                }))
                {
                    var tmp = _arr.ToList();
                    tmp.RemoveAt(row);
                    _arr = tmp.ToArray();
                }
                else
                {
                    break;
                }
            }

            for(int column = NbColumns - 1; column >= 0; column--)
            {
                var vector = GetColumnVector(column);
                if (vector.All(_ =>
                {
                    if (_.IsNumberEntity(out NumberEntity ne) && ne.Number.Value == 0)
                    {
                        return true;
                    }

                    return false;
                }))
                {
                    for(int row = 0; row < NbRows; row++)
                    {
                        var tmp = _arr[row].ToList();
                        tmp.RemoveAt(column);
                        _arr[row] = tmp.ToArray();
                    }
                }
                else
                {
                    break;
                }

            }
        }

        public void SwapLines(int fRow, int sRow)
        {
            var fVector = GetRowVector(fRow).ToList();
            var sVector = GetRowVector(sRow).ToList();
            for(int column = 0; column < NbColumns; column++)
            {
                SetValue(fRow, column, sVector[column]);
                SetValue(sRow, column, fVector[column]);
            }
        }

        public Matrix Multiply(Entity[] entities)
        {
            var arr = new Entity[entities.Length][];
            for(int row = 0; row < entities.Length; row++)
            {
                arr[row] = new Entity[] { entities[row] };
            }

            var tmp = new Matrix(arr);
            return Multiply(tmp);
        }

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
                for (var targetColumn = 0; targetColumn < matrix.NbColumns; targetColumn++)
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
        
        public KeyValuePair<int, int>? FindPivotColumn(int rowIndex, int columnIndex)
        {
            for (int row = rowIndex; row < NbRows; row++)
            {
                var val = GetValue(row, columnIndex) as NumberEntity;
                if (val == null || (val != null && val.Number.Value != 0))
                {
                    return new KeyValuePair<int, int>(row, columnIndex);
                }
            }

            return null;
        }

        public int? FindPreviousNullRow(int currentRow)
        {
            for (int row = currentRow - 1; row >= 0; row--)
            {
                var rowVector = GetRowVector(row);
                if (rowVector.All(_ =>
                {
                    if (_.IsNumberEntity(out NumberEntity numEnt))
                    {
                        return numEnt.Number.Value == 0;
                    }

                    return false;
                }))
                {
                    return row;
                }
            }

            return null;
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
