/*
  Copyright 2016 TUGraz, http://www.tugraz.at/
  
  Licensed under the Apache License, Version 2.0 (the "License");
  you may not use this file except in compliance with the License.
  This project has received funding from the European Union’s Horizon
  2020 research and innovation programme under grant agreement No 644187.
  You may obtain a copy of the License at
  
      http://www.apache.org/licenses/LICENSE-2.0
  
  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
  
  This software has been created in the context of the EU-funded RAGE project.
  Realising and Applied Gaming Eco-System (RAGE), Grant agreement No 644187, 
  http://rageproject.eu/

  Development was done by Cognitive Science Section (CSS) 
  at Knowledge Technologies Institute (KTI)at Graz University of Technology (TUGraz).
  http://kti.tugraz.at/css/

  Created by: Matthias Maurer, TUGraz <mmaurer@tugraz.at>
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DomainModelAssetNameSpace
{
	/*

	/// <summary>
	/// Singelton Class for handling (read DM from Web/File, store DM to File) the Domainmodel (DM)
	/// </summary>
	internal class DomainModelHandler
	{
		#region Fields

		/// <summary>
		/// If true logging is done, otherwise no logging is done.
		/// </summary>
		private Boolean doLogging = true;

		/// <summary>
		/// Run-time Asset storage of domain model.
		/// </summary>
		private DomainModel domainModel = null;

		#endregion Fields
		#region Constructors

		/// <summary>
		/// private DomainModelHandler-ctor for Singelton-pattern 
		/// </summary>
		internal DomainModelHandler() { }

		#endregion Constructors
		#region Properties


		/// <summary>
		/// If set to true - logging is done, otherwise no logging is done.
		/// </summary>
		public Boolean DoLogging
		{
			set
			{
				doLogging = value;
			}
			get
			{
				return doLogging;
			}
		}

		/// <summary>
		/// DomainModel Property
		/// </summary>
		internal DomainModel DomainModel
		{
			get
			{
				return getDomainModel();
			}
		}

		#endregion Properties
		#region Methods 


		/// <summary>
		/// Method returning domain model either from the run-tima asset storage if available or from specified (default) source(File/Web).
		/// </summary>
		/// 
		/// <returns> The domein model. </returns>
		private DomainModel getDomainModel()
		{
			if (domainModel != null)
				return domainModel;
			DomainModel dm = loadDefaultDomainModel();
			domainModel = dm;
			return dm;
		}

		/// <summary>
		/// Method for setting the domain model
		/// </summary>
		/// <param name="dm"> The new doamin model</param>
		internal void setDomainModel(DomainModel dm)
		{
			domainModel = dm;
		}

		/// <summary>
		/// Method for storing a domain model.
		/// </summary>
		/// <param name="dm"> Domain model to store. </param>
		internal void storeDomainModel(DomainModel dm)
		{
			domainModel = dm;
		}

		/// <summary>
		/// Method loading domain model - location specified by settings.
		/// </summary>
		/// <returns>Domain Model for the player.</returns>
		internal DomainModel loadDefaultDomainModel()
		{
			loggingDM("Loading default Domain model.");
			DomainModelAssetSettings dmas = (DomainModelAssetSettings) getDMA().Settings;

			if (dmas.LocalSource)
			{
				IDataStorage ids = DomainModelAsset.Instance.getInterfaceFromAsset<IDataStorage>();
				if (ids != null )
				{
					if (!ids.Exists(dmas.Source))
					{
						loggingDM("File "+ dmas.Source + " not found for loading Domain model.", Severity.Error);
						throw new Exception("EXCEPTION: File "+ dmas.Source + " not found for loading Domain model.") ;
					}

					loggingDM("Loading DomainModel from File.");
					return (this.getDMFromXmlString(ids.Load(dmas.Source)));
				}
				else
				{
					loggingDM("IDataStorage bridge absent for requested local loading method of the Domain model.", Severity.Error);
					throw new Exception("EXCEPTION: IDataStorage bridge absent for requested local loading method of the Domain model.");
				}
			}
			else
			{
				IWebServiceRequest iwr = DomainModelAsset.Instance.getInterfaceFromAsset<IWebServiceRequest>();
				if (iwr != null)
				{
					loggingDM("Loading web DomainModel.");
					Uri uri = new Uri(dmas.Source);
					Dictionary<string, string> headers = new Dictionary<string, string>();
					//headers.Add("user", playerId);
					//string body = dmas.Source;
					WebServiceResponse wsr = new WebServiceResponse();
					//currentPlayerId = playerId;

					RequestSetttings rs = new RequestSetttings();
					rs.method = "GET";
					rs.uri = uri;
					rs.requestHeaders = headers;
					//rs.body = body;

					RequestResponse rr = new RequestResponse();

					iwr.WebServiceRequest(rs, out rr);
					return (this.getDMFromXmlString(rr.body));
				}
				else
				{
					loggingDM("IWebServiceRequest bridge absent for requested web loading method of the Domain model.", Severity.Error);
					throw new Exception("EXCEPTION: IWebServiceRequest bridge absent for requested web loading method of the Domain model.");
				}
			}

		}

		/// <summary>
		/// Method for deserialization of a XML-String to the coressponding Domainmodel.
		/// </summary>
		/// 
		/// <param name="str"> String containing the XML-Domainmodel for deserialization. </param>
		///
		/// <returns>
		/// DomainModel-type coressponding to the parameter "str" after deserialization.
		/// </returns>
		internal DomainModel getDMFromXmlString(String str)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(DomainModel));
			using (TextReader reader = new StringReader(str))
			{
				DomainModel result = (DomainModel)serializer.Deserialize(reader);
				return (result);
			}
		}

		#endregion Methods
		#region TestMethods

		/// <summary>
		/// Diagnostic logging method.
		/// </summary>
		/// 
		/// <param name="msg"> String to be logged.  </param>
		/// <param name="severity"> Severity of the logging-message, optional. </param>
		internal void loggingDM(String msg, Severity severity = Severity.Information)
		{
			if (DoLogging)
				DomainModelAsset.Instance.Log(severity, "[DMA]: " +msg);
		}

		#endregion TestMethods

	}

	/// <summary>
	/// Implementation of the WebServiceResponse-Interface for handling web requests.
	/// </summary>
	public class WebServiceResponse 
	{
		/// <summary>
		/// Describes behaviour in case the web request failed.
		/// </summary>
		/// <param name="url"></param>
		/// <param name="msg"></param>
		public void Error(string url, string msg)
		{
			DomainModelAsset.Handler.loggingDM("Web Request for retriving Domain model from "+url+" failed! " + msg, Severity.Error);
			throw new Exception("EXCEPTION: Web Request for retriving Domain model from " + url + " failed! " + msg);
		}

		/// <summary>
		/// Describes behaviour in case the web requests succeeds
		/// </summary>
		/// <param name="url"></param>
		/// <param name="code"></param>
		/// <param name="headers"></param>
		/// <param name="body"></param>
		public void Success(string url, int code, Dictionary<string, string> headers, string body)
		{
			DomainModelAsset.Handler.loggingDM("WebClient request successful!");
			DomainModelAsset.Handler.storeDomainModel(DomainModelAsset.Handler.getDMFromXmlString(body));
		}
	}


	*/
}
