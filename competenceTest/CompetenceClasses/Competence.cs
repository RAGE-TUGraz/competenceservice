using System;
using System.Collections.Generic;
using consoleTest;

namespace competenceTest
{
	//*
	/// <summary>
	/// Class representing a Competence in the Competence-Tree of the Domainmodel.
	/// </summary>
	public class Competence
	{
		#region Fields 

		/// <summary>
		/// Unique id within a competence-structure
		/// </summary>
		public string id;

		/// <summary>
		/// Human-readable name of the competence
		/// </summary>
		public string title;

		/// <summary>
		/// List of prerequisites to this competence
		/// </summary>
		public List<Competence> prerequisites = new List<Competence>();

		/// <summary>
		/// List of successors to this competence
		/// </summary>
		public List<Competence> successors = new List<Competence>();

		/// <summary>
		/// Competence-structure containing this competence
		/// </summary>
		public CompetenceStructure cst;

		#endregion Fields
		#region Constructors

		/// <summary>
		/// Competence ctor
		/// </summary>
		/// 
		/// <param name="newId"> Unique Competence Id within a competence structure. </param>
		/// <param name="newTitle"> Competence name/description. </param>
		/// <param name="newCst"> Spezifies CompetenceStructure this competence is contained in. </param>
		public Competence(String newId, String newTitle, CompetenceStructure newCst)
		{
			id = newId;
			title = newTitle;
			cst = newCst;
		}

		#endregion Constructors
		#region Methods

		/// <summary>
		/// Method for adding a prerequisite.
		/// </summary>
		/// 
		/// <param name="prerequisite"> Specifies prerequisite to be added to the competence. </param>
		public void addPrerequisite(Competence prerequisite)
		{
			prerequisites.Add(prerequisite);
		}

		/// <summary>
		/// Method for adding a successor.
		/// </summary>
		/// 
		/// <param name="successor"> Specifies successor to be added to the competence. </param>
		public void addSuccessor(Competence successor)
		{
			successors.Add(successor);
		}

		/// <summary>
		/// Diagnostic method for displaying a competence.
		/// </summary>
		public void print()
		{
			Logger.Log("Competence: " + id);

			if (prerequisites.Count > 0)
			{
				Logger.Log("Prerequisites: ");
			}
			foreach (Competence com in prerequisites)
			{
				Logger.Log("       - " + com.id);
			}

			if (successors.Count > 0)
			{
				Logger.Log("Successors: ");
			}
			foreach (Competence com in successors)
			{
				Logger.Log("       - " + com.id);
			}
		}

		/// <summary>
		/// Method determining if the competence (this) is a (in)direct prerequisite of a given competence (com).
		/// </summary>
		/// 
		/// <param name="com"> Specifies for which competence the checking is done. </param>
		///
		/// <returns>
		/// Boolean: True if the competence (this) is a (in)direct prerequisite of a given competence (com), false otherwise.
		/// </returns>
		public Boolean isIndirectPrerequesiteOf(Competence com)
		{
			if (this.id == com.id)
			{
				return (true);
			}
			else
			{
				if (com.prerequisites.Count == 0)
					return (false);

				foreach (Competence c in com.prerequisites)
				{
					if (this.isIndirectPrerequesiteOf(c))
						return (true);
				}
				return (false);
			}
		}

		/// <summary>
		/// Method determining if the competence (this) is a (in)direct successor of a given competence (com).
		/// </summary>
		/// 
		/// <param name="com"> Specifies for which competence the checking is done. </param>
		///
		/// <returns>
		/// Boolean: True if the competence (this) is a (in)direct successor of a given competence (com), false otherwise.
		/// </returns>
		public Boolean isIndirectSuccessorOf(Competence com)
		{
			return (!com.isIndirectPrerequesiteOf(this));
		}

		/// <summary>
		/// Method returning the set of all direct prerequisites not mastered with an given competence state
		/// </summary>
		/// <param name="cs"> competence state for wich the set should be returned </param>
		/// <returns> List of not possessed direct prerequisite competences </returns>
		public List<Competence> getPrerequisitesNotMastered(CompetenceState cs)
		{
			List<Competence> prereqNotMastered = new List<Competence>();
			foreach (Competence com in this.prerequisites)
				if (cs.getValue(com.id) < CompetenceHandler.transitionProbability)
					prereqNotMastered.Add(com);
			return (prereqNotMastered);
		}

		/// <summary>
		/// Method determining, if all prerequisites to one competence are met
		/// </summary>
		/// <param name="cs"> Competence for which this is determined </param>
		/// <returns> True, if all prerequisites are met, false otherwise</returns>
		public Boolean allPrerequisitesMet(CompetenceState cs)
		{
			Boolean allPrerequisitesMet = true;
			foreach(Competence com in this.prerequisites)
			{
				if (cs.getValue(com.id) < CompetenceHandler.transitionProbability)
				{
					allPrerequisitesMet = false;
					break;
				}
			}

			return allPrerequisitesMet;
		}

		/// <summary>
		/// Method determining, if all prerequisites to one competence are met
		/// </summary>
		/// <param name="cs"> Competence for which this is determined </param>
		/// <returns> True, if all prerequisites are met, false otherwise</returns>
		public Boolean allPrerequisitesMet(Dictionary<string,double> cs)
		{
			Boolean allPrerequisitesMet = true;
			foreach (Competence com in this.prerequisites)
			{
				if (cs[com.id] < CompetenceHandler.transitionProbability)
				{
					allPrerequisitesMet = false;
					break;
				}
			}

			return allPrerequisitesMet;
		}

		/// <summary>
		/// Method for getting all successor-competence for which all prerequisites are met, but this one
		/// </summary>
		/// <param name="cs"> Competence state </param>
		/// <returns>successor-competence for which all prerequisites are met, but this one</returns>
		public List<Competence> getSuccessorsWithAllPrerequisitesMasteredButThis(CompetenceState cs)
		{
			List<Competence> successorsWithAllPrerequisitesMasteredButThis = new List<Competence>();
			foreach(Competence competence in this.successors)
			{
				List<Competence> prerequisitesNotMastered = competence.getPrerequisitesNotMastered(cs);
				if (prerequisitesNotMastered.Count == 1 && prerequisitesNotMastered[0].id.Equals(this.id))
					successorsWithAllPrerequisitesMasteredButThis.Add(competence);
			}
			return successorsWithAllPrerequisitesMasteredButThis;
		}

		/// <summary>
		/// Method for getting all mastered successors of one competence
		/// </summary>
		/// <param name="cs"> Competence state </param>
		/// <returns>all mastered successors of one competence</returns>
		public List<Competence> getSuccessorsMastered(CompetenceState cs)
		{
			List<Competence> successorsMastered = new List<Competence>();
			foreach (Competence competence in this.successors)
				if (cs.getValue(competence.id) >= cs.transitionProbability)
					successorsMastered.Add(competence);
			return (successorsMastered);
		}

		/// <summary>
		/// Method for getting all prerequisite competences for one competence for which non successor is mastered but this one
		/// </summary>
		/// <param name="cs"> competence state </param>
		/// <returns>all prerequisite competences for one competence for which non successor is mastered but this one</returns>
		public List<Competence> getPrerequisiteWithAllSuccessorsNotInCompetenceStateButThis(CompetenceState cs)
		{
			List<Competence> prerequisiteWithAllSuccessorsNotInCompetenceStateButThis = new List<Competence>();
			foreach (Competence competence in this.prerequisites)
			{
				List<Competence> successorsMastered = competence.getSuccessorsMastered(cs);
				if (successorsMastered.Count == 1 && successorsMastered[0].id.Equals(this.id))
					prerequisiteWithAllSuccessorsNotInCompetenceStateButThis.Add(competence);
			}
			return prerequisiteWithAllSuccessorsNotInCompetenceStateButThis;
		}

		/// <summary>
		/// Method for calculating an update value, such that the competence reaches a certain limit
		/// </summary>
		/// <param name="updatedCompetence"> competence which gets updated </param>
		/// <param name="limitToBeReached"> the probability to be reached for this competence </param>
		/// <param name="cs"> the corresponding competence state </param>
		/// <param name="evidenceDirection"> indicates if an up- or downgrad is happening </param>
		/// <returns> the xi value for reaching the certain probability </returns>
		public double calculateXi(Competence updatedCompetence, double limitToBeReached, CompetenceState cs, Boolean evidenceDirection)
		{
			if (evidenceDirection)
			{
				if (updatedCompetence.isIndirectPrerequesiteOf(this) && updatedCompetence.id != this.id)
				{
					return ((limitToBeReached*(1-cs.getValue(updatedCompetence.id))) /(cs.getValue(this.id)-cs.getValue(updatedCompetence.id)*limitToBeReached));
				}
				else if (this.isIndirectPrerequesiteOf(updatedCompetence))
				{
					return ((limitToBeReached-cs.getValue(updatedCompetence)*limitToBeReached-cs.getValue(this.id)+cs.getValue(updatedCompetence.id))/(cs.getValue(updatedCompetence.id)*(1-limitToBeReached)));
				}
				else
					throw new Exception("This line should not be reached!");
			}
			else
			{
				if (updatedCompetence.isIndirectPrerequesiteOf(this) && updatedCompetence.id != this.id)
				{
					return ((cs.getValue(this.id)-limitToBeReached*cs.getValue(updatedCompetence.id))/(limitToBeReached*(1-cs.getValue(updatedCompetence.id))));
				}
				else if (this.isIndirectPrerequesiteOf(updatedCompetence))
				{
					return ((cs.getValue(updatedCompetence.id)*(limitToBeReached-1))/(-limitToBeReached*(1-cs.getValue(updatedCompetence.id))+(cs.getValue(this.id)-cs.getValue(updatedCompetence.id))));
				}
				else
					throw new Exception("This line should not be reached!");
			}
		}

		#endregion Methods

	}
	//*/
}

