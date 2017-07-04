using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace storagedb
{
    /// <summary>
    /// Class containing all History (competence developement + evidences) of one tracking id
    /// </summary>
    [XmlRoot("updatehistory")]
    public class UpdateHistory
    {
        #region Fields
        [XmlElement("updatehistoryentry")]
        public List<UpdateHistoryEntry> history = new List<UpdateHistoryEntry>();
        [XmlAttribute("trackingid")]
        public string trackingid;
        #endregion
        #region Constructor
        public UpdateHistory()
        {

        }

        public UpdateHistory(string tid)
        {
            this.trackingid = tid;
            List<string>[] historyList = DatabaseHandler.Instance.competencedevelopmentdb.getCompetenceDevelopment(tid);
            List<string> competenceState = historyList[1];
            List<string> dateTime = historyList[2];
            List<string> evidence = historyList[3];
            for(int i=0; i < competenceState.Count; i++)
            {
                history.Add(new UpdateHistoryEntry(competenceState[i],dateTime[i],evidence[i]));
            }
        }
        #endregion
        #region Methods
        public static UpdateHistory getDMFromXmlString(String str)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UpdateHistory));
                using (TextReader reader = new StringReader(str))
                {
                    UpdateHistory result = (UpdateHistory)serializer.Deserialize(reader);
                    return (result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Occured while desirilizing: " + e.Message);
                return null;
            }
        }

        public String toXmlString()
        {
            try
            {
                var xmlserializer = new XmlSerializer(typeof(UpdateHistory));
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

    public class UpdateHistoryEntry
    {
        #region Fields
        [XmlElement("competencestateentry")]
        public string competenceState;
        [XmlAttribute("datetimeentry")]
        public string dateTime;
        [XmlElement("evidenceentry")]
        public string evidence;
        #endregion
        #region Constructor
        public UpdateHistoryEntry()
        {
        }

        public UpdateHistoryEntry(string competenceState, string dateTime, string evidence)
        {
            this.competenceState = competenceState;
            this.dateTime = dateTime;
            //if (evidence.Equals("initial Competencestate"))
            //    this.evidence = "<evidence><initial Competencestate/><evidence>";
            this.evidence = evidence;
        }
        #endregion
    }
}
