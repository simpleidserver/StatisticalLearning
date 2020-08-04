// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Echelons;
using StatisticalLearning.Entities;
using StatisticalLearning.Math;
using StatisticalLearning.Numeric;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalLearning.Decompositions
{
    public class SingularValueDecomposition
    {
        private static string VARIABLE_NAME = "x";


        public SingularValueDecompositionResult Decompose(Matrix matrix)
        {
            var aat = matrix.Multiply(matrix.Transpose()).Solve();
            var ata = matrix.Transpose().Multiply(matrix).Solve();
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
                if (_.EingenValue.IsNumberEntity(out NumberEntity r) && r.Number.Value == 0)
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

            u.Clean();
            sum.Clean();
            v.Clean();
            return new SingularValueDecompositionResult
            {
                U = u,
                S = sum,
                V = v
            };
        }

        private static List<NormalizedVector> DecomposeMatrix(Matrix matrix)
        {
            var result = new List<NormalizedVector>();
            var rowEchelon = new RowEchelon();
            VariableEntity lambda = "λ";
            var aatIdentity = Matrix.BuildIdentityMatrix(matrix.NbRows);
            var aatEquation = matrix - (lambda * aatIdentity);
            var aatDeterminant = aatEquation.ComputeDeterminant().Evaluate(lambda);
            var eingenvalues = aatDeterminant.Solve(lambda);
            foreach (var eingenvalue in eingenvalues.OrderByDescending(_ => _))
            {
                var vectorMatrix = matrix.Substract(Matrix.BuildIdentityMatrix(matrix.NbRows).Multiply(eingenvalue));
                var reducedForm = rowEchelon.BuildReducedRowEchelonForm(vectorMatrix);
                var arr = new Entity[reducedForm.NbRows][];
                for (var variableIndex = 0; variableIndex < reducedForm.NbRows; variableIndex++)
                {
                    arr[variableIndex] = new Entity[] { $"{VARIABLE_NAME}{variableIndex}" };
                }

                var variablesMatrix = new Matrix(arr);
                var matrixEquations = reducedForm.Multiply(variablesMatrix);
                var dic = new Dictionary<VariableEntity, Entity>();
                var vector = new NumberEntity[reducedForm.NbRows];
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

                        vector[variableIndex] = equation.Eval() as NumberEntity;
                    }
                    else if(!rightPart.Equals(default(KeyValuePair<VariableEntity, Entity>)) && rightPart.Key != null)
                    {
                        vector[variableIndex] = (NumberEntity)Number.Create(1);
                    }
                    else
                    {
                        vector[variableIndex] = (NumberEntity)Number.Create(0);
                    }
                }

                var length = (NumberEntity)Number.Create(System.Math.Sqrt(vector.Sum(_ => System.Math.Pow(_.Number.Value, 2))));
                var normalizedVector = new NumberEntity[reducedForm.NbRows];
                for(int i = 0; i < reducedForm.NbRows; i++)
                {
                    normalizedVector[i] = (NumberEntity)(vector[i] / length).Eval();
                }


                result.Add(new NormalizedVector(eingenvalue, normalizedVector));
            }

            return result;
        }

        private static string GetVariableName(int pivotIndex)
        {
            return $"{VARIABLE_NAME}{pivotIndex}";
        }

        private class NormalizedVector
        {
            public NormalizedVector(Entity eingenvalue, NumberEntity[] vector)
            {
                EingenValue = eingenvalue;
                Vector = vector;
            }

            public Entity EingenValue { get; set; }
            public NumberEntity[] Vector { get; set; }
        }
    }
}
