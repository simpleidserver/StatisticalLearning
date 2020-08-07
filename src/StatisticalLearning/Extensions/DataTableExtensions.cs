using System.Collections.Generic;
using System.Data;

namespace StatisticalLearning.Extensions
{
    public static class DataTableExtensions
    {
        public static double[] GetColumn(this DataTable dataTable, string columnName)
        {
            var columnIndex = dataTable.GetColumnIndex(columnName);
            var result = new List<double>();
            if (columnName != null)
            {
                foreach(DataRow row in dataTable.Rows)
                {
                    result.Add((double)row[columnIndex.Value]);
                }
            }

            return result.ToArray();
        }

        public static int? GetColumnIndex(this DataTable dataTable, string columnName)
        {
            for(int i = 0; i < dataTable.Columns.Count; i++)
            {
                var column = dataTable.Columns[i];
                if (column.ColumnName == columnName)
                {
                    return i;
                }
            }

            return null;
        }
    }
}
