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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using competenceframework;
using System;
using System.Net;
using System.IO;
using System.Text;

namespace UnitTestCompetenceservice
{
    [TestClass]
    public class TestCases
    {
        #region Fields
        public string domainmodel = null;
        public string urlprefix = "http://localhost:54059/rest/competenceservice";
        #endregion
        #region Initialization

        //called before each and every test method
        [TestInitialize]
        public void Initialize()
        {
            if (domainmodel == null)
                storeDomainmodel();
            else
                CompetenceFramework.resetStorage();
        }
        #endregion
        #region Utilitymethods

        /// <summary>
        /// Method creating the testdata and storing it after reseting the database
        /// </summary>
        /// <returns></returns>
        public void storeDomainmodel()
        {
            CompetenceFramework.resetStorage();
            CompetenceFramework.createTestdata();
            domainmodel = CompetenceFramework.getdm("1");
        }

        /// <summary>
        /// Method for firering a rest call
        /// </summary>
        /// <param name="url"> url to call</param>
        /// <param name="type"> type GET/POST...</param>
        /// <returns></returns>
        public string makeRESTcall(string url,string type,string data)
        {
            WebRequest webRequest =WebRequest.Create(url);
            webRequest.Method = type;
            if (type.Equals("POST"))
            {
                webRequest.ContentType = "text/xml";
                UTF8Encoding encoder1 = new UTF8Encoding();
                byte[] bytes = encoder1.GetBytes(data);
                Stream webStream = webRequest.GetRequestStream();
                webStream.Write(bytes, 0, bytes.Length);
                webStream.Close();
            }
            WebResponse webResp = webRequest.GetResponse();

            var reader = new StreamReader(webResp.GetResponseStream());
            string result = reader.ReadToEnd();
            return(result);
        }

        #endregion
        #region TestmethodsCompetenceFramework

        /// <summary>
        /// Storing a domainmodel, requesting a tracking id and performing an update by competence id
        /// </summary>
        [TestCategory("competenceframework"), TestMethod]
        public void test01()
        {
            Console.WriteLine("************************** TEST 1 **************************");
            //getting DOMAINMODEL and its ID
            string domainmodelid = CompetenceFramework.storedm(domainmodel);
            Console.WriteLine("DOMAINMODELID:\n==============\n" + domainmodelid);
            string dm = CompetenceFramework.getdm(domainmodelid);
            Console.WriteLine("DOMAINMODEL:\n============\n" + dm);

            //requesting tracking id to the domainmodel
            string trackingid = CompetenceFramework.createtrackingid(domainmodelid);
            Console.WriteLine("TRACKINGID:\n===========\n" + trackingid);

            //requesting the competence probabilities
            string cp = CompetenceFramework.getcpByTid(trackingid);
            Console.WriteLine("COMPETENCES:\n============\n" + cp);

            //performing an update for competence 'C1'
            Console.WriteLine("PERFORMING UPDATE BASED ON COMPETENCE ID (C1-MEDIUM-UP)");
            string evidence = "<evidenceset><evidence><type>Competence</type><competenceid>C1</competenceid><direction>true</direction><power>Medium</power></evidence></evidenceset>";
            if (!CompetenceFramework.updatecompetencestate(trackingid, evidence))
                throw new Exception("Update unsuccessful");

            //requesting the competence probabilities
            cp = CompetenceFramework.getcpByTid(trackingid);
            Console.WriteLine("COMPETENCES:\n============\n" + cp);
        }

        /// <summary>
        /// Storing a domainmodel, requesting a tracking id and performing an update by activity
        /// </summary>
        [TestCategory("competenceframework"), TestMethod]
        public void test02()
        {
            Console.WriteLine("************************** TEST 2 **************************");
            //getting DOMAINMODEL and its ID
            string domainmodelid = CompetenceFramework.storedm(domainmodel);
            Console.WriteLine("DOMAINMODELID:\n==============\n" + domainmodelid);
            string dm = CompetenceFramework.getdm(domainmodelid);
            Console.WriteLine("DOMAINMODEL:\n============\n" + dm);

            //requesting tracking id to the domainmodel
            string trackingid = CompetenceFramework.createtrackingid(domainmodelid);
            Console.WriteLine("TRACKINGID:\n===========\n" + trackingid);

            //requesting the competence probabilities
            string cp = CompetenceFramework.getcpByTid(trackingid);
            Console.WriteLine("COMPETENCES:\n============\n" + cp);

            //performing an update for competence 'C1'
            Console.WriteLine("PERFORMING UPDATE BASED ON ACTIVITY (C1-MEDIUM-UP)");
            string evidence = "<evidenceset><evidence><type>Activity</type><activity>activityc1</activity></evidence></evidenceset>";
            if(!CompetenceFramework.updatecompetencestate(trackingid, evidence))
                throw new Exception("Update unsuccessful");

            //requesting the competence probabilities
            cp = CompetenceFramework.getcpByTid(trackingid);
            Console.WriteLine("COMPETENCES:\n============\n" + cp);
        }

        /// <summary>
        /// Storing a domainmodel, requesting a tracking id and performing an update by gamesituation
        /// </summary>
        [TestCategory("competenceframework"), TestMethod]
        public void test03()
        {
            Console.WriteLine("************************** TEST 3 **************************");
            //getting DOMAINMODEL and its ID
            string domainmodelid = CompetenceFramework.storedm(domainmodel);
            Console.WriteLine("DOMAINMODELID:\n==============\n" + domainmodelid);
            string dm = CompetenceFramework.getdm(domainmodelid);
            Console.WriteLine("DOMAINMODEL:\n============\n" + dm);

            //requesting tracking id to the domainmodel
            string trackingid = CompetenceFramework.createtrackingid(domainmodelid);
            Console.WriteLine("TRACKINGID:\n===========\n" + trackingid);

            //requesting the competence probabilities
            string cp = CompetenceFramework.getcpByTid(trackingid);
            Console.WriteLine("COMPETENCES:\n============\n" + cp);

            //performing an update for competence 'C1'
            Console.WriteLine("PERFORMING UPDATE BASED ON GAMESITUATION (C1-MEDIUM-UP)");
            string evidence = "<evidenceset><evidence><type>Gamesituation</type><gamesituation>gs1</gamesituation><direction>true</direction></evidence></evidenceset>";
            if(!CompetenceFramework.updatecompetencestate(trackingid, evidence))
                throw new Exception("Update unsuccessful");

            //requesting the competence probabilities
            cp = CompetenceFramework.getcpByTid(trackingid);
            Console.WriteLine("COMPETENCES:\n============\n" + cp);
        }

        /// <summary>
        /// Creating a tracking id and deleting the tracking id again
        /// </summary>
        [TestCategory("competenceframework"), TestMethod]
        public void test04()
        {
            Console.WriteLine("************************** TEST 4 **************************");
            //getting DOMAINMODEL and its ID
            string domainmodelid = CompetenceFramework.storedm(domainmodel);
            Console.WriteLine("DOMAINMODELID:\n==============\n" + domainmodelid);
            string dm = CompetenceFramework.getdm(domainmodelid);
            Console.WriteLine("DOMAINMODEL:\n============\n" + dm);

            //requesting tracking id to the domainmodel
            string trackingid = CompetenceFramework.createtrackingid(domainmodelid);
            Console.WriteLine("TRACKINGID:\n===========\n" + trackingid);

            //requesting the competence probabilities
            string cp = CompetenceFramework.getcpByTid(trackingid);
            Console.WriteLine("COMPETENCES:\n============\n" + cp);

            //deleting the tracking id
            CompetenceFramework.deletetid(trackingid);

            //requesting the competence probabilities
            cp = CompetenceFramework.getcpByTid(trackingid);
            if (cp != null)
                throw new Exception("this should be null now!");
        }

        /// <summary>
        /// Creating a tracking id to domain model and deleting the domain model
        /// </summary>
        [TestCategory("competenceframework"), TestMethod]
        public void test05()
        {
            Console.WriteLine("************************** TEST 5 **************************");
            //getting DOMAINMODEL and its ID
            string domainmodelid = CompetenceFramework.storedm(domainmodel);
            Console.WriteLine("DOMAINMODELID:\n==============\n" + domainmodelid);
            string dm = CompetenceFramework.getdm(domainmodelid);
            Console.WriteLine("DOMAINMODEL:\n============\n" + dm);

            //requesting tracking id to the domainmodel
            string trackingid = CompetenceFramework.createtrackingid(domainmodelid);
            Console.WriteLine("TRACKINGID:\n===========\n" + trackingid);

            //requesting the competence probabilities
            string cp = CompetenceFramework.getcpByTid(trackingid);
            Console.WriteLine("COMPETENCES:\n============\n" + cp);

            //deleting the domainmodel
            CompetenceFramework.deletedm(domainmodelid);

            //requesting the competence probabilities
            cp = CompetenceFramework.getcpByTid(trackingid);
            if (cp != null)
                throw new Exception("this should be null now!");

            //requesting the domain model
            dm = CompetenceFramework.getdm(domainmodelid);
            if (dm != null)
                throw new Exception("this should be null now!");
        }

        #endregion
        #region TestmethodsCompetenceService

        /// <summary>
        /// Storing a domainmodel, requesting a tracking id and performing an update by competence id
        /// </summary>
        [TestCategory("competenceservice"), TestMethod]
        public void test06()
        {
            //getting DOMAINMODEL and its ID
            Console.WriteLine("************************** TEST 6 **************************");
            string domainmodelid = makeRESTcall(urlprefix + "/storedm", "POST", domainmodel);
            Console.WriteLine("DOMAINMODELID:\n==============\n" + domainmodelid);
            string dm = makeRESTcall(urlprefix+"/getdm/"+domainmodelid,"GET","");
            Console.WriteLine("DOMAINMODEL:\n============\n" + dm);
            
            //requesting tracking id to the domainmodel
            string trackingid = makeRESTcall(urlprefix+ "/createtrackingid/"+ domainmodelid,"GET","");
            Console.WriteLine("TRACKINGID:\n===========\n" + trackingid);

            //requesting the competence probabilities
            string cp = makeRESTcall(urlprefix + "/getcompetencestate/" + trackingid, "GET", "");
            Console.WriteLine("COMPETENCES:\n============\n" + cp);

            //performing an update for competence 'C1'
            Console.WriteLine("PERFORMING UPDATE BASED ON COMPETENCE ID (C1-MEDIUM-UP)");
            string evidence = "<evidenceset><evidence><type>Competence</type><competenceid>C1</competenceid><direction>true</direction><power>Medium</power></evidence></evidenceset>";
            makeRESTcall(urlprefix + "/updatecompetencestate/" + trackingid, "POST", evidence);

            //requesting the competence probabilities
            cp = makeRESTcall(urlprefix + "/getcompetencestate/" + trackingid, "GET", "");
            Console.WriteLine("COMPETENCES:\n============\n" + cp);
            
        }

        /// <summary>
        /// Storing a domainmodel, requesting a tracking id and performing an update by activity
        /// </summary>
        [TestCategory("competenceservice"), TestMethod]
        public void test07()
        {
            Console.WriteLine("************************** TEST 7 **************************");
            //getting DOMAINMODEL and its ID
            string domainmodelid = makeRESTcall(urlprefix + "/storedm", "POST", domainmodel);
            Console.WriteLine("DOMAINMODELID:\n==============\n" + domainmodelid);
            string dm = makeRESTcall(urlprefix + "/getdm/" + domainmodelid, "GET", "");
            Console.WriteLine("DOMAINMODEL:\n============\n" + dm);

            //requesting tracking id to the domainmodel
            string trackingid = makeRESTcall(urlprefix + "/createtrackingid/" + domainmodelid, "GET", "");
            Console.WriteLine("TRACKINGID:\n===========\n" + trackingid);

            //requesting the competence probabilities
            string cp = makeRESTcall(urlprefix + "/getcompetencestate/" + trackingid, "GET", "");
            Console.WriteLine("COMPETENCES:\n============\n" + cp);

            //performing an update for competence 'C1'
            Console.WriteLine("PERFORMING UPDATE BASED ON ACTIVITY (C1-MEDIUM-UP)");
            string evidence = "<evidenceset><evidence><type>Activity</type><activity>activityc1</activity></evidence></evidenceset>";
            makeRESTcall(urlprefix + "/updatecompetencestate/" + trackingid, "POST", evidence);

            //requesting the competence probabilities
            cp = makeRESTcall(urlprefix + "/getcompetencestate/" + trackingid, "GET", "");
            Console.WriteLine("COMPETENCES:\n============\n" + cp);
        }

        /// <summary>
        /// Storing a domainmodel, requesting a tracking id and performing an update by gamesituation
        /// </summary>
        [TestCategory("competenceservice"), TestMethod]
        public void test08()
        {
            Console.WriteLine("************************** TEST 8 **************************");
            //getting DOMAINMODEL and its ID
            string domainmodelid = makeRESTcall(urlprefix + "/storedm", "POST", domainmodel);
            Console.WriteLine("DOMAINMODELID:\n==============\n" + domainmodelid);
            string dm = makeRESTcall(urlprefix + "/getdm/" + domainmodelid, "GET", "");
            Console.WriteLine("DOMAINMODEL:\n============\n" + dm);

            //requesting tracking id to the domainmodel
            string trackingid = makeRESTcall(urlprefix + "/createtrackingid/" + domainmodelid, "GET", "");
            Console.WriteLine("TRACKINGID:\n===========\n" + trackingid);

            //requesting the competence probabilities
            string cp = makeRESTcall(urlprefix + "/getcompetencestate/" + trackingid, "GET", "");
            Console.WriteLine("COMPETENCES:\n============\n" + cp);

            //performing an update for competence 'C1'
            Console.WriteLine("PERFORMING UPDATE BASED ON GAMESITUATION (C1-MEDIUM-UP)");
            string evidence = "<evidenceset><evidence><type>Gamesituation</type><gamesituation>gs1</gamesituation><direction>true</direction></evidence></evidenceset>";
            makeRESTcall(urlprefix + "/updatecompetencestate/" + trackingid, "POST", evidence);

            //requesting the competence probabilities
            cp = makeRESTcall(urlprefix + "/getcompetencestate/" + trackingid, "GET", "");
            Console.WriteLine("COMPETENCES:\n============\n" + cp);
        }

        /// <summary>
        /// Creating a tracking id and deleting the tracking id again
        /// </summary>
        [TestCategory("competenceservice"), TestMethod]
        public void test09()
        {
            Console.WriteLine("************************** TEST 9 **************************");
            //getting DOMAINMODEL and its ID
            string domainmodelid = makeRESTcall(urlprefix + "/storedm", "POST", domainmodel);
            Console.WriteLine("DOMAINMODELID:\n==============\n" + domainmodelid);
            string dm = makeRESTcall(urlprefix + "/getdm/" + domainmodelid, "GET", "");
            Console.WriteLine("DOMAINMODEL:\n============\n" + dm);

            //requesting tracking id to the domainmodel
            string trackingid = makeRESTcall(urlprefix + "/createtrackingid/" + domainmodelid, "GET", "");
            Console.WriteLine("TRACKINGID:\n===========\n" + trackingid);

            //requesting the competence probabilities
            string cp = makeRESTcall(urlprefix + "/getcompetencestate/" + trackingid, "GET", "");
            Console.WriteLine("COMPETENCES:\n============\n" + cp);

            //deleting the tracking id
            makeRESTcall(urlprefix + "/deletetrackingid/" + trackingid, "DELETE", "");

            //requesting the competence probabilities
            cp = makeRESTcall(urlprefix + "/getcompetencestate/" + trackingid, "GET", "");
            if (!cp.Equals("<failure />"))
                throw new Exception("this should yield a failure now!");
        }

        /// <summary>
        /// Creating a tracking id to domain model and deleting the domain model
        /// </summary>
        [TestCategory("competenceservice"), TestMethod]
        public void test10()
        {
            Console.WriteLine("************************** TEST 10 **************************");
            //getting DOMAINMODEL and its ID
            string domainmodelid = makeRESTcall(urlprefix + "/storedm", "POST", domainmodel);
            Console.WriteLine("DOMAINMODELID:\n==============\n" + domainmodelid);
            string dm = makeRESTcall(urlprefix + "/getdm/" + domainmodelid, "GET", "");
            Console.WriteLine("DOMAINMODEL:\n============\n" + dm);

            //requesting tracking id to the domainmodel
            string trackingid = makeRESTcall(urlprefix + "/createtrackingid/" + domainmodelid, "GET", "");
            Console.WriteLine("TRACKINGID:\n===========\n" + trackingid);

            //requesting the competence probabilities
            string cp = makeRESTcall(urlprefix + "/getcompetencestate/" + trackingid, "GET", "");
            Console.WriteLine("COMPETENCES:\n============\n" + cp);

            //deleting the domain model
            makeRESTcall(urlprefix + "/deletedm/" + domainmodelid, "DELETE", "");

            //requesting the competence probabilities
            cp = makeRESTcall(urlprefix + "/getcompetencestate/" + trackingid, "GET", "");
            if (!cp.Equals("<failure />"))
                throw new Exception("this should yield a failure now!");

            //requesting the domain model
            dm = makeRESTcall(urlprefix + "/getdm/" + domainmodelid, "GET", "");
            if (!dm.Equals("<failure />"))
                throw new Exception("this should yield a failure now!");
        }

        #endregion
    }
}
