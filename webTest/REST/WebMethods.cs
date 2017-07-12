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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace competenceservice
{
    public static class WebMethods
    {

        #region Methods

        /// <summary>
        /// Method for storing a domainmodel, an id for this domainmodel is returned
        /// </summary>
        public static string storedm(string data)
        {
            string returnstring = CompetenceFramework.storedm(data);
            if (returnstring == null)
            {
                return "<failure />";
            }
            else
            {
                return returnstring;
            }
        }

        /// <summary>
        /// Method for deleting a domainmodel and all related competencestates and trackingids
        /// </summary>
        public static string deletedm(string dmid)
        {
            if (CompetenceFramework.deletedm(dmid))
            {
                return "<success />";
            }
            else
            {
                return "<failure />";
            }
        }
        
        /// <summary>
        /// Method for getting the domain model for a given domain model id (dmid)
        /// </summary>
        /// <param name="dmid"> id of the requested domain model</param>
        /// <returns> xml representation of the domain model</returns>
        public static string getdm(string dmid)
        {
            string returnstring = CompetenceFramework.getdm(dmid);
            if (returnstring == null)
            {
                return "<failure />";
            }
            else
            {
                return returnstring;
            }
        }

        /// <summary>
        /// Method for creating a trackingid for a given domainmodel speziffied by id (dmid)
        /// </summary>
        public static string createtrackingid(string dmid)
        {
            string returnstring = CompetenceFramework.createtrackingid(dmid);
            if (returnstring == null)
            {
                return "<failure />";
            }
            else
            {
                return returnstring;
            }
        }

        /// <summary>
        /// Method for updating the competence state of a payer by trackingid
        /// </summary>
        /// <param name="tid"> tracking id of the player</param>
        /// <param name="evidence"> xml representation of the evidence </param>
        /// <returns></returns>
        public static string updatecompetencestate(string tid, string evidence)
        {
            if (CompetenceFramework.updatecompetencestate(tid, evidence))
            {
                return "<success />";
            }
            else
            {
                return "<failure />";
            }
        }

        /// <summary>
        /// Method for deleting a tracing id and the related competence state
        /// </summary>
        /// <param name="tid"> tracking id to delete</param>
        /// <returns></returns>
        public static string deletetid(string tid)
        {
            if (CompetenceFramework.deletetid(tid))
            {
                return "<success />";
            }
            else
            {
                return "<failure />";
            }
        }

        /// <summary>
        /// Method for returning the competence state of a player by tracking id
        /// </summary>
        /// <param name="tid"> tracking id of the player</param>
        /// <returns> xml representation of the player's competence state</returns>
        public static string getcpByTid(string tid)
        {
            string returnstring = CompetenceFramework.getcpByTid(tid);
            if (returnstring == null)
            {
                return "<failure />";
            }
            else
            {
                return returnstring;
            }
        }

        /// <summary>
        /// Method for returning the competence probabilities of a player by tracking id
        /// the format is html + js, such that it can easily be included in a website
        /// </summary>
        public static string getcphtmlByTid(string tid)
        {
            //get domainmodel
            string competenceProbabilities = CompetenceFramework.getcpByTid(tid);
            if (competenceProbabilities == null)
            {
                return "<failure/>";
            }

            string dmid = competenceframework.CompetenceFramework.getDomainModelIdByTrackingId(tid);
            string dmstring = competenceframework.CompetenceFramework.getdm(dmid);

            
            //http://localhost:54059/rest/competenceservice/getcompetencestatehtml/1
            string ipaddress = HttpContext.Current.Request.Url.Authority;
            string  pathToJsFiles = "http://" + ipaddress + "/websites/js";
            string pathToCssFiles = "http://" + ipaddress + "/websites/css";

            string dm = "\"" + dmstring.Replace("\"", "'") + "\"";
            string cp = "\"" + competenceProbabilities.Replace("\"", "'") + "\"";
            string updateHistory = "\"" + CompetenceFramework.getTrackingHistory(tid).Replace("\"", "'") + "\"";
            string html = "";
            //add script for loading stylesheets from js
            //html += "<script> var style = document.createElement('link'); style.rel = 'stylesheet'; style.type = 'text/css'; style.href = '"+pathToCssFiles+"/viewcompetencestate.css'; document.getElementsByTagName('head')[0].appendChild(style);</script>\n";
            html += "<script> var style = document.createElement('link'); style.rel = 'stylesheet'; style.type = 'text/css'; style.href = '" + pathToCssFiles + "/vis.css'; document.getElementsByTagName('head')[0].appendChild(style);</script>\n";
            //add html code for the visualization
            html += "<div id='visualizationdiv'><div id ='timelinediv'><h3>Timeline:</h3><div id ='visualization'></div >";
            html += "</div><div id='graphdiv'><h3>Graph representation:</h3><div id='graphwrapperdiv'  style='width: 100%;overflow: hidden;'><div id='graph'";
            html +=" style='float: left;margin-right: 10px;width: 300px;height: 400px;border: solid 1px black;'></div>";
            html += "<div id ='nodeInfo' style='overflow: hidden;width: 300px;padding: 3px;'></div></div></div></div>\n";
            //include js files
            //html += "<script src='"+ pathToJsFiles + "/vis.js'></script>\n";
            html += "<script src='" + pathToJsFiles + "/graph.js'></script>\n";
            html += "<script src='" + pathToJsFiles + "/raphael-min.js'></script>\n";
            html += "<script src='" + pathToJsFiles + "/domainmodel.js'></script>\n";
            html += "<script src='" + pathToJsFiles + "/jquery-3.2.1.min.js'></script>\n";
            html += "<script src='" + pathToJsFiles + "/viewcompetencestate.js'></script>\n";
            //visualize data
            html += "<script>drawDomainModel(" + dm + ");\n drawCompetenceState(" + cp + ");\n drawUpdateHistory(" + updateHistory + ");\n timeline.redraw();\n</script>";

            
            return html;
        }

        #endregion

    }
}