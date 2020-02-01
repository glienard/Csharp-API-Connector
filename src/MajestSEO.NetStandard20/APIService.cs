/// <copyright>
/// The license for this file can be found at https://github.com/majestic/Csharp-API-Connector.
/// </copyright>
/// 
/// <version>0.9.4</version>

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace MajesticSEO.External.RPC
{
	public class APIService
	{
		/// <summary>
		/// The default timeout to wait before aborting a request, expressed in seconds.
		/// </summary>
		private const int DefaultTimeout = 5;

		private readonly string _applicationId;
		private readonly string _endpoint;

		/// <summary>
		/// Constructs a new instance of the ApiService class.
		/// </summary>
		/// <param name="applicationId">the unique identifier for your application - for api requests, this is your "api key" ... 	for OpenApp request, this is your "private key"</param>
		/// <param name="endpoint">must point to the url you wish to target, ie: enterprise or developer</param>
		public APIService(string applicationId, string endpoint)
		{
			_applicationId = applicationId;
			_endpoint = endpoint;
		}

		/// <summary>
		/// This method will execute the specified command as an api request, using the default timeout. 
		/// </summary>
		/// <param name="name">the name of the command you wish to execute, e.g. GetIndexItemInfo</param>
		/// <param name="parameters">a set of command parameters</param>
		/// <returns>the response</returns>
		public Response ExecuteCommand(string name, Dictionary<string, string> parameters)
		{
			return ExecuteCommand(name, parameters, DefaultTimeout);
		}

		/// <summary>
		/// This method will execute the specified command as an api request, using the specified timeout.
		/// </summary>
		/// <param name="name">the name of the command you wish to execute, e.g. GetIndexItemInfo</param>
		/// <param name="parameters">a set of command parameters</param>
		/// <param name="timeout">the amount of time to wait, expressed in seconds, before aborting the request</param>
		/// <returns>the response</returns>
		public Response ExecuteCommand(string name, Dictionary<string, string> parameters, int timeout)
		{
			parameters.Add("app_api_key", _applicationId);
			parameters.Add("cmd", name);

			return ExecuteRequest(parameters, timeout);
		}

		/// <summary>
		/// This method will execute the specified command as an api request, using the specified timeout.
		/// </summary>
		/// <param name="commandName">the name of the command you wish to execute, e.g. GetIndexItemInfo</param>
		/// <param name="parameters">a set of command parameters</param>
		/// <param name="accessToken">the token the user provided to access their resources</param>
		/// <returns>the response</returns>
		public Response ExecuteOpenAppRequest(string commandName, Dictionary<string, string> parameters, string accessToken)
		{
			return ExecuteOpenAppRequest(commandName, parameters, accessToken, DefaultTimeout);
		}

		/// <summary>
		/// This method will execute the specified command as an api request, using the specified timeout.
		/// </summary>
		/// <param name="commandName">the name of the command you wish to execute, e.g. GetIndexItemInfo</param>
		/// <param name="parameters">a set of command parameters</param>
		/// <param name="accessToken">the token the user provided to access their resources</param>
		/// <param name="timeout">the amount of time to wait, expressed in seconds, before aborting the request</param>
		/// <returns>the response</returns>
		public Response ExecuteOpenAppRequest(string commandName, Dictionary<string, string> parameters, string accessToken, int timeout)
		{
			parameters.Add("accesstoken", accessToken);
			parameters.Add("cmd", commandName);
			parameters.Add("privatekey", _applicationId);

			return ExecuteRequest(parameters, timeout);
		}

		/// <summary>
		/// This method executes the request.
		/// </summary>
		/// <param name="parameters">a set of command parameters</param>
		/// <param name="timeout">the amount of time to wait, expressed in seconds, before aborting the request</param>
		/// <returns></returns>
		private Response ExecuteRequest(Dictionary<string, string> parameters, int timeout)
		{
			// add query parameters to StringBuilder
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				sb.Append(parameter.Key).Append("=");
				sb.Append(HttpUtility.UrlEncode(parameter.Value)).Append("&"); // url encode values
			}

			// remove last "&" from string
			string queryString = sb.ToString().Substring(0, sb.Length - 1);

			// create a post request 
			Uri uri = new Uri(_endpoint);
			WebRequest request = WebRequest.Create(uri);
			request.Method = "POST";

			// create a byte array of query parameters
			byte[] queryAsBytes = Encoding.UTF8.GetBytes(queryString);

			// set request options
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = queryAsBytes.Length;
			request.Timeout = timeout * 1000;

			try
			{
				// write query parameters to request stream
				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(queryAsBytes, 0, queryAsBytes.Length);
				}

				// get response
				using (WebResponse response = request.GetResponse())
				{
					Stream responseStream = response.GetResponseStream();
					return new Response(responseStream);
				}
			}
			catch (WebException)
			{
				return new Response("ConnectionError", "Problem connecting to data source");
			}
		}
	}
}