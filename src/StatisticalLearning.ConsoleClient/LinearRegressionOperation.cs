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
            var ssX = CalculateSumOfSquaresX(data, avgX, x);
            var ssY = CalculateSumOfSquaresY(data, avgY, y);
            var b1 = CalculateLeastSquareSlope(data, ssX, x, y, avgX, avgY);
            var b0 = CalculateLeastSquareIntercept(b1, avgX, avgY);
            var rss = CalculateResidualSumOfSquares(data, x, y, b0, b1);
            var rse = CalculateResidualStandardError(data, rss);
            var seB0 = CalculateStandardErrorIntercept(data, ssX, avgX, rse);
            var seB1 = CalculateStandardErrorSlope(ssX, rse);
            var tstat0 = CalculateTStatistic(b0, 0, seB0);
            var tstat1 = CalculateTStatistic(b1, 0, seB1);
            return new LinearRegressionReport
            {
                B0 = b0,
                B1 = b1,
                RSS = rss,
                RSE = rse,
                SEB0 = seB0,
                SEB1 = seB1,
                TSTAT0 = tstat0,
                TSTAT1 = tstat1
            };
        }

        /// <summary>
        /// Estimate B0 (intercept)
        /// </summary>
        private double CalculateLeastSquareIntercept(double b1, double avgX, double avgY)
        {
            return (avgY - (b1 * avgX));
        }

        /// <summary>
        /// Estimate B1 (slope)
        /// </summary>
        private double CalculateLeastSquareSlope(DataTable data, double ssX, string x, string y, double avgX, double avgY)
        {
            double dividend = 0;
            foreach (DataRow dataRow in data.Rows)
            {
                var xi = dataRow.GetColumnValue(x);
                var yi = dataRow.GetColumnValue(y);
                dividend += (xi - avgX) * (yi - avgY);
            }

            return dividend / ssX;
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

        /// <summary>
        /// Calculate Standard Error (SE) of intercept (B0).
        /// </summary>
        /// <returns></returns>
        private double CalculateStandardErrorIntercept(DataTable data, double ssX, double avgX, double rse)
        {
            var divResult = ((Math.Pow(avgX, 2) * data.Rows.Count) + ssX) / (data.Rows.Count * ssX);
            return Math.Sqrt(Math.Pow(rse, 2) * divResult);
        }

        /// <summary>
        /// Calculate Standard Error (SE) of slope (B1).
        /// </summary>
        /// <returns></returns>
        private double CalculateStandardErrorSlope(double ssX, double rse)
        {
            double dividend = Math.Pow(rse, 2);
            return Math.Sqrt((dividend / ssX));
        }

        /// <summary>
        /// Compute t-statistic.
        /// </summary>
        /// <returns></returns>
        private double CalculateTStatistic(double estimatedCoefficient, double referenceValue, double se)
        {
            return (estimatedCoefficient - referenceValue) / (se);
        }

        private double CalculateSumOfSquaresX(DataTable table, double avgX, string x)
        {
            double result = 0;
            foreach (DataRow row in table.Rows)
            {
                var xi = row.GetColumnValue(x);
                result += Math.Pow((xi - avgX), 2);
            }

            return result;
        }

        private double CalculateSumOfSquaresY(DataTable table, double avgY, string y)
        {
            double result = 0;
            foreach (DataRow row in table.Rows)
            {
                var yi = row.GetColumnValue(y);
                result += Math.Pow((yi - avgY), 2);
            }

            return result;
        }
    }
}
