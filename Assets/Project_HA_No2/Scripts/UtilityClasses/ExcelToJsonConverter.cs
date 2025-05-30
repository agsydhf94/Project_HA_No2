#if UNITY_EDITOR
using UnityEngine;
using System.Data;
using System.IO;
using ExcelDataReader;
using System.Collections.Generic;
using System.Text;

namespace HA
{
    public static class ExcelToJsonConverter
    {
        public static Dictionary<string, string> ConvertExcelToJsonBySheet(string excelFilePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var sheetJsonDict = new Dictionary<string, string>();

            using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();

                foreach (DataTable table in result.Tables)
                {
                    var jsonList = new List<Dictionary<string, object>>();
                    var columns = new List<string>();

                    for (int col = 0; col < table.Columns.Count; col++)
                        columns.Add(table.Rows[0][col].ToString());

                    for (int row = 1; row < table.Rows.Count; row++)
                    {
                        var dict = new Dictionary<string, object>();
                        for (int col = 0; col < columns.Count; col++)
                        {
                            dict[columns[col]] = table.Rows[row][col];
                        }
                        jsonList.Add(dict);
                    }

                    sheetJsonDict[table.TableName] = JsonUtility.ToJson(new Wrapper(jsonList), true);
                }
            }

            return sheetJsonDict;
        }

        [System.Serializable]
        private class Wrapper
        {
            public List<Dictionary<string, object>> items;
            public Wrapper(List<Dictionary<string, object>> items) => this.items = items;
        }
    }
}
#endif