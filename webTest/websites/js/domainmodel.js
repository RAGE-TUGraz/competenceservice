drawDomainModel = function (domMod) {
    domainModelObject = new DomainModelObject(domMod);
    domainModelObject.drawDomainModel("graph");
    domainModelObject.displayNodeInformationOnClick("nodeInfo");
    domainModelObject.displayCompetenceState(cs);
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
        element.css("border-color", "black");
        element.css("border", "solid");

        //get competence information by id
        var html = "";
        html += "<font size='1'>Competence: &nbsp &nbsp</font><font size='5'> " + nodeId.toUpperCase() + "</font><br>";
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
