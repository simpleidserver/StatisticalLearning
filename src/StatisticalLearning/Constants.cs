// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using System.Collections.Generic;

namespace StatisticalLearning
{
    public static class Constants
    {
        public static class Operators
        {
            public const string DIV = "div";
            public const string MUL = "mul";
            public const string SUM = "sum";
            public const string SUB = "sub";
        }

        public static class Funcs
        {
            public const string SQUAREROOT = "sqrt";
            public const string POW = "pow";
            public const string ACOS = "acos";
            public const string COS = "cos";
            public const string ABS = "abs";
            public const string EXP = "exp";
        }

        public static Dictionary<string, string> MappingOperatorToSign = new Dictionary<string, string>
        {
            { Operators.DIV, "/" },
            { Operators.MUL, "*" },
            { Operators.SUM, "+" },
            { Operators.SUB, "-" }
        };

        public static Dictionary<string, string> MappingFuncToSign = new Dictionary<string, string>
        {
            { Funcs.SQUAREROOT, "√" },
            { Funcs.ACOS, "acos" },
            { Funcs.POW, "^" }
        };
    }
}
