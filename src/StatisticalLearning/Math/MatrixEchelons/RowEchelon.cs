// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using StatisticalLearning.Math.Entities;

namespace StatisticalLearning.Math
{
    public static class RowEchelonExtensions
    {
        /// <summary>
        /// https://en.wikipedia.org/wiki/Row_echelon_form
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix ReducedRowEchelonForm(this Matrix matrix)
        {
            var result = (Matrix)matrix.Clone();
            if (result.NbColumns == 1 && result.NbRows == 1)
            {
                return result;
            }

            var rowIndex = 0;
            for(int pivotColumn = 0; pivotColumn < result.NbColumns; pivotColumn++)
            {
                var previousNullRow = result.FindPreviousNullRow(rowIndex);
                if (previousNullRow != null)
                {
                    result.SwapLines(previousNullRow.Value, rowIndex);
                    rowIndex = previousNullRow.Value;
                    pivotColumn--;
                    continue;
                }

                var pivot = result.FindPivotColumn(rowIndex, pivotColumn);
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
                            if (System.Math.Round(r.Number.Value, 4) == 0 || System.Math.Round(-r.Number.Value, 4) == 0)
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
    }
}
