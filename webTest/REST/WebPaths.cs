﻿/*
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

using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Extensions;

namespace competenceservice
{
    public class WebPaths : Nancy.NancyModule
    {
        public WebPaths() : base("/rest/competenceservice")
        {

            #region WebMethods

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
            Get["/getcompetencestate/{tid}"] = data => WebMethods.getcp(data.tid);

            

            #endregion
        }
    }
}