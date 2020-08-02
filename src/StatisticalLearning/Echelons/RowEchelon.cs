// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Entities;
using StatisticalLearning.Math;
using StatisticalLearning.Numeric;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalLearning.Echelons
{
    public class RowEchelon
    {
        public Matrix BuildReducedRowEchelonForm(Matrix matrix)
        {
            var result = (Matrix)matrix.Clone();
            var rowIndex = 0;
            for(int pivotColumn = 0; pivotColumn < result.NbColumns; pivotColumn++)
            {
                var previousNullRow = FindPreviousNullRow(result, rowIndex);
                if (previousNullRow != null)
                {
                    result.SwapLines(previousNullRow.Value, rowIndex);
                    rowIndex = previousNullRow.Value;
                    pivotColumn--;
                    continue;
                }

                var pivot = FindPivotColumn(result, rowIndex, pivotColumn);
                if(pivot == null)
                {
                    rowIndex++;
                    continue;
                }

                if (pivot.Value.Key != rowIndex)
                {
                    result.SwapLines(rowIndex, pivot.Value.Key);
                }

                var pivotRowVector = result.GetRowVector(rowIndex);
                var div = pivotRowVector[pivotColumn];
                for (int vectorIndex = 0; vectorIndex < pivotRowVector.Length; vectorIndex++)
                {
                    pivotRowVector[vectorIndex] = (pivotRowVector[vectorIndex] / div).Eval();
                }

                for (int row = 0; row < result.NbRows; row++)
                {
                    if (row == rowIndex)
                    {
                        continue;
                    }

                    var rowVector = result.GetRowVector(row);
                    var mulTimes = rowVector[pivotColumn];
                    for(int col = 0; col < result.NbColumns; col++)
                    {
                        Entity value = (result.GetValue(row, col) - mulTimes * pivotRowVector[col]).Eval();
                        if (value.IsNumberEntity(out NumberEntity r))
                        {
                            if (System.Math.Floor(r.Number.Value) == 0)
                            {
                                value = Number.Create(0);
                            }
                        }

                        result.SetValue(row, col, value);
                    }
                }

                rowIndex++;
            }

            return result;
        }

        private static KeyValuePair<int, int>? FindPivotColumn(Matrix matrix, int rowIndex, int columnIndex)
        {
            for(int row = rowIndex; row < matrix.NbRows; row++)
            {
                var val = matrix.GetValue(row, columnIndex) as NumberEntity;
                if (val == null || (val != null && val.Number.Value != 0))
                {
                    return new KeyValuePair<int, int>(row, columnIndex);
                }
            }

            return null;
        }

        private static int? FindPreviousNullRow(Matrix matrix, int currentRow)
        {
            for (int row = currentRow - 1; row >= 0; row--)
            {
                var rowVector = matrix.GetRowVector(row);
                if (rowVector.All(_ =>
                {
                    if (_.IsNumberEntity(out NumberEntity numEnt))
                    {
                        return numEnt.Number.Value == 0;
                    }

                    return false;
                })) {
                    return row;
                }
            }

            return null;
        }
    }
}
