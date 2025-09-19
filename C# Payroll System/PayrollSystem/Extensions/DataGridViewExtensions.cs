using System;
using System.Windows.Forms;

namespace PayrollSystem.Extensions
{
    public static class DataGridViewExtensions
    {
        public static void SetTimeSpanFormat(this DataGridView dgv, string columnName, string format = @"HH\:mm")
        {
            if (dgv.Columns[columnName] != null)
            {
                dgv.Columns[columnName].DefaultCellStyle.Format = format;
                dgv.Columns[columnName].DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.InvariantCulture;
                dgv.CellFormatting += (sender, e) =>
                {
                    if (e.ColumnIndex == dgv.Columns[columnName].Index && e.Value != null && e.Value != DBNull.Value)
                    {
                        try
                        {
                            if (e.Value is TimeSpan timeSpan)
                            {
                                e.Value = timeSpan.ToString(format);
                                e.FormattingApplied = true;
                            }
                            else if (e.Value is DateTime dateTime)
                            {
                                e.Value = dateTime.ToString(format);
                                e.FormattingApplied = true;
                            }
                        }
                        catch
                        {
                            e.Value = "--:--";
                            e.FormattingApplied = true;
                        }
                    }
                };
            }
        }
    }
}