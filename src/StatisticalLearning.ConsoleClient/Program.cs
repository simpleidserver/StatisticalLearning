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
        static void Main(string[] args)
        {
            var advertisingData = Extract(Path.Combine(Directory.GetCurrentDirectory(), "DataSets", "Advertising.csv"));
            var linearRegressionOperation = new LinearRegressionOperation();
            var report = linearRegressionOperation.Execute("TV", "Sales", advertisingData);
            Console.WriteLine($"Intercept (B0) estimation : {report.B0}");
            Console.WriteLine($"Slope (B1) estimation : {report.B1}");
            Console.WriteLine($"Residual Sum Of Squares (RSS) : {report.RSS}");
            Console.WriteLine($"Residual Standard Error (RSE) : {report.RSE}");
            Console.WriteLine($"Standard Error of Intercept (B0) : {report.SEB0}");
            Console.WriteLine($"Standard Error of Slope (B1) : {report.SEB1}");
            Console.WriteLine($"t-statistic Intercept (B0) : {report.TSTAT0}");
            Console.WriteLine($"t-statistic Slope (B1) : {report.TSTAT1}");
            Console.WriteLine("Press any key to quit the application ...");
            Console.ReadKey();
        }

        private static DataTable Extract(string csvFile)
        {            
            var lines = File.ReadAllLines(csvFile);
            var table = new DataTable();
            // NOTE : Only quantitative variables are supported.
            foreach(var columnName in lines.First().Split(","))
            {
                table.Columns.Add(new DataColumn(columnName.Trim('"'), typeof(double)));
            }

            for(var i = 1; i < lines.Count(); i++)
            {
                var line = lines[i];
                var tableRow = table.NewRow();
                var columnValues = line.Split(",");
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