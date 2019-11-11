using StatisticalLearning.ConsoleClient.Extensions;
using System;
using System.Data;

namespace StatisticalLearning.ConsoleClient
{
    public class LinearRegressionOperation
    {
        /// <summary>
        /// Simple linear regression = Y = B0 + B1X + E
        /// B0 = Intercept
        /// B1 = Slope
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="data"></param>
        public LinearRegressionReport Execute(string x, string y, DataTable data)
        {
            var avgX = data.CalculateAvg(x);
            var avgY = data.CalculateAvg(y);
            var b1 = CalculateLeastSquareSlope(data, x, y, avgX, avgY);
            var b0 = CalculateLeastSquareIntercept(b1, avgX, avgY);
            var rss = CalculateResidualSumOfSquares(data, x, y, b0, b1);
            var rse = CalculateResidualStandardError(data, rss);
            var seB0 = CalculateStandardErrorIntercept();
            var seB1 = CalculateStandardErrorSlope(data, x, avgX, rse);
            return new LinearRegressionReport
            {
                B0 = b0,
                B1 = b1,
                RSS = rss,
                RSE = rse,
                SEB1 = seB1
            };
        }

        /// <summary>
        /// Calculate B0(intercept estimation)
        /// </summary>
        private double CalculateLeastSquareIntercept(double b1, double avgX, double avgY)
        {
            return (avgY - (b1 * avgX));
        }

        /// <summary>
        /// Calculate B1(slope estimation)
        /// </summary>
        private double CalculateLeastSquareSlope(DataTable data, string x, string y, double avgX, double avgY)
        {
            double dividend = 0, divisor = 0;
            foreach (DataRow dataRow in data.Rows)
            {
                var xi = dataRow.GetColumnValue(x);
                var yi = dataRow.GetColumnValue(y);
                dividend += (xi - avgX) * (yi - avgY);
                divisor += Math.Pow(xi - avgX, 2);
            }

            return dividend / divisor;
        }

        /// <summary>
        /// Calculate RSS (Residual Sum Of Squares)
        /// </summary>
        /// <returns></returns>
        public double CalculateResidualSumOfSquares(DataTable data, string x, string y, double estimatedIntercept, double estimatedSlope)
        {
            double result = 0;
            foreach (DataRow dataRow in data.Rows)
            {
                var xi = dataRow.GetColumnValue(x);
                var yi = dataRow.GetColumnValue(y);
                result += Math.Pow((yi - estimatedIntercept - (estimatedSlope * xi)), 2);
            }

            return result;
        }

        /// <summary>
        /// Calculate the Residual Standard Error (RSE).
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public double CalculateResidualStandardError(DataTable data, double rss)
        {
            return Math.Sqrt((rss) / data.Rows.Count);
        }

        private double CalculateStandardErrorIntercept()
        {
            return 0;
        }

        /// <summary>
        /// Calculate Standard Error (SE) of slope (B1).
        /// </summary>
        /// <returns></returns>
        private double CalculateStandardErrorSlope(DataTable table, string x, double avgX, double rse)
        {
            double dividend = Math.Pow(rse, 2), divisor = 0;
            foreach (DataRow row in table.Rows)
            {
                var xi = row.GetColumnValue(x);
                var tmp = xi - avgX;
                divisor += Math.Pow((xi - avgX), 2);
            }

            return Math.Sqrt((dividend / divisor));
        }
    }
}
