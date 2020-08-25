// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Statistic.Regression;
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
            var advertisingData = Extract(Path.Combine(Directory.GetCurrentDirectory(), "DataSets", "Advertising.csv"));
            var simpleLinearRegression = new SimpleLinearRegression();
            var result = simpleLinearRegression.Regress("TV", "sales", advertisingData);
            Console.WriteLine("Press a key to quit the application");
            Console.ReadKey();
        }

        private static DataTable Extract(string csvFile)
        {            
            var lines = File.ReadAllLines(csvFile);
            var table = new DataTable();
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