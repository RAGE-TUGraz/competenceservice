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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CBKST.Elements
{
    /// <summary>
    /// Class storing multiple Evidences - used to update a competence probability vector 
    /// </summary>
    [XmlRoot("evidenceset")]
    public class EvidenceSet
    {
        #region Fields

        [XmlElement("evidence")]
        public List<Evidence> evidences;
        
        #endregion
        #region Constructors

        public EvidenceSet()
        {
            evidences = new List<Evidence>();
        }

        #endregion Constructors
        #region Methods

        public static EvidenceSet getESFromXmlString(String str)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EvidenceSet));
                using (TextReader reader = new StringReader(str))
                {
                    EvidenceSet result = (EvidenceSet)serializer.Deserialize(reader);
                    return (result);
                }
            }
            catch 
            {
                return null;
            }

        }

        public String toXmlString()
        {
            try
            {
                var xmlserializer = new XmlSerializer(typeof(EvidenceSet));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, this);
                    String xml = stringWriter.ToString();

                    return (xml);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        #endregion
    }
}
