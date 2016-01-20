/// <copyright>
/// The license for this file can be found at https://github.com/majestic/Csharp-API-Connector.
/// </copyright>
/// 
/// <version>0.9.4</version>

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MajesticSEO.External.RPC
{
    public class DataTable
    {
        private string tableName;
        private List<string> tableHeaders;
        private Dictionary<string, string> tableParams;
        private List<Dictionary<string, string>> tableRows;

        // Constructor for a data table
        public DataTable()
        {
            tableName = "";
            tableHeaders = new List<string>();
            tableParams = new Dictionary<string, string>();
            tableRows = new List<Dictionary<string, string>>();
        }

        // Set the table's name
        public void SetTableName(string name)
        {
            tableName = name;
        }

        // Set the table's headers
        public void SetTableHeaders(string headers)
        {
            string[] headersArray = Split(headers);
            tableHeaders = new List<string>(headers.Length);
            tableHeaders.AddRange(headersArray);
        }

        // Set the table's parameters
        public void SetTableParams(string name, string value)
        {
            tableParams.Add(name, value);
        }

        // Set the table's row
        public void SetTableRow(string row)
        {
            Dictionary<string, string> rowsHash = new Dictionary<string, string>();
            string[] elements = Split(row);

            for (int index = 0; index < tableHeaders.Count; index++)
            {
                if (elements[index].Equals(" "))
                {
                    elements[index] = "";
                }

                rowsHash.Add(tableHeaders[index], elements[index]);
            }

            tableRows.Add(rowsHash);
        }

        // Splits the input from pipe separated form into an array.
        private string[] Split(string value)
        {
            string[] array = Regex.Split(value, "(?<!\\|)\\|(?!\\|)");

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = array[i].Replace("||", "|");
            }

            return array;
        }

        // Returns the table's name
        public string GetTableName()
        {
            return tableName;
        }

        // Returns the table's headers
        public List<string> GetTableHeaders()
        {
            return tableHeaders;
        }

        // Returns the table's parameters
        public Dictionary<string, string> GetTableParams()
        {
            return tableParams;
        }

        // Returns a table's parameter for a given name
        public string GetParamForName(string name)
        {
            string param;
            if (tableParams.TryGetValue(name, out param))
            {
                return param;
            }

            return null;
        }

        // Returns the number of rows in the table
        public int GetRowCount()
        {
            return tableRows.Count;
        }

        // Returns the table's rows
        public List<Dictionary<string, string>> GetTableRows()
        {
            return tableRows;
        }
    }
}
