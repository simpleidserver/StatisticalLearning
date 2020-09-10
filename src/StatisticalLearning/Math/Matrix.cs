// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace StatisticalLearning.Math
{
    public class Matrix : IEquatable<Matrix>, ICloneable
    {
        private readonly Entity[][] _values;
        public static implicit operator Matrix(Vector vector)
        {
            return new Matrix(vector);
        }
        public static implicit operator Matrix(string[][] values)
        {
            var arr = new Entity[values.Length][];
            for (int row = 0; row < values.Length; row++)
            {
                arr[row] = new Entity[values[0].Length];
                for (int column = 0; column < values[0].Length; column++)
                {
                    arr[row][column] = values[row][column];
                }
            }

            return new Matrix(arr);
        }
        public static implicit operator Matrix(double[][] values)
        {
            var arr = new Entity[values.Length][];
            for(int row = 0; row < values.Length; row++)
            {
                arr[row] = new Entity[values[0].Length];
                for(int column = 0; column < values[0].Length; column++)
                {
                    arr[row][column] = values[row][column];
                }
            }

            return new Matrix(arr);
        }
        public static implicit operator Matrix(Entity[][] values)
        {
            return new Matrix(values);
        }
        public static implicit operator Matrix(Vector[] rows)
        {
            return new Matrix(rows);
        }
        public static Matrix operator *(Matrix a, Matrix b) => a.Multiply(b);
        public static Matrix operator -(Matrix a, Matrix b) => a.Substract(b);
        public static Matrix operator *(Entity a, Matrix b) => b.Multiply(a);
        public static Matrix operator *(Matrix a, Entity b) => a.Multiply(b);

        public Matrix(Entity[][] values)
        {
            _values = values;
        }

        public Matrix(Vector vector)
        {
            Entity[][] values;
            if (vector.Direction == VectorDirections.HORIZONTAL)
            {
                values = new Entity[1][];
                values[0] = new Entity[vector.Length];
                for (int i = 0; i < vector.Length; i++)
                {
                    values[0][i] = vector[i];
                }
            }
            else
            {
                values = new Entity[vector.Length][];
                for(int i = 0; i < vector.Length; i++)
                {
                    values[i] = new Entity[] { vector[i] };
                }
            }

            _values = values;
        }

        public Matrix(Vector[] rows)
        {
            _values = rows.Select(_ => _.Values).ToArray();
        }

        public Entity[][] Values => _values;
        public double[][] DoubleValues
        {
            get
            {
                var values = new double[NbRows][];
                for(int row = 0; row < NbRows; row++)
                {
                    values[row] = new double[NbColumns];
                    for(int column = 0; column < NbColumns; column++)
                    {
                        values[row][column] = _values[row][column].GetNumber();
                    }
                }

                return values;
            }
        }
        public int NbRows => Values.Length;
        public int NbColumns => Values[0].Length;

        /// <summary>
        /// Evaluate each entity of the matrix.
        /// </summary>
        /// <returns></returns>
        public Matrix Evaluate()
        {
            for (int row = 0; row < NbRows; row++)
            {
                for (int column = 0; column < NbColumns; column++)
                {
                    var record = GetValue(row, column);
                    _values[row][column] = record.Eval();
                }
            }

            return this;
        }

        #region Get

        /// <summary>
        /// Get diagonal.
        /// </summary>
        /// <returns></returns>
        public Entity[] GetDiagonal()
        {
            var min = System.Math.Min(NbRows, NbColumns);
            var result = new Entity[min];
            for (int i = 0; i < min; i++)
            {
                result[i] = GetValue(i, i);
            }

            return result;
        }

        public Entity GetValue(int row, int column)
        {
            return _values[row][column];
        }

        public Vector GetColumnVector(int column)
        {
            var values = new Entity[NbRows];
            for (var i = 0; i < NbRows; i++)
            {
                values[i] = _values[i][column];
            }

            return new Vector(values);
        }

        public Vector GetRowVector(int row)
        {
            return _values[row];
        }

        public Matrix GetRows(int[] rows)
        {
            var arr = new double[rows.Count()][];
            int i = 0;
            foreach(var row in rows)
            {
                arr[i] = GetRowVector(row).GetNumbers();
                i++;
            }

            return arr;
        }

        public Matrix GetSubMatrix(int startRow, int startColumn, int? nbColumns = null)
        {
            int m = NbRows - startRow;
            int n = NbColumns - startColumn;
            if (nbColumns != null)
            {
                n = nbColumns.Value ;
            }

            var arr = new Entity[m][];
            for (int r = 0; r < m; r++)
            {
                arr[r] = new Entity[n];
                for (int c = 0; c < n; c++)
                {
                    arr[r][c] = GetValue(r + startRow, c + startColumn);
                }
            }

            return new Matrix(arr);
        }

        /// <summary>
        /// Get index of the pivot.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public int? GetPivotColumnIndex(int row)
        {
            var rowVector = GetRowVector(row);
            for (int i = 0; i < rowVector.Length; i++)
            {
                if (rowVector[i].IsOne())
                {
                    return i;
                }
            }

            return null;
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
                    return _.IsZero();
                }))
                {
                    return row;
                }
            }

            return null;
        }

        #endregion

        #region Set

        public void SetValue(int row, int column, Entity entity)
        {
            _values[row][column] = entity;
        }

        /// <summary>
        /// Swap two lines.
        /// </summary>
        /// <param name="fRow"></param>
        /// <param name="sRow"></param>
        public void SwapLines(int fRow, int sRow)
        {
            var fVector = GetRowVector(fRow);
            var sVector = GetRowVector(sRow);
            for (int column = 0; column < NbColumns; column++)
            {
                SetValue(fRow, column, sVector[column]);
                SetValue(sRow, column, fVector[column]);
            }
        }

        /// <summary>
        /// Add a new column in the first index.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Matrix AddColumn(Entity entity)
        {
            var result = new Entity[NbRows][];
            for (int i = 0; i < NbRows; i++)
            {
                var inputRow = GetRowVector(i);
                var newRow = new Entity[1 + inputRow.Length];
                newRow[0] = entity;
                for (int column = 0; column < inputRow.Length; column++)
                {
                    newRow[1 + column] = inputRow[column];
                }

                result[i] = newRow;
            }

            return new Matrix(result);
        }

        public void Replace(Matrix newMatrix, int row, int column)
        {
            for (int r = row; r < NbRows; r++)
            {
                for (int c = column; c < NbColumns; c++)
                {
                    SetValue(r, c, newMatrix.GetValue(r - row, c - column));
                }
            }
        }

        public void SetRow(Vector vector, int row)
        {
            for(int column = 0; column < NbColumns; column++)
            {
                SetValue(row, column, vector.Values[column]);
            }
        }

        public void SetColumn(Vector vector, int column, int startRow = 0)
        {
            for(int row = startRow; row < NbRows; row++)
            {
                SetValue(row, column, vector.Values[row - startRow]);
            }
        }

        #endregion

        #region Transform matrix

        /// <summary>
        /// Transpose a matrix.
        /// </summary>
        /// <returns></returns>
        public Matrix Transpose()
        {
            var result = new Entity[NbColumns][];
            for (var column = 0; column < NbColumns; column++)
            {
                var columnVector = GetColumnVector(column);
                result[column] = columnVector.Values;
            }

            return new Matrix(result);
        }

        /// <summary>
        /// Inverse the matrix.
        /// </summary>
        /// <returns></returns>
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

        #endregion

        #region Calculation on matrix

        /// <summary>
        /// Calculate house holder matrix
        /// http://www.klubprepa.fr/Site/Document/ChargementDocument.aspx?IdDocument=3658
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public Matrix HouseHolderMatrix(int column)
        {
            // 
            var identity = Matrix.BuildIdentityMatrix(NbColumns);
            var vector = GetColumnVector(column);
            var tmp = (2 / MathEntity.Pow(vector.Norm(), 2)).Eval();
            var matrixHolder = identity.Substract(Matrix.Multiply(vector, vector).Multiply(tmp)).Evaluate();
            return matrixHolder;
        }

        public Vector Avg()
        {
            var result = new Vector(NbColumns);
            for(int i = 0; i < NbColumns; i++)
            {
                result[i] = GetColumnVector(i).Avg();
            }

            return result;
        }

        public Vector Variance(bool isSample = false)
        {
            var result = new Vector(NbColumns);
            for (int i = 0; i < NbColumns; i++)
            {
                result[i] = GetColumnVector(i).Variance(isSample);
            }

            return result;
        }

        public Vector SumOfSquare()
        {
            var result = new Vector(NbColumns);
            for (int i = 0; i < NbColumns; i++)
            {
                result[i] = GetColumnVector(i).SumOfSquares();
            }

            return result;
        }

        /// <summary>
        /// Calculate centered mean matrix.
        /// http://users.stat.umn.edu/~helwig/notes/datamat-Notes.pdf.
        /// </summary>
        /// <returns></returns>
        public Matrix CenteredMeanMatrix()
        {
            var arr = new Entity[NbRows][];
            for (int column = 0; column < NbColumns; column++)
            {
                var mean = GetColumnVector(column).Avg();
                for (int row = 0; row < NbRows; row++)
                {
                    var value = arr[row];
                    if (value == null)
                    {
                        arr[row] = new Entity[NbColumns];
                    }

                    arr[row][column] = GetValue(row, column) - mean;
                }
            }

            return new Matrix(arr);
        }

        /// <summary>
        /// Compute centered mean reduced matrix.
        /// </summary>
        /// <param name="unbiased"></param>
        /// <returns></returns>
        public Matrix CenteredMeanReducedMatrix(bool unbiased = true)
        {
            var result = CenteredMeanMatrix();
            for (int colum = 0; colum < NbColumns; colum++)
            {
                var columnStandardDeviation = GetColumnVector(colum).StandardDeviation(unbiased);
                for (int row = 0; row < NbRows; row++)
                {
                    var value = result.GetValue(row, colum);
                    result.SetValue(row, colum, (value / columnStandardDeviation));
                }
            }

            return result;
        }

        /// <summary>
        /// Calculate centered normative matrix.
        /// </summary>
        /// <param name="unbiased"></param>
        /// <returns></returns>
        public Matrix CenteredNormativeMatrix(bool unbiased = true)
        {
            var result = CenteredMeanReducedMatrix(unbiased).Multiply(Number.Create(1) / MathEntity.Sqrt(Number.Create(NbRows)));
            return result;
        }

        /// <summary>
        /// Calculate covariance.
        /// </summary>
        /// <returns></returns>
        public Matrix Covariance()
        {
            var result = new Entity[NbColumns][];
            for (int columnX = 0; columnX < NbColumns; columnX++)
            {
                result[columnX] = new Entity[NbColumns];
                var xVector = GetColumnVector(columnX);
                var meanX = xVector.Avg();
                for (int columnY = 0; columnY < NbColumns; columnY++)
                {
                    var yVector = GetColumnVector(columnY);
                    var meanY = yVector.Avg();
                    Entity sum = 0;
                    for (int row = 0; row < NbRows; row++)
                    {
                        var xValue = xVector[row];
                        var yValue = yVector[row];
                        sum += (xValue - meanX) * (yValue - meanY);
                    }

                    var covariance = sum / (NbRows);
                    result[columnX][columnY] = covariance;
                }
            }

            return new Matrix(result);
        }

        /// <summary>
        /// Calculate scatter matrix.
        /// </summary>
        /// <returns></returns>
        public Matrix Scatter(Vector avgVector)
        {
            var scatter = Matrix.BuildEmptyMatrix(NbColumns, NbColumns);
            for(int i = 0; i < NbRows; i++)
            {
                var row = GetRowVector(i);
                scatter = scatter.Sum(Matrix.Multiply(row.Substract(avgVector), row.Substract(avgVector)));
            }

            return scatter;
        }

        /// <summary>
        /// Calculate correlation.
        /// </summary>
        /// <param name="unbiased"></param>
        /// <returns></returns>
        public Matrix Correlation(bool unbiased = true)
        {
            var result = Covariance();
            for (int row = 0; row < result.NbRows; row++)
            {
                var standardDeviationX = GetColumnVector(row).StandardDeviation(unbiased);
                for (int column = 0; column < result.NbColumns; column++)
                {
                    var standardDeviationY = GetColumnVector(column).StandardDeviation(unbiased);
                    var value = result.GetValue(row, column) / (standardDeviationX * standardDeviationY);
                    result.SetValue(row, column, value);
                }
            }

            return result;
        }

        /// <summary>
        /// Calculate the determinant.
        /// </summary>
        /// <returns></returns>
        public Entity Determinant()
        {
            if (NbRows == 2)
            {
                return GetValue(0, 0) * GetValue(1, 1) - GetValue(0, 1) * GetValue(1, 0);
            }
            else if (NbRows > 2)
            {
                int nbDeterminant = 1;
                Entity result = null;
                for (int column = 0; column < NbColumns; column++)
                {
                    var determinant = GetValue(0, column) * GetDeterminantMatrix(0, column).Determinant();
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

            return GetValue(0, 0);
        }

        /// <summary>
        /// Sum all rows.
        /// </summary>
        /// <returns></returns>
        public Vector SumAllRows()
        {
            var result = new Entity[NbRows];
            for (int row = 0; row < NbRows; row++)
            {
                Entity record = 0.0;
                for (int column = 0; column < NbColumns; column++)
                {
                    record = (record + GetValue(row, column)).Eval();
                }

                result[row] = record;
            }

            return result;
        }

        #endregion

        #region Operations on matrix

        /// <summary>
        /// Multiply a matrix by an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Matrix Multiply(Entity entity)
        {
            return ExecuteOperation(entity, Constants.Operators.MUL);
        }

        /// <summary>
        /// Multiply a matrix by a vector.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Matrix Multiply(Vector vector)
        {
            var result = new Entity[NbRows][];
            for (int row = 0; row < NbRows; row++)
            {
                var record = new Entity[NbColumns];
                for (int column = 0; column < NbColumns; column++)
                {
                    if (vector.Direction == VectorDirections.HORIZONTAL)
                    {
                        record[column] = vector[column] * GetValue(row, column);
                    }
                    else
                    {
                        record[column] = vector[row] * GetValue(row, column);
                    }
                }

                result[row] = record;
            }

            return new Matrix(result);
        }

        /// <summary>
        /// Multiply two matrix.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public Matrix Multiply(Matrix matrix)
        {
            return ExecuteOperation(matrix, Constants.Operators.MUL);
        }

        /// <summary>
        /// Divide a matrix by an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Matrix Div(Entity entity)
        {
            return ExecuteOperation(entity, Constants.Operators.DIV);
        }

        /// <summary>
        /// Divide the matrix by a vector.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Matrix Div(Vector vector)
        {
            var result = new Entity[NbRows][];
            for (var row = 0; row < NbRows; row++)
            {
                var values = new Entity[NbColumns];
                for (var column = 0; column < NbColumns; column++)
                {
                    values[column] = GetValue(row, column) / vector[column];
                }

                result[row] = values;
            }

            return new Matrix(result).Evaluate();
        }

        /// <summary>
        /// Divide two matrix.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public Matrix Div(Matrix matrix)
        {
            return ExecuteOperation(matrix, Constants.Operators.DIV);
        }

        /// <summary>
        /// Substract the matrix with an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Matrix Substract(Entity entity)
        {
            return ExecuteOperation(entity, Constants.Operators.SUB);
        }

        /// <summary>
        /// Substract the matrix with a vector.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Matrix Substract(Vector vector)
        {
            if (vector.Length == 1)
            {
                return Substract(vector.Values[0]);
            }

            var result = new Entity[NbRows][];
            for (int row = 0; row < NbRows; row++)
            {
                var record = new Entity[NbColumns];
                for (int column = 0; column < NbColumns; column++)
                {
                    if (vector.Direction == VectorDirections.HORIZONTAL)
                    {
                        record[column] = GetValue(row, column) - vector[column];
                    }
                    else
                    {
                        record[column] = GetValue(row, column) - vector[row];
                    }
                }

                result[row] = record;
            }

            return new Matrix(result).Evaluate();
        }

        /// <summary>
        /// Substract two matrix.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public Matrix Substract(Matrix matrix)
        {
            return ExecuteOperation(matrix, Constants.Operators.SUB);
        }

        /// <summary>
        /// Sum the matrix with an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Matrix Sum(Entity entity)
        {
            return ExecuteOperation(entity, Constants.Operators.SUM);
        }

        /// <summary>
        /// Sum two matrix.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public Matrix Sum(Matrix matrix)
        {
            return ExecuteOperation(matrix, Constants.Operators.SUM);
        }

        /// <summary>
        /// Calculate pow.
        /// </summary>
        /// <param name="nb"></param>
        /// <returns></returns>
        public Matrix Pow(int nb)
        {
            var result = new Entity[NbRows][];
            for (var row = 0; row < NbRows; row++)
            {
                var values = new Entity[NbColumns];
                for (var column = 0; column < NbColumns; column++)
                {
                    values[column] = MathEntity.Pow(GetValue(row, column), nb);
                }

                result[row] = values;
            }

            return new Matrix(result).Evaluate();
        }

        /// <summary>
        /// Calculate exponential.
        /// </summary>
        /// <returns></returns>
        public Matrix Exp()
        {
            var result = new Entity[NbRows][];
            for (var row = 0; row < NbRows; row++)
            {
                var values = new Entity[NbColumns];
                for (var column = 0; column < NbColumns; column++)
                {
                    values[column] = MathEntity.Exp(GetValue(row, column));
                }

                result[row] = values;
            }

            return new Matrix(result).Evaluate();
        }

        /// <summary>
        /// Calculate log(sum(exp(matrix)))
        /// </summary>
        /// <returns></returns>
        public Vector Logsumexp()
        {
            return Exp().SumAllRows().Log();
        }

        #endregion

        public bool Equals(Matrix x)
        {
            if (x == null)
            {
                return false;
            }

            return x.GetHashCode() == GetHashCode();
        }

        private Matrix ExecuteOperation(Matrix matrix, string op)
        {
            if (matrix.NbRows == 1 && matrix.NbColumns == 1)
            {
                return ExecuteOperation(matrix.GetValue(0, 0), op);
            }

            var result = new Entity[NbRows][];
            if (op == Constants.Operators.SUB || op == Constants.Operators.SUM)
            {
                for (var row = 0; row < NbRows; row++)
                {
                    var values = new Entity[NbColumns];
                    for (var column = 0; column < NbColumns; column++)
                    {
                        switch(op)
                        {
                            case Constants.Operators.SUM:
                                values[column] = GetValue(row, column) + matrix.GetValue(row, column);
                                break;
                            case Constants.Operators.SUB:
                                values[column] = GetValue(row, column) - matrix.GetValue(row, column);
                                break;
                        }
                    }

                    result[row] = values;
                }

                return new Matrix(result);
            }

            for (var row = 0; row < NbRows; row++)
            {
                var values = new Entity[matrix.NbColumns];
                for (var targetColumn = 0; targetColumn < matrix.NbColumns; targetColumn++)
                {
                    var targetColumnVector = matrix.GetColumnVector(targetColumn);
                    Entity value = null;
                    for (var column = 0; column < NbColumns; column++)
                    {
                        var source = GetValue(row, column);
                        Entity target;
                        if (column >= targetColumnVector.Length)
                        {
                            target = Number.Create(0);
                        }
                        else
                        {
                            target = targetColumnVector[column];
                        }

                        Entity tmp = null;
                        switch(op)
                        {
                            case Constants.Operators.DIV:
                                tmp = source / target;
                                break;
                            case Constants.Operators.MUL:
                                tmp = source * target;
                                break;
                        }

                        if (value == null)
                        {
                            value = tmp;
                        }
                        else
                        {
                            value += tmp;
                        }
                    }

                    values[targetColumn] = value;
                }

                result[row] = values;
            }

            return new Matrix(result);
        }

        private Matrix ExecuteOperation(Entity entity, string op)
        {
            var result = new Entity[NbRows][];
            for (var row = 0; row < NbRows; row++)
            {
                var values = new Entity[NbColumns];
                for (var column = 0; column < NbColumns; column++)
                {
                    switch(op)
                    {
                        case Constants.Operators.MUL:
                            values[column] = GetValue(row, column) * entity;
                            break;
                        case Constants.Operators.DIV:
                            values[column] = GetValue(row, column) / entity;
                            break;
                        case Constants.Operators.SUB:
                            values[column] = GetValue(row, column) - entity;
                            break;
                        case Constants.Operators.SUM:
                            values[column] = GetValue(row, column) + entity;
                            break;
                    }
                }

                result[row] = values;
            }

            return new Matrix(result);
        }

        private Matrix GetDeterminantMatrix(int excludedRow, int excludedColumn)
        {
            var result = new Entity[NbRows - 1][];
            int rowIndex = 0;
            for (int row = 0; row < NbRows; row++)
            {
                if (row == excludedRow)
                {
                    continue;
                }

                var columns = new Entity[NbColumns - 1];
                int columnIndex = 0;
                for (int column = 0; column < NbColumns; column++)
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
                lst.Add($"[ {string.Join(",", vector.Values.Select(_ => _.ToString())) } ]");
            }

            return $"[ {string.Join(",", lst.ToArray())} ]";
        }

        #region Static methods

        /// <summary>
        /// Multiply two vectors to get a matrix.
        /// </summary>
        /// <param name="columnVector"></param>
        /// <param name="rowVector"></param>
        /// <returns></returns>
        public static Matrix Multiply(Vector columnVector, Vector rowVector)
        {
            var arr = new Entity[columnVector.Length][];
            for (int row = 0; row < columnVector.Length; row++)
            {
                arr[row] = new Entity[rowVector.Length];
                for (int column = 0; column < rowVector.Length; column++)
                {
                    arr[row][column] = columnVector[row] * rowVector[column];
                }
            }

            return new Matrix(arr);
        }

        /// <summary>
        /// Build an identity matrix.
        /// </summary>
        /// <param name="nbRowColumn"></param>
        /// <returns></returns>
        public static Matrix BuildIdentityMatrix(int nbRowColumn)
        {
            var result = new Entity[nbRowColumn][];
            for (var row = 0; row < nbRowColumn; row++)
            {
                var rowContent = new Entity[nbRowColumn];
                for (var column = 0; column < nbRowColumn; column++)
                {
                    if (column == row)
                    {
                        rowContent[column] = 1;
                    }
                    else
                    {
                        rowContent[column] = 0;
                    }
                }

                result[row] = rowContent;
            }

            return new Matrix(result);
        }

        /// <summary>
        /// Build an identity matrix.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Matrix BuildIdentityMatrix(Vector vector)
        {
            return BuildIdentityMatrix(vector.Values);
        }

        /// <summary>
        /// Build an identity matrix.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Matrix BuildIdentityMatrix(Entity[] values)
        {
            var arr = new Entity[values.Length][];
            for (int i = 0; i < values.Length; i++)
            {
                var row = new Entity[values.Length];
                for (int j = 0; j < values.Length; j++)
                {
                    if (i == j)
                    {
                        row[j] = values[i];
                    }
                    else
                    {
                        row[j] = 0.0;
                    }
                }

                arr[i] = row;
            }

            return new Matrix(arr);
        }

        /// <summary>
        /// Build an empty matrix.
        /// </summary>
        /// <param name="nbRow"></param>
        /// <param name="nbColumn"></param>
        /// <returns></returns>
        public static Matrix BuildEmptyMatrix(int nbRow, int nbColumn)
        {
            var result = new Entity[nbRow][];
            for (var row = 0; row < nbRow; row++)
            {
                var rowContent = new Entity[nbColumn];
                for (var column = 0; column < nbColumn; column++)
                {
                    rowContent[column] = 0;
                }

                result[row] = rowContent;
            }

            return new Matrix(result);
        }

        #endregion

        public object Clone()
        {
            return new Matrix(_values.Select(_ => _.ToArray()).ToArray());
        }
    }
}
