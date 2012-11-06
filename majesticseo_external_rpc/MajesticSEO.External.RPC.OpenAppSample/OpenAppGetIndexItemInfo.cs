using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MajesticSEO.External.RPC.OpenAppSample
{
	internal class OpenAppGetIndexItemInfo
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		private static void Main(string[] args)
		{
			const string endpoint = "http://enterprise.majesticseo.com/api_command";

			Console.WriteLine("\n***********************************************************"
			                  + "*****************");

			Console.WriteLine("\nEndpoint: " + endpoint);

			if ("http://enterprise.majesticseo.com/api_command".Equals(endpoint))
			{
				Console.WriteLine("\nThis program is hard-wired to the Enterprise API.");

				Console.WriteLine("\nIf you do not have access to the Enterprise API, "
				                  + "change the endpoint to: \nhttp://developer.majesticseo.com/api_command.");
			}
			else
			{
				Console.WriteLine("\nThis program is hard-wired to the Developer API "
				                  + "and hence the subset of data \nreturned will be substantially "
				                  + "smaller than that which will be returned from \neither the "
				                  + "Enterprise API or the Majestic SEO website.");

				Console.WriteLine("\nTo make this program use the Enterprise API, change "
				                  + "the endpoint to: \nhttp://enterprise.majesticseo.com/api_command.");
			}

			Console.WriteLine("\n***********************************************************"
			                  + "*****************");

			Console.WriteLine(
				"\n\nThis example program will return key information about \"index items\"."
				+ "\n\nThe following must be provided in order to run this program: "
				+ "\n1. OpenApp \"private key\"\n2. Access Token\n3. List of items to query"
				+ "\n\nPlease enter your OpenApp \"private key\":");

			string privateKey = Console.ReadLine();

			Console.WriteLine("Enter your access token:");
			string accessToken = Console.ReadLine();

			Console.WriteLine(
				"\nPlease enter the list of items you wish to query seperated by "
				+ "commas: \n(e.g. majesticseo.com, majestic12.co.uk)");

			string itemsToQuery = Console.ReadLine();
			string[] items = Regex.Split(itemsToQuery, ", ");

			/* create a Dictionary from the resulting array with the key being
             * "item0 => first item to query, item1 => second item to query" etc */
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			for (int i = 0; i < items.Length; i++)
			{
				parameters.Add("item" + i, items[i]);
			}

			// add the total number of items to the Dictionary with the key being "items"
			parameters.Add("items", items.Length.ToString());
			parameters.Add("datasource", "fresh");

			APIService apiService = new APIService(privateKey, endpoint);
			Response response = apiService.ExecuteOpenAppRequest("GetIndexItemInfo", parameters, accessToken);

			// check the response code
			if (response.IsOK())
			{
				// print the results table
				DataTable results = response.GetTableForName("Results");

				foreach (Dictionary<string, string> row in results.GetTableRows())
				{
					string item = row["Item"];
					Console.WriteLine("\n<" + item + ">");

					List<string> keys = new List<string>(row.Keys);
					keys.Sort();

					foreach (string key in keys)
					{
						if (!key.Equals("item"))
						{
							string value = row[key];
							Console.WriteLine(" " + key + " ... " + value);
						}
					}
				}

				if ("http://developer.majesticseo.com/api_command".Equals(endpoint))
				{
					Console.WriteLine("\n\n***********************************************************"
					                  + "*****************");

					Console.WriteLine("\nEndpoint: " + endpoint);

					Console.WriteLine("\nThis program is hard-wired to the Developer API "
					                  + "and hence the subset of data \nreturned will be substantially "
					                  + "smaller than that which will be returned from \neither the "
					                  + "Enterprise API or the Majestic SEO website.");

					Console.WriteLine("\nTo make this program use the Enterprise API, change "
					                  + "the endpoint to: \nhttp://enterprise.majesticseo.com/api_command.");

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
				Console.WriteLine("  OpenApp \"private key\": \t" + privateKey);
				Console.WriteLine("  Access Token: \t" + accessToken);

				if ("http://enterprise.majesticseo.com/api_command".Equals(endpoint))
				{
					Console.WriteLine("\n  Is this API Key valid for this Endpoint?");

					Console.WriteLine("\n  This program is hard-wired to the Enterprise API.");

					Console.WriteLine("\n  If you do not have access to the Enterprise API, "
					                  + "change the endpoint to: \n  http://developer.majesticseo.com/api_command.");
				}

				Console.WriteLine("\n***********************************************************"
				                  + "*****************");
			}

			Console.Read();
		}
	}
}