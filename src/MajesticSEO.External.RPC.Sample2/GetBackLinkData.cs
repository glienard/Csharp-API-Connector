/// <copyright>
/// The license for this file can be found at https://github.com/majestic/Csharp-API-Connector.
/// </copyright>

/* NOTE: The code below is specifically for the GetTopbackLinks API command
 *       For other API commands, the arguments required may differ.
 *       Please refer to the Majestic Developer Wiki for more information
 *       regarding other API commands and their arguments.
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MajesticSEO.External.RPC
{
    public class GetBackLinkData
    {
        public static void Main(string[] args)
        {
            string endpoint = "http://enterprise.majestic.com/api_command";

            Console.WriteLine("\n***********************************************************"
                + "*****************");

            Console.WriteLine("\nEndpoint: " + endpoint);

            if ("http://enterprise.majestic.com/api_command".Equals(endpoint))
            {
                Console.WriteLine("\nThis program is hard-wired to the Enterprise API.");

                Console.WriteLine("\nIf you do not have access to the Enterprise API, "
                    + "change the endpoint to: \nhttp://developer.majestic.com/api_command.");
            }
            else
            {
                Console.WriteLine("\nThis program is hard-wired to the Developer API "
                    + "and hence the subset of data \nreturned will be substantially "
                    + "smaller than that which will be returned from \neither the "
                    + "Enterprise API or the Majestic website.");

                Console.WriteLine("\nTo make this program use the Enterprise API, change "
                    + "the endpoint to: \nhttp://enterprise.majestic.com/api_command.");
            }

            Console.WriteLine("\n***********************************************************"
                    + "*****************");

            Console.WriteLine(
                "\n\nThis example program will return the top backlinks for any URL, domain "
                + "\nor subdomain."
                + "\n\nThe following must be provided in order to run this program: "
                + "\n1. API key "
                + "\n2. A URL, domain or subdomain to query"
                + "\n\nPlease enter your API key:");

            string app_api_key = Console.ReadLine();

            Console.WriteLine("\nPlease enter a URL, domain or subdomain to query:");

            string itemToQuery = Console.ReadLine();
            
            // set up parameters
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("datasource", "fresh");
            parameters.Add("Count", "10");
            parameters.Add("item", itemToQuery);
            parameters.Add("Mode", "0");

            APIService apiService = new APIService(app_api_key, endpoint);
            Response response = apiService.ExecuteCommand("GetBackLinkData", parameters);

            // check the response code
            if(response.IsOK())
            {
                // print the URL table
                DataTable results = response.GetTableForName("BackLinks");

                foreach(Dictionary<string, string> row in results.GetTableRows())
                {
                    Console.WriteLine("\nURL: " + row["SourceURL"]);
                    Console.WriteLine("Trust Flow: " + row["SourceTrustFlow"]);
                }

                if ("http://developer.majestic.com/api_command".Equals(endpoint))
                {
                    Console.WriteLine("\n\n***********************************************************"
                        + "*****************");

                    Console.WriteLine("\nEndpoint: " + endpoint);

                    Console.WriteLine("\nThis program is hard-wired to the Developer API "
                        + "and hence the subset of data \nreturned will be substantially "
                        + "smaller than that which will be returned from \neither the "
                        + "Enterprise API or the Majestic website.");

                    Console.WriteLine("\nTo make this program use the Enterprise API, change "
                        + "the endpoint to: \nhttp://enterprise.majestic.com/api_command.");

                    Console.WriteLine("\n***********************************************************"
                        + "*****************");
                }
            }
            else
            {
                Console.WriteLine("\nERROR MESSAGE:");
                Console.WriteLine(response.GetErrorMessage());

                Console.WriteLine("\n\n***********************************************************"
                    + "*****************");

                Console.WriteLine("\nDebugging Info:");
                Console.WriteLine("\n  Endpoint: \t" + endpoint);
                Console.WriteLine("  API Key: \t" + app_api_key);

                if ("http://enterprise.majestic.com/api_command".Equals(endpoint))
                {
                    Console.WriteLine("\n  Is this API Key valid for this Endpoint?");

                    Console.WriteLine("\n  This program is hard-wired to the Enterprise API.");

                    Console.WriteLine("\n  If you do not have access to the Enterprise API, "
                       + "change the endpoint to: \n  http://developer.majestic.com/api_command.");
                }

                Console.WriteLine("\n***********************************************************"
                    + "*****************");
            }

            Console.Read();
        }
    }
}
