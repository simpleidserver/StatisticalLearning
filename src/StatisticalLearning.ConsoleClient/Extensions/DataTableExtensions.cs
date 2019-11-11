using System.Data;

namespace StatisticalLearning.ConsoleClient.Extensions
{
    public static class DataTableExtensions
    {
        public static double CalculateAvg(this DataTable data, string name)
        {
            var columnIndex = data.Columns[name].Ordinal;
            double totalSum = 0;
            foreach (DataRow row in data.Rows)
            {
                var value = row.GetColumnValue(name);
                totalSum += value;
            }

            return totalSum / data.Rows.Count;
        }

        public static double GetColumnValue(this DataRow row, string name)
        {
            var columnIndex = row.Table.Columns[name].Ordinal;
            return (double)row[columnIndex];
        }
    }
}
