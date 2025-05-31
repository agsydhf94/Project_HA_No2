#if UNITY_EDITOR
using UnityEngine;
using System.Data;
using System.IO;
using ExcelDataReader;
using System.Collections.Generic;
using System.Text;
using System;

namespace HA
{
    public static class ExcelToJsonConverter
    {
        public static List<T> ConvertSheetToList<T>(string excelFilePath, string sheetName) where T : new()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var list = new List<T>();

            using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                var table = result.Tables[sheetName];

                var columns = new List<string>();
                for (int col = 0; col < table.Columns.Count; col++)
                    columns.Add(table.Rows[0][col].ToString());

                for (int row = 1; row < table.Rows.Count; row++)
                {
                    var item = new T();
                    var type = typeof(T);

                    for (int col = 0; col < columns.Count; col++)
                    {
                        var field = type.GetField(columns[col]);
                        if (field == null) continue;

                        object value = table.Rows[row][col];
                        if (value == DBNull.Value) continue;

                        // int나 string만 처리
                        if (field.FieldType == typeof(int))
                            field.SetValue(item, int.Parse(value.ToString()));
                        else if (field.FieldType == typeof(string))
                            field.SetValue(item, value.ToString());
                    }

                    list.Add(item);
                }
            }

            return list;
        }
    }
}
#endif