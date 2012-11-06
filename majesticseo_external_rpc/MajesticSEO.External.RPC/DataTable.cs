
/// <copyright>
/// 
/// Version 0.9.3
/// 
/// Copyright (c) 2011, Majestic-12 Ltd
/// All rights reserved.
/// 
/// Redistribution and use in source and binary forms, with or without
/// modification, are permitted provided that the following conditions are met:
///     1. Redistributions of source code must retain the above copyright
///        notice, this list of conditions and the following disclaimer.
///     2. Redistributions in binary form must reproduce the above copyright
///        notice, this list of conditions and the following disclaimer in the
///        documentation and/or other materials provided with the distribution.
///     3. Neither the name of the Majestic-12 Ltd nor the
///        names of its contributors may be used to endorse or promote products
///        derived from this software without specific prior written permission.
///        
/// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
/// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
/// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
/// DISCLAIMED. IN NO EVENT SHALL Majestic-12 Ltd BE LIABLE FOR ANY
/// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
/// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
/// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
/// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
/// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
/// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
/// 
/// </copyright>

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
