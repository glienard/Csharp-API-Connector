/// <copyright>
/// The license for this file can be found at https://github.com/majestic/Csharp-API-Connector.
/// </copyright>
/// 
/// <version>0.9.4</version>

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MajesticSEO.External.RPC
{
    public class Response
    {
        private Dictionary<string, string> responseAttributes;
        private Dictionary<string, string> parameters;
        private Dictionary<string, DataTable> tables;

        // Constructor for a valid response
        public Response(Stream xml_data)
        {
            responseAttributes = new Dictionary<string, string>();
            parameters = new Dictionary<string, string>();
            tables = new Dictionary<string, DataTable>();

            if (xml_data != null)
            {
                ImportData(xml_data);
            }
        }

        // Constructor for a failed response
        public Response(string code, string errorMessage) : this(null)
        {
            // response timed out
            responseAttributes.Add("Code", code);
            responseAttributes.Add("ErrorMessage", errorMessage);
            responseAttributes.Add("FullError", errorMessage);
        }

        // Parses the response xml, storing the result internally
        private void ImportData(Stream xml_data)
        {
            DataTable dataTable = null;

            XmlReader reader = XmlReader.Create(xml_data);
            reader.Read();

            while (!reader.EOF)
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "Result":
                            responseAttributes.Add("Code", reader["Code"]);
                            responseAttributes.Add("ErrorMessage", reader["ErrorMessage"]);
                            responseAttributes.Add("FullError", reader["FullError"]);
                            reader.Read();
                            break;

                        case "GlobalVars":
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    parameters.Add(reader.Name, reader.Value);
                                }
                            }

                            reader.Read();
                            break;

                        case "DataTable":
                            dataTable = new DataTable();
                            dataTable.SetTableName(reader["Name"]);
                            dataTable.SetTableHeaders(reader["Headers"]);

                            while (reader.MoveToNextAttribute())
                            {
                                if (!"Name".Equals(reader.Name) && !"Headers".Equals(reader.Name))
                                {
                                    dataTable.SetTableParams(reader.Name, reader.Value);
                                }
                            }

                            tables.Add(dataTable.GetTableName(), dataTable);
                            reader.Read();
                            break;

                        case "Row":
                            string row = reader.ReadElementContentAsString();
                            dataTable.SetTableRow(row);
                            break;

                        default:
                            reader.Read();
                            break;
                    }
                }
                else
                {
                    reader.Read();
                }
            }
        }

        // Returns the response's attributes
        public Dictionary<string, string> GetResponseAttributes()
        {
            return responseAttributes;
        }

        // Returns if the request was successful
        public bool IsOK()
        {
            return "OK".Equals(GetCode()) || "QueuedForProcessing".Equals(GetCode());
        }

        // Returns the response code - "OK" represents predicted state, all else represents an error
        public string GetCode()
        {
            return responseAttributes["Code"];
        }

        // Returns the response's error message if any
        public string GetErrorMessage()
        {
            return responseAttributes["ErrorMessage"];
        }

        // Returns the response's full error message if any
        public string GetFullError()
        {
            return responseAttributes["FullError"];
        }

        // Returns the response's global parameters
        public Dictionary<string, string> GetGlobalParams()
        {
            return parameters;
        }

        // Returns a specific parameter from the response's global parameters
        public string GetParamForName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            string param;
            if (parameters.TryGetValue(name, out param))
            {
                return param;
            }

            return null;
        }

        // Returns the response's data tables
        public Dictionary<string, DataTable> GetTables()
        {
            return tables;
        }

        // Returns a specific DataTable object from the response's data tables
        public DataTable GetTableForName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            DataTable table;
            if(tables.TryGetValue(name, out table))
            {
                return tables[name];
            }

            return new DataTable();
        }
    }
}
