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

using competenceframework;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Extensions;
using System.Text;
using Nancy;

namespace competenceservice
{
    public class WebPaths : Nancy.NancyModule
    {
        public WebPaths() : base("/rest/competenceservice")
        {

            #region WebMethods

            /// <summary>
            /// Method invoked before each route, checks the authentification and if database is running
            /// </summary>
            Before += (ctx) => {
                //check if database is running
                if (!CompetenceFramework.canConnectToDatabase())
                    return "<failure> Can not connect to Database. Is it runing? Is database present? </failure>";

                string[] splits = getUsernameAndPwdFromRequest(Request);
                if(splits.Length<2)
                    return "<failure> authentification failed1 </failure>";
                string username = splits[0];
                string password = splits[1];
                if(!CompetenceFramework.isUserValid(username, password))
                    return "<failure> authentification failed2 </failure>";

                return null;
            };

            /// <summary>
            /// Method for storing a domainmodel, an id for this domainmodel is returned
            /// </summary>
            Post["/storedm"] = data => WebMethods.storedm(Request.Body.AsString());

            /// <summary>
            /// Method for getting the domain model for a given domain model id (dmid)
            /// </summary>
            Get["/getdm/{dmid}"] = data => WebMethods.getdm(data.dmid);

            /// <summary>
            /// Method for deleting a domainmodel with given domainmodel id (dmid)
            /// </summary>
            Delete["/deletedm/{dmid}"] = data => WebMethods.deletedm(data.dmid);

            /// <summary>
            /// Method for creating a trackingid for a given domainmodel speziffied by id (dmid)
            /// </summary>
            Get["/createtrackingid/{dmid}"] = data => WebMethods.createtrackingid(data.dmid);

            /// <summary>
            /// Method for updating the competence state of a payer by trackingid
            /// </summary>
            Post["/updatecompetencestate/{tid}"] = data => WebMethods.updatecompetencestate(data.tid, Request.Body.AsString());

            /// <summary>
            /// Method for deleting a tracing id and the related competence state
            /// </summary>
            Delete["/deletetrackingid/{tid}"] = data => WebMethods.deletetid(data.tid);

            /// <summary>
            /// Method for returning the competence probabilities of a player by tracking id
            /// </summary>
            Get["/getcompetencestate/{tid}"] = data => WebMethods.getcpByTid(data.tid);

            /// <summary>
            /// Method for returning the competence probabilities of a player by tracking id
            /// the format is html + js, such that it can easily be included in a website
            /// </summary>
            Get["/getcompetencestatehtml/{tid}"] = data => WebMethods.getcphtmlByTid(data.tid);

            #endregion


        }

        #region Utilitymethods

        /// <summary>
        /// gets username and password for basic authentification from request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string[] getUsernameAndPwdFromRequest(Request request)
        {
            IEnumerable<string> authorizationHeader = request.Headers["Authorization"];
            string authorizationString = string.Join("-", authorizationHeader.ToArray());
            int beginPasswordIndexPosition = authorizationString.IndexOf(" ") + 1;
            string encodedAuth = authorizationString.Substring(beginPasswordIndexPosition);
            string decodedAuth = Encoding.UTF8.GetString(Convert.FromBase64String(encodedAuth));
            string[] splits = decodedAuth.Split(':');
            return splits;
        }
        #endregion
    }
}