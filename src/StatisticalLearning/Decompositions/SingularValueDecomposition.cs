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
        public void Decompose(Matrix matrix)
        {
            // var aat = matrix.Multiply(matrix.Transpose()).Solve();
            var ata = matrix.Transpose().Multiply(matrix).Solve();
            // DecomposeMatrix(aat);
            DecomposeMatrix(ata);
        }

        private void DecomposeMatrix(Matrix matrix)
        {
            var rowEchelon = new RowEchelon();
            VariableEntity lambda = "λ";
            var aatIdentity = Matrix.BuildIdentityMatrix(matrix.NbRows);
            var aatEquation = matrix - (lambda * aatIdentity);
            var aatDeterminant = aatEquation.ComputeDeterminant().Evaluate(lambda);
            var eingenvalues = aatDeterminant.Solve(lambda);
            foreach (var eingenvalue in eingenvalues)
            {
                var vectorMatrix = matrix.Substract(Matrix.BuildIdentityMatrix(matrix.NbRows).Multiply(eingenvalue));
                // TODO : IL FAUT RESOUDRE LA FORME REDUITE !!! => QUAND DELTA = 0
                var reducedForm = rowEchelon.BuildReducedRowEchelonForm(vectorMatrix);
                var arr = new Entity[reducedForm.NbRows][];
                for (var variableIndex = 0; variableIndex < reducedForm.NbRows; variableIndex++)
                {
                    arr[variableIndex] = new Entity[] { $"{VARIABLE_NAME}{variableIndex}" };
                }

                var variablesMatrix = new Matrix(arr);
                var matrixEquations = reducedForm.Multiply(variablesMatrix);
                var dic = new Dictionary<VariableEntity, Entity>();
                var vector = new Entity[reducedForm.NbRows][];
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
                            equation.Assign(GetVariableName(index), vector[index][0] as NumberEntity);
                        }

                        var result = equation.Eval();
                        vector[variableIndex] = new Entity[] { result };
                    }
                    else if(!rightPart.Equals(default(KeyValuePair<VariableEntity, Entity>)) && rightPart.Key != null)
                    {
                        vector[variableIndex] = new Entity[] { Number.Create(1) };
                    }
                    else
                    {
                        vector[variableIndex] = new Entity[] { Number.Create(0) };
                    }
                }

                string sss = "";
            }
        }

        private static string GetVariableName(int pivotIndex)
        {
            return $"{VARIABLE_NAME}{pivotIndex}";
        }
    }
}
