using System;
using consoleTest;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace competenceTest
{
	/// <summary>
	/// Class containing all Domainmodel data.
	/// </summary>
	[XmlRoot("domainmodel")]
	public class DomainModel
	{
		#region Properties

		[XmlElement("elements")]
		public Elements elements { get; set; }

		[XmlElement("relations")]
		public Relations relations { get; set; }

		[XmlElement("updatelevels")]
		public UpdateLevels updateLevels { get; set; }


		#endregion Properties

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("Printing out DM:");
			elements.print();
			relations.print();
			if(updateLevels != null)
				updateLevels.print();
		}

		#region Methods

		public static DomainModel getDMFromXmlString(String str)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(DomainModel));
			using (TextReader reader = new StringReader(str))
			{
				DomainModel result = (DomainModel)serializer.Deserialize(reader);
				return (result);
			}
		}

		public String toXmlString()
		{
			try
			{
				var xmlserializer = new XmlSerializer(typeof(DomainModel));
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

		#endregion Methods
	}

	public class UpdateLevels
	{
		#region Fields

		[XmlElement("level")]
		public List<UpdateLevel> updateLevelList { get; set; }

		#endregion Fields
		#region Methods

		public void print()
		{
			Logger.Log("-Printing out updateLevels:");
			foreach(UpdateLevel ul in updateLevelList)
				ul.print();
		}

		#endregion Methods
	}

	public class UpdateLevel
	{

		#region Fields

		[XmlAttribute("direction")]
		public string direction { get; set; }

		[XmlAttribute("power")]
		public string power { get; set; }

		[XmlAttribute("xi")]
		public string xi { get; set; }

		[XmlAttribute("minonecompetence")]
		public string minonecompetence { get; set; }

		[XmlAttribute("maxonelevel")]
		public string maxonelevel { get; set; }

		#endregion Fields


		#region Methods

		public void print()
		{
			Logger.Log("-----");
			Logger.Log("--direction:" +direction);
			Logger.Log("--power:" + power);
			Logger.Log("--xi:" + xi);
			Logger.Log("--minonecompetence:" + minonecompetence);
			Logger.Log("--maxonelevel:" + maxonelevel);

		}

		#endregion Methods

	}

	public class Elements
	{
		#region Properties

		[XmlElement("competences")]
		public CompetenceList competences { get; set; }
		[XmlElement("situations")]
		public SituationsList situations { get; set; }
		[XmlElement("activities")]
		public ActivityList activities { get; set; }

		#endregion Properties

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("-Printing out elements:");
			if (competences != null)
				competences.print();
			if (situations != null)
				situations.print();
			if (activities != null)
				activities.print();
		}
	}

	public class ActivityList
	{
		#region Properties

		[XmlElement("activity")]
		public List<Activity> activityList { get; set; }

		#endregion Properties

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("--Printing out Activities:");
			foreach (Activity ac in activityList)
			{
				ac.print();
			}
		}
	}

	public class Activity
	{
		#region Properties

		[XmlAttribute("id")]
		public string id { get; set; }

		#endregion Properties
		#region Constructors

		public Activity() { }

		public Activity(string id)
		{
			this.id = id;
		}

		#endregion Constructors
		#region Methods
		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("---id:" + id);
		}
		#endregion Methods
	}

	public class SituationsList
	{
		#region Properties

		[XmlElement("situation")]
		public List<Situation> situationList { get; set; }

		#endregion Properties

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("--Printing out Situations:");
			foreach (Situation si in situationList)
			{
				si.print();
			}
		}
	}

	public class Situation
	{
		#region Properties

		[XmlAttribute("id")]
		public string id { get; set; }
		[XmlAttribute("title")]
		public string title { get; set; }
		[XmlAttribute("uri")]
		public string uri { get; set; }

		#endregion Properties
		#region Constructor

		/// <summary>
		/// default c-tor
		/// </summary>
		public Situation() { }

		/// <summary>
		/// c-tor with id
		/// </summary>
		/// <param name="id"> Situation identifier </param>
		public Situation(String id)
		{
			this.id = id;
		}

		#endregion Constructor

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("---");
			Logger.Log("---id:" + id);
			Logger.Log("---title:" + title);
			Logger.Log("---uri:" + uri);
		}
	}

	public class CompetenceList
	{
		#region Properties

		[XmlElement("competence")]
		public List<CompetenceDesc> competenceList { get; set; }

		#endregion Properties

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("--Printing out Competences:");
			foreach (CompetenceDesc comp in competenceList)
				comp.print();
		}
	}

	public class CompetenceDesc
	{

		#region Properties

		[XmlAttribute("description")]
		public string description { get; set; }
		[XmlAttribute("id")]
		public string id { get; set; }
		[XmlAttribute("title")]
		public string title { get; set; }
		[XmlAttribute("uri")]
		public string uri { get; set; }

		#endregion Properties

		#region Constructor

		/// <summary>
		/// default c-tor
		/// </summary>
		public CompetenceDesc() { }

		/// <summary>
		/// c-tor with id
		/// </summary>
		/// <param name="id"> competence identifier </param>
		public CompetenceDesc(String id)
		{
			this.id = id;
		}

		#endregion Constructor

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("---");
			Logger.Log("---description:" + description);
			Logger.Log("---id:" + id);
			Logger.Log("---title:" + title);
			Logger.Log("---uri:" + uri);
		}
	}

	public class Relations
	{
		#region Properties

		[XmlElement("competenceprerequisites")]
		public CompetenceprerequisitesList competenceprerequisites { get; set; }

		[XmlElement("situations")]
		public SituationRelationList situations { get; set; }
		[XmlElement("activities")]
		public ActivityRelationList activities { get; set; }

		#endregion Properties

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("-Printing out relations:");
			if (competenceprerequisites != null)
				competenceprerequisites.print();
			if (situations != null)
				situations.print();
			if (activities != null)
				activities.print();
		}
	}

	public class SituationRelationList
	{
		#region Properties

		[XmlElement("situation")]
		public List<SituationRelation> situations { get; set; }

		#endregion Properties

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("--Printing out situation-Relation:");
			foreach (SituationRelation sr in situations)
			{
				sr.print();
			}
		}

		/*
        public void addSituationRelation(string situationId, string competenceId)
        {
            Boolean found = false;
            foreach(SituationRelation sr in this.situations)
            {
                if (sr.id.Equals(situationId))
                {
                    CompetenceSituation competence = new CompetenceSituation();
                    competence.id = competenceId;
                    competence.up = "medium";
                    competence.down = "medium";
                    sr.competences.Add(competence);
                    found = true;
                    break;
                }
            }
            if (!found)
                situations.Add(new SituationRelation(situationId,new String[] { competenceId }));
        }
        */
	}

	public class SituationRelation
	{
		#region Properties

		[XmlAttribute("id")]
		public String id { get; set; }
		[XmlElement("competence")]
		public List<CompetenceSituation> competences { get; set; }

		#endregion Properties
		#region Constructors

		public SituationRelation() { }

		public SituationRelation(string situationId, string[] competenceId)
		{
			id = situationId;
			competences = new List<CompetenceSituation>();
			foreach(String cid in competenceId)
			{
				CompetenceSituation competence = new CompetenceSituation();
				competence.id = cid;
				competence.up = "medium";
				competence.down = "medium";
				competences.Add(competence);
			}
		}

		#endregion Constructors
		#region Methds
		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("---------------");
			Logger.Log("---id pr.:" + id);
			foreach(CompetenceSituation cs in this.competences)
				cs.print();
		}

		#endregion Methods
	}

	public class CompetenceSituation
	{
		#region Properties

		[XmlAttribute("id")]
		public String id { get; set; }

		[XmlAttribute("levelup")]
		public String up { get; set; }

		[XmlAttribute("leveldown")]
		public String down { get; set; }

		#endregion Properties

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("---id comp.:" + id);
			Logger.Log("---up comp.:" + up);
			Logger.Log("---down comp.:" + down);
		}
	}

	public class ActivityRelationList
	{
		#region Properties

		[XmlElement("activity")]
		public List<ActivitiesRelation> activities { get; set; }

		#endregion Properties

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("--Printing out activities-Relation:");
			foreach (ActivitiesRelation ar in activities)
			{
				ar.print();
			}
		}

		/*
        public void addActivityRelation(String activitytId, String competenceId)
        {
            Boolean found = false;
            foreach(ActivitiesRelation ar in this.activities)
                if(ar.id == activitytId)
                {
                    CompetenceActivity clo = new CompetenceActivity();
                    clo.id = competenceId;
                    ar.competences.Add(clo);
                    found = true;
                    break;
                }
            if (!found)
                this.activities.Add(new ActivitiesRelation(activitytId, competenceId ));
        }
        */
	}

	public class ActivitiesRelation
	{
		#region Properties

		[XmlAttribute("id")]
		public String id { get; set; }
		[XmlElement("competence")]
		public List<CompetenceActivity> competences { get; set; }

		#endregion Properties
		#region Constructors

		/// <summary>
		/// default c-tor
		/// </summary>
		public ActivitiesRelation() { }

		/// <summary>
		/// c-tor with learningobject id and competence id for a competence which is part of the learning object
		/// </summary>
		/// 
		/// <param name="activitytId">activity id</param>
		/// <param name="competenceId">competence id for a competence which is part of the learning object</param>
		public ActivitiesRelation(String activitytId, CompetenceActivity[] competenceId)
		{
			this.id = activitytId;
			this.competences = new List<CompetenceActivity>();
			foreach(CompetenceActivity cid in competenceId)
				this.competences.Add(cid);
		}

		public ActivitiesRelation(String id, string competenceId)
		{
			this.id = id;
			competences = new List<CompetenceActivity>();
			competences.Add(new CompetenceActivity(competenceId,"medium","up"));
		}

		#endregion Constructors

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("---id lo.:" + id);
			foreach(CompetenceActivity ca in this.competences)
				ca.print();
		}
	}

	public class CompetenceActivity
	{

		#region Properties

		[XmlAttribute("id")]
		public String id { get; set; }

		[XmlAttribute("power")]
		public String power { get; set; }

		[XmlAttribute("direction")]
		public String direction { get; set; }

		#endregion Properties
		#region Constructors

		public CompetenceActivity() { }

		public CompetenceActivity(string id, string power, string direction)
		{
			this.id = id;
			this.power = power;
			this.direction = direction;
		}

		#endregion Constructors
		#region Methods
		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("---id comp.:" + id);
			Logger.Log("---power comp.:" + power);
			Logger.Log("---direction comp.:" + direction);
		}
		#endregion Methods
	}

	public class CompetenceprerequisitesList
	{
		#region Properties

		[XmlElement("competence")]
		public List<CompetenceP> competences { get; set; }

		#endregion Properties

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("--Printing out Competenceprerequisites");
			foreach (CompetenceP cp in competences)
			{
				cp.print();
			}
		}

		/*
        public void addPrerequisiteCompetence(String id, String prerequisiteID)
        {
            Boolean found = false;
            foreach(CompetenceP cp in this.competences)
            {
                if (cp.id.Equals(id))
                {
                    Prereqcompetence precom = new Prereqcompetence();
                    precom.id = prerequisiteID;
                    cp.prereqcompetences.Add(precom);
                    found = true;
                    break;
                }
            }
            if (!found)
                competences.Add(new CompetenceP(id, new String[] { prerequisiteID }));
        }
        */
	}

	public class CompetenceP
	{
		#region Properties

		[XmlAttribute("id")]
		public String id { get; set; }
		[XmlElement("prereqcompetence")]
		public List<Prereqcompetence> prereqcompetences { get; set; }

		#endregion Properties

		#region Constructors

		/// <summary>
		/// default c-tor
		/// </summary>
		public CompetenceP() { }

		/// <summary>
		/// modified c-tor
		/// </summary>
		/// 
		/// <param name="id"> id of the competence </param>
		/// <param name="prerequisiteID"> id of the prerequisite competence </param>
		public CompetenceP(String id, String[] prerequisiteIDs)
		{
			this.id = id;
			prereqcompetences = new List<Prereqcompetence>();
			foreach (String pid in prerequisiteIDs)
			{
				Prereqcompetence precom = new Prereqcompetence();
				precom.id = pid;
				prereqcompetences.Add(precom);
			}
		}

		#endregion Constructors

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("---id: " + id);
			foreach(Prereqcompetence pc in prereqcompetences)
				pc.print();
		}
	}

	public class Prereqcompetence
	{
		#region Properties

		[XmlAttribute("id")]
		public String id { get; set; }

		#endregion Properties

		/// <summary>
		/// Diagnostic-method printing Domainmodel data
		/// </summary>
		public void print()
		{
			Logger.Log("---id prereq.:" + id);
		}
	}

}

