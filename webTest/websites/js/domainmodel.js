drawDomainModel = function (domMod) {
    domainModelObject = new DomainModelObject(domMod);
    domainModelObject.drawDomainModel("graph");
    domainModelObject.displayNodeInformationOnClick("nodeInfo");
}

drawCompetenceState = function (comState) {
    domainModelObject.displayCompetenceState(comState);
    if (graph.highlightedNode != null)
        graph.nodeClicked();
}

drawUpdateHistory = function (udateh) {
    uh = new UpdateHistory(udateh);
    //timeline: see http://visjs.org/timeline_examples.html
    var container = document.getElementById('visualization');
    //var items = new vis.DataSet([{ id: 1, content: uh.entries[0].evidence, start: uh.entries[0].datetime, type: 'point' }, { id: 2, content: uh.entries[1].evidence, start: uh.entries[1].datetime, type: 'point' }]); 
    var items = new vis.DataSet();
    for (var i = 0; i < uh.entries.length; i++) {
        items.add([{ id: i, content: uh.entries[i].evidenceSet.getDisplayText(), start: uh.entries[i].datetime, type: 'point' }]);
    }
    var options = {"height": "200px"};
    var timeline = new vis.Timeline(container, items, options);

    timeline.on('click', function (properties) {
        if (properties.item != null) {
            drawCompetenceState(uh.entries[properties.item].competenceStateString);
        }
    });
}

function DomainModelObject(domainModelText) {
    this.graphWasDrawn = false;
    this.transitionprobability;
    this.domainModelString = domainModelText;
    this.nodeInformationDiv;
    this.competenceState;
    this.nodeInformationDivCreation = function (nodeId) {

        var element = $("#" + domainModelObject.nodeInformationDiv);
        //element.css.style.borderColor = "#ff0000";
        //element.css("border-color", "black");
        element.css("border", "solid 1px black");

        //get competence information by id
        var html = "";
        html += "<font size='1'>Competence: &nbsp &nbsp</font><font size='5'> " + nodeId.toUpperCase() + "</font><br>";
        if (domainModelObject.competenceState != null)
            html += "<font size='1'>Value &nbsp &nbsp &nbsp &nbsp &nbsp : &nbsp &nbsp</font> <font size='5'>" + Math.round(100 * domainModelObject.competenceState.getValueById(nodeId)) / 100 +"</font>";
        return html;
    }
    this.displayNodeInformationOnClick = function (nodeInformationDiv) {
        this.nodeInformationDiv = nodeInformationDiv;
        graph.nodeClickDiv = nodeInformationDiv;
        graph.nodeClickFunction = this.nodeInformationDivCreation;
    }
    this.drawDomainModel = function (divId) {
        var dm = new DomainModel(this.domainModelString);
        graph = new Graph(divId);
        /*add competences*/
        for (var i = 0; i < dm.competences.length; i++) {
            graph.addNode(dm.competences[i].id);
        }
        /*add edges*/
        for (var i = 0; i < dm.competences.length; i++) {
            for (var j = 0; j < dm.competences[i].to.length; j++) {
                graph.addEdge(dm.competences[i].id, dm.competences[i].to[j].id);
            }
        }
        graph.plotGraph();
        this.transitionprobability = dm.transitionprobability;
        this.graphWasDrawn = true;
    };
    this.colorNode = function (nodeElement, value) {
        if (value >= domainModelObject.transitionprobability) {
            nodeElement.setColor('lightgreen');
        } else {
            nodeElement.setColor('red');
        }

    }
    this.displayCompetenceState = function (competenceStateString) {
        if (!this.graphWasDrawn) {
            return;
        }
        this.competenceState = new CompetenceState(competenceStateString);
        for (var i = 0; i < this.competenceState.competencePairs.length; i++) {
            graph.colorNodeById(this.competenceState.competencePairs[i].id, this.competenceState.competencePairs[i].value, this.colorNode);
        }
        this.reprintNodeInformationDiv();
    };
    this.reprintNodeInformationDiv = function () {

    };
}

/**
 * Type representing a domain model - DomainModel.competences array holds all competences with competence.to (edges to succesors)
 * and competence.from (edge from prerequisite) and graphical information competence.x/competence.y as [0,1] number ((0,0) lower left corner)
 * @param {xml domain model string} domainModelString
 */
function DomainModel(domainModelString) {
    this.transitionprobability;
    this.competences = [];
    this.maxPathLength = -1;
    function Competence(id) {
        this.id = id;
        this.from=[];
        this.to = [];
        /*competence position in the graph*/
        this.x = -1;
        this.y = -1;
        /*identifies the max path length just going from competence to prerequisite or successor*/
        this.maxNumberOfPrerequisites=-1;
        this.maxNumberOfSuccessors=-1;
        this.getMaxNumberOfSuccessors = function () {
            if (this.maxNumberOfSuccessors == -1) {
                if (this.to.length == 0) {
                    this.maxNumberOfSuccessors = 0;
                } else {
                    var maxVal = 0;
                    for (var loop = 0; loop < this.to.length; loop++) {
                        var newVal = this.to[loop].getMaxNumberOfSuccessors();
                        if (newVal > maxVal)
                            maxVal = newVal;
                    }
                    this.maxNumberOfSuccessors = maxVal + 1;
                }
            }
            return this.maxNumberOfSuccessors;
        };
        this.getMaxNumberOfPrerequisites = function () {
            if (this.maxNumberOfPrerequisites == -1) {
                if (this.from.length == 0) {
                    this.maxNumberOfPrerequisites = 0;
                } else {
                    var maxVal = 0;
                    for (var loop = 0; loop < this.from.length; loop++) {
                        var newVal = this.from[loop].getMaxNumberOfPrerequisites();
                        if (newVal > maxVal)
                            maxVal = newVal;
                    }
                    this.maxNumberOfPrerequisites = maxVal + 1;
                }
            }
            return this.maxNumberOfPrerequisites;
        };
    };
    this.getCompetenceById = function (id) {
        for (var loopCounter = 0; loopCounter < this.competences.length; loopCounter++) {
            if (id == this.competences[loopCounter].id)
                return this.competences[loopCounter];
        }
        return null;
    };
    this.getCompetencesByMaxPrerequisiteNumber = function (maxPrerequisiteLength) {
        var requestedElements = [];
        for (var count = 0; count < this.competences.length; count++) {
            if (this.competences[count].maxPrerequisiteLength == maxPrerequisiteLength)
                requestedElements.push(this.competences[count]);
        }
        return requestedElements;
    };
    this.initialize = function (domainModelString) {
        var xmlDoc = $.parseXML(domainModelString);
        var $xml = $(xmlDoc);

        //store transition probability
        this.transitionprobability = parseFloat($xml.find("domainmodel")[0].getAttribute("transitionprobability"));

        /*Add competences*/
        var $competences = $xml.find("elements").find("competences").find("competence");
        for (var i = 0; i < ($competences).length; i++) {
            this.competences.push(new Competence($competences[i].getAttribute("id")));
        }

        /*Add competence links*/
        var $competenceLinks = $xml.find("relations").find("competenceprerequisites").find("competence");
        for (i = 0; i < ($competenceLinks).length; i++) {
            var to = $competenceLinks[i].getAttribute("id");
            var competenceTo = this.getCompetenceById(to);
            for (j = 0; j < $($competenceLinks[i]).find("prereqcompetence").length; j++) {
                var from = $($competenceLinks[i]).find("prereqcompetence")[j].getAttribute("id");
                var competenceFrom = this.getCompetenceById(from);
                competenceTo.from.push(competenceFrom);
                competenceFrom.to.push(competenceTo);
            }
        }

        /*Calculate position in graph representation*/
        //this.calculateCompetencePositionInGrapRepresentation();
    };
    this.calculateCompetencePositionInGrapRepresentation = function () {
        /*calculate basic values of the graph*/
        for (var i = 0; i < this.competences.length; i++) {
            var maxNumberOfSuccessors = this.competences[i].getMaxNumberOfSuccessors();
            this.competences[i].getMaxNumberOfPrerequisites();
            if (maxNumberOfSuccessors > this.maxPathLength)
                this.maxPathLength = maxNumberOfSuccessors;
        }
        /*determine y position (y=0..bottom,y=1..top)*/
        for (var i = 0; i < this.competences.length; i++) {
            var competence = this.competences[i];
            competence.y = (competence.maxPrerequisiteLength / this.maxPathLength);
        }
        /*determine x position (x=0..left,x=1..right)*/
        for (var i = 0; i <= this.maxPathLength; i++) {
            var competences = this.getCompetencesByMaxPrerequisiteNumber(i);
            if (competences.length == 0){
                continue;
            }else if (competences.length == 1) {
                competences[0].x = 0.5;
                continue;
            }
            var stepwidth = 1 / (competences.length - 1);
            for (var j = 0; j < competences.length; j++){
                competences[j].x = j * stepwidth;
            }
        }
    }
    this.initialize(domainModelString);
}

function CompetenceState(competenceStateString) {
    this.competencePairs = [];
    function CompetencePair(id,value) {
        this.id = id;
        this.value = value;
    };
    this.getValueById = function (id) {
        for (var i = 0; i < this.competencePairs.length; i++) {
            if (id == this.competencePairs[i].id)
                return this.competencePairs[i].value;
        }
        return null;
    }
    this.initialize = function (competenceStateString) {
        var xmlDoc = $.parseXML(competenceStateString);
        var $xml = $(xmlDoc);

        /*Add competences/probabilities*/
        var $competences = $xml.find("competence");
        for (var i = 0; i < ($competences).length; i++) {
            var $competence = $($competences[i]);
            this.competencePairs.push(new CompetencePair($competence.find("id").text(), $competence.find("probability").text()));
        }
    };
    this.initialize(competenceStateString);
}


function UpdateHistory(updateHistoryXml) {
    this.trackingid;
    this.entries = [];
    this.initialize = function (updateHistoryxml) {
        var xmlDoc = $.parseXML(updateHistoryxml);
        var $xml = $(xmlDoc);

        //store transition probability
        this.trackingid = parseFloat($xml.find("updatehistory")[0].getAttribute("trackingid"));

        /*Add competences*/
        var $entries = $xml.find("updatehistoryentry");
        for (var i = 0; i < ($entries).length; i++) {
            this.entries.push(new UpdateHistoryEntry($entries[i]));
        }
    };
    this.initialize(updateHistoryXml);


    function UpdateHistoryEntry(UpdateHistoryEntryXml) {
        this.datetime;
        this.evidenceSet;
        this.competencestate;
        this.competenceStateString;
        this.initialize = function (UpdateHistoryEntryxml) {
            
            this.datetime = UpdateHistoryEntryxml.getAttribute("datetimeentry");
            this.evidenceSet = new EvidenceSet($(UpdateHistoryEntryxml).find("evidenceentry").html().replace(/&lt;/g, "<").replace(/&gt;/g, ">"));
            this.competenceStateString = $(UpdateHistoryEntryxml).find("competencestateentry").html().replace(/&lt;/g, "<").replace(/&gt;/g, ">");
            this.competencestate = new CompetenceState(this.competenceStateString); 
        };
        this.initialize(UpdateHistoryEntryXml);
    }

    function EvidenceSet(dataInput) {
        this.initial = false;
        this.evidences=[];
        this.initialize = function (data) {
            if (data == "initial Competencestate") {
                this.initial = true;
                return;
            }
            
            var xmlDoc = $.parseXML(data);
            var $xml = $(xmlDoc);
            var elements = $xml.find("evidence");
            for (var i = 0; i < elements.length; i++) {
                this.evidences.push(new Evidence(elements[i]));
            }
        };
        this.initialize(dataInput);
        this.getDisplayText = function () {
            if (this.initial) {
                return "initial state";
            }
            var type = this.evidences[0].type;
            for (var i = 0; i < this.evidences.length; i++) {
                if (type != this.evidences[i].type) {
                    return "mixed update";
                }
            }
            if (type == "Competence") {
                var competences = ""
                for (var i = 0; i < this.evidences.length; i++) {
                    if (i != 0)
                        competences += ",";
                    competences+="(" + this.evidences[i].id + "," + this.evidences[i].getUpdateInfoToDisplay()+")";
                }
                return "Competence update ["+competences+"]";
            }
            if (type == "Activity") {
                var activities = []
                for (var i = 0; i < this.evidences.length; i++) {
                    activities.push(this.evidences[i].id);
                }
                return "Activity update [" + activities.toString() + "]";
            }
            if (type == "Gamesituation") {
                var gs = ""
                for (var i = 0; i < this.evidences.length; i++) {
                    ga += "(" + this.evidences[i].id + "," + this.evidences[i].getUpdateInfoToDisplay() + ")";
                }
                return "Gamesituation update [" + competences + "]";
            }
            return "Update unknown";
        }
    }

    function Evidence(dataInput) {
        this.type;
        this.id;
        this.power;
        this.direction;
        this.initialize = function (data) {

            this.type = $(data).find("type").html();
            if (this.type == "Competence") {
                this.id = $(data).find("id").html();
                this.power = $(data).find("power").html();
                this.direction = $(data).find("direction").html();
            } else if (this.type == "Activity") {
                this.id = $(data).find("id").html();
            } else if (this.type == "Gamesituation") {
                this.id = $(data).find("id").html();
                this.direction = $(data).find("direction").html();
            }
        };
        this.initialize(dataInput);
        this.getUpdateInfoToDisplay = function(){
            if (this.type == "Gamesituation") {
                if (this.direction == "true")
                    return "+";
                else
                    return "-";
            } else if (this.type == "Competence") {
                var sign = (this.direction == "true") ? "+" : "-";
                if (this.power == "Low") {
                    return sign;
                } else if (this.power == "Medium") {
                    return sign+sign;
                } else if (this.power == "High") {
                    return sign+sign+sign;
                }
            }
        };
    }
}