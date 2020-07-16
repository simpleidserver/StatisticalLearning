// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Data;
using System.IO;
using System.Linq;

namespace StatisticalLearning.ConsoleClient
{
    class Program
    {
        private const string SEPARATOR = ",";

        static void Main(string[] args)
        {
            BuildMatrix();
            /*
            var advertisingData = Extract(Path.Combine(Directory.GetCurrentDirectory(), "DataSets", "Advertising.csv"));
            var linearRegressionOperation = new LinearRegressionOperation();
            var report = linearRegressionOperation.Execute("sales", "TV", advertisingData);
            Console.WriteLine($"Intercept (B0) estimation : {report.B0}");
            Console.WriteLine($"Slope (B1) estimation : {report.B1}");
            Console.WriteLine($"Residual Sum Of Squares (RSS) : {report.RSS}");
            Console.WriteLine($"Residual Standard Error (RSE) : {report.RSE}");
            Console.WriteLine($"Standard Error of Intercept (B0) : {report.SEB0}");
            Console.WriteLine($"Standard Error of Slope (B1) : {report.SEB1}");
            Console.WriteLine($"t-statistic Intercept (B0) : {report.TSTAT0}");
            Console.WriteLine($"t-statistic Slope (B1) : {report.TSTAT1}");
            Console.WriteLine($"p-value Intercept (B0) : {report.PVAL0}");
            Console.WriteLine($"p-value Slope (B1) : {report.PVAL1}");
            Console.WriteLine($"R square : {report.RSQUARE}");
            Console.WriteLine($"F-Stat : {report.FSTAT}");
            */
            Console.WriteLine("Press any key to quit the application ...");
            Console.ReadKey();
        }

        private static void BuildMatrix()
        {
            double[] inputs = { 80, 60, 10, 20, 30 };
            double[] outputs = { 20, 40, 30, 50, 60 };
            double[][] X = new double[inputs.Length][];
            for (int i = 0; i < inputs.Length; i++)
                X[i] = new double[] { 1.0, inputs[i] };
            int rows = X.Length;
            int cols = X[0].Length;
            var designMatrix = new double[rows, cols];
            for (int i = 0; i < inputs.Length; i++)
                for (int j = 0; j < X[i].Length; j++)
                    designMatrix[i, j] = X[i][j];


            string sss = ";";
        }

        private static DataTable Extract(string csvFile)
        {            
            var lines = File.ReadAllLines(csvFile);
            var table = new DataTable();
            // NOTE : Only quantitative variables are supported.
            foreach(var columnName in lines.First().Split(SEPARATOR))
            {
                table.Columns.Add(new DataColumn(columnName.Trim('"'), typeof(double)));
            }

            for(var i = 1; i < lines.Count(); i++)
            {
                var line = lines[i];
                var tableRow = table.NewRow();
                var columnValues = line.Split(SEPARATOR);
                for (var z = 0; z < columnValues.Count(); z++)
                {
                    tableRow[table.Columns[z].ColumnName] = double.Parse(columnValues.ElementAt(z).Trim('"').Replace(".", ","));
                }

                table.Rows.Add(tableRow);
            }

            return table;
        }
    }
}