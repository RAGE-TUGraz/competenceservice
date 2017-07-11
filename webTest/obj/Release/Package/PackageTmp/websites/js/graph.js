//sign(0)=1
function sign(x) { return x ? x < 0 ? -1 : 1 : 1; }
//instance has be be named graph!
function Graph(div) {
    this.size = 1;
    this.colorNodeById = function (id, val, colorFunction) {
        colorFunction(this.getNodeById(id), val);
        //this.getNodeById(id).elementCircle.node.setAttribute('fill', color);
    };
    this.nodeClickFunction = null;
    this.nodeClickDiv;
    this.div = div;
    this.paper;
    this.highlightedNode;
    this.lastTimeNodeClicked = 0;
    this.paperHeight;
    this.paperWidth;
    this.mousedownNodeId;
    this.arrowPeakLength = 7 * this.size;
    this.arrowPeakAngle = Math.PI / 12;
    this.isDirected = true;
    this.nodes = [];
    this.maxPathLength = -1;
    this.addNode = function (nodeId) {
        if (!this.doesNodeIdExists(nodeId))
            this.nodes.push(new Node(nodeId));
    };
    this.deleteNode = function (nodeId) {
        var node = this.getNodeById(nodeId);
        for (var i = 0; i < node.edges.length; i++)
            this.deleteEdge(node.edges[i].from.id, node.edges[i].to.id);
        var pos = -1;
        for (var i = 0; i < this.nodes.length; i++) {
            if (this.nodes[i].id == nodeId) {
                pos = i;
                break;
            }
        }
        if (pos != -1)
            this.nodes.splice(pos, 1);
    };
    this.addEdge = function (fromId, toId) {
        fromNode = this.getNodeById(fromId);
        toNode = this.getNodeById(toId);
        if (!fromNode.hasEdgeTo(toNode)) {
            edge = new Edge(fromNode, toNode);
            fromNode.addEdge(edge);
            toNode.addEdge(edge);
        }

    };
    this.deleteEdge = function (fromId, toId) {
        fromNode = this.getNodeById(fromId);
        toNode = this.getNodeById(toId);
        fromNode.deleteEdgeFromThisNodeToNode(toNode);
    };
    this.getNodeById = function (nodeId) {
        var pos = -1;
        for (var i = 0; i < this.nodes.length; i++) {
            if (this.nodes[i].id == nodeId) {
                pos = i;
                break;
            }
        }
        if (pos == -1)
            return (null);
        else
            return (this.nodes[pos]);
    };
    this.getCompetencesByMaxPrerequisiteNumber = function (maxPrerequisiteLength) {
        var requestedElements = [];
        for (var count = 0; count < this.nodes.length; count++) {
            if (this.nodes[count].maxNumberOfPrerequisites == maxPrerequisiteLength) {
                requestedElements.push(this.nodes[count]);
            }
        }
        return requestedElements;
    };
    this.doesNodeIdExists = function (nodeId) {
        var node = this.getNodeById(nodeId);
        if (node == null)
            return (false);
        else
            return (true);
    };
    this.setPaperDimensions = function () {
        var element = document.getElementById(this.div);
        var rect = element.getBoundingClientRect();
        this.paperHeight = (rect.bottom - rect.top);
        this.paperWidth = (rect.right - rect.left);
    };
    this.setPaperCanvas = function () {
        if (this.paper != null)
            this.paper.remove();
        var element = document.getElementById(this.div);
        this.paper = Raphael(element, this.paperWidth, this.paperHeight);
        //this.paper.canvas.style.backgroundColor = '#F00';
    };
    this.setNodeCoordinates = function () {

        //new set basic values for position calculation
        for (var i = 0; i < this.nodes.length; i++) {
            var newPathLength = this.nodes[i].getMaxNumberOfPrerequisites();
            if (newPathLength > this.maxPathLength)
                this.maxPathLength = newPathLength;
        }
        //calculate x position (x=0..left,x=1..right)
        for (var i = 0; i <= this.maxPathLength; i++) {
            var nodes = this.getCompetencesByMaxPrerequisiteNumber(i);
            if (i == 0) {
                if (nodes.length == 0) {
                    continue;
                } else if (nodes.length == 1) {
                    nodes[0].x = 0.5;
                    continue;
                }
                var stepwidth = 1 / (nodes.length - 1);
                for (var j = 0; j < nodes.length; j++) {
                    nodes[j].x = j * stepwidth;
                }
            } else {
                for (var j = 0; j < nodes.length; j++) {
                    var sum = 0;
                    var prerequisites = nodes[j].getPrerequisites();
                    for (var k = 0; k < prerequisites.length; k++) {
                        sum = sum + prerequisites[k].x;
                    }
                    nodes[j].x = sum / prerequisites.length;
                }
            }

        }
        //determine y position (y=0..bottom,y=1..top)
        for (var i = 0; i < this.nodes.length; i++) {
            var competence = this.nodes[i];
            competence.y = (competence.maxNumberOfPrerequisites / this.maxPathLength);
        }

        //shift coordinates to the middle
        for (var i = 0; i < this.nodes.length; i++) {
            var node = this.nodes[i];
            var factor = 0.85;
            node.x = ((1 - factor) / 2) + node.x * factor;
            node.y = ((1 - factor) / 2) + node.y * factor;
        }
    };
    this.getProperEdgeReductionX = function (radius, deltaX, deltaY) {
        if (deltaX == 0)
            return (0);
        var alpha = Math.atan(deltaY / deltaX);
        var reductionX = radius * Math.cos(alpha);
        return (reductionX);
    };
    this.plotGraph = function () {
        this.setPaperDimensions();
        this.setPaperCanvas();
        this.setNodeCoordinates();
        //print edges
        for (var i = 0; i < this.nodes.length; i++) {
            var node = this.nodes[i];
            for (var j = 0; j < node.edges.length; j++) {
                var edge = node.edges[j];
                edge.draw(node.id);
            }
        }
        //print nodes
        for (var i = 0; i < this.nodes.length; i++) {
            var node = this.nodes[i];
            node.draw();
        }

    };
    this.drawArrowPeak = function (peakX, peakY, shaftX, shaftY) {

        var alpha = this.arrowPeakAngle;
        var length = this.arrowPeakLength;
        var arrowPeakHight = Math.cos(alpha) * length;

        var arrowLength = Math.sqrt((peakX - shaftX) * (peakX - shaftX) + (peakY - shaftY) * (peakY - shaftY));
        var deltaXMirrow = arrowPeakHight * ((peakX - shaftX) / arrowLength);
        var deltaYMirrow = arrowPeakHight * ((peakY - shaftY) / arrowLength);
        var mirrorPointX = peakX - deltaXMirrow;
        var mirrorPointY = peakY - deltaYMirrow;


        var beta = Math.atan((peakX - shaftX) / (peakY - shaftY));
        var gamma = sign(beta) * beta - alpha;
        var dy = Math.cos(gamma) * length;
        var dx = Math.sin(gamma) * length;
        var n1x = peakX + dx * sign(-(peakX - shaftX));
        var n1y = peakY + dy * sign(-(peakY - shaftY));
        var n2x = mirrorPointX + (mirrorPointX - n1x);
        var n2y = mirrorPointY + (mirrorPointY - n1y);

        var peak = this.paper.path("M " + n1x + " " + n1y + " L " + n2x + " " + n2y + " L " + peakX + " " + peakY + " z")
            .attr({ fill: "black" });
        return (peak);
    };
    this.exampleFill = function () {
        this.addNode("C1");
        this.addNode("C2");
        this.addNode("C3");
        this.addNode("C4");
        this.addNode("C5");
        this.addEdge("C1", "C3");
        this.addEdge("C2", "C3");
        this.addEdge("C3", "C5");
        this.addEdge("C4", "C5");
    };
    this.dragStart = function () {
        this.ox = this.attr("x");
        this.oy = this.attr("y");

        this.ldx = this.attr("x");
        this.ldy = this.attr("y");
    };
    this.dragMove = function (dx, dy) {
        this.attr({ x: this.ox + dx, y: this.oy + dy });

        /*
	    if (Math.sqrt(((this.attr("x") - this.ldx) / graph.paperWidth) * ((this.attr("x") - this.ldx) / graph.paperWidth) + ((this.attr("y") - this.ldy) / graph.paperHeight) * ((this.attr("y") - this.ldy) / graph.paperHeight)) > 0.3) {
	        //graph.shiftNode(this.data("id"), this.attr("x") - this.ox, -(this.attr("y") - this.oy));
	        this.ldx = this.attr("x");
	        this.ldy = this.attr("y");

	        graph.getNodeById(this.data("id")).elementCircle.attr({ x: this.ox + dx, y: this.oy + dy })

	        //alert(this.data("id"));
	        //alert("shift id " + this.data("id") + " to (" + (this.attr("x") - this.ox) + "/" + (-(this.attr("y") - this.oy))+")");
	    }
        */

    };
    this.dragStop = function () {
        if (this.attr("x") == this.ox && this.attr("y") == this.oy)
            graph.nodeClicked();
        else
            graph.shiftNode(this.attr("id"), this.attr("x") - this.ox, -(this.attr("y") - this.oy));
    };
    this.shiftNode = function (nodeId, dx, dy) {
        var node = this.getNodeById(this.mousedownNodeId);
        node.x += dx / this.paperWidth;
        node.y += dy / this.paperHeight;
        node.redraw();
    };
    this.nodeClicked = function () {
        this.size = this.size * 2;
        this.redraw();
        if (this.nodeClickFunction == null)
            return;
        else if (this.lastTimeNodeClicked + 500 < performance.now()) {
            this.lastTimeNodeClicked = performance.now();
            $("#" + this.nodeClickDiv).html(this.nodeClickFunction(this.mousedownNodeId));
            this.highlightNode(this.mousedownNodeId);

        }
    }
    this.highlightNode = function (nodeId) {
        if (this.highlightedNode)
            this.highlightedNode.unhighlightNode();
        this.highlightedNode = this.getNodeById(nodeId);
        this.highlightedNode.highlightNode();
    };
    this.changeGraphSize = function (value) {
        this.size = value;
        this.arrowPeakLength = 7 * value;
        for (var i = 0; i < this.nodes.length; i++) {
            this.nodes[i].radius = 0.05 * value
        }
        this.redraw();
    };
    this.redraw = function () {
        for (var i = 0; i < this.nodes.length; i++) {
            this.nodes[i].redraw();
        }
    };


    function Node(id) {
        this.id = id;
        this.color = "none";
        this.elementSet;
        this.elementCircle;
        this.elementLable;
        this.x;
        this.y;
        this.radius = 0.05 * graph.size;
        this.edges = [];
        this.hasEdgeTo = function (node) {
            for (var i = 0; i < this.edges.length; i++) {
                if (this.edges[i].to.id == node.id)
                    return (true)
            }
            return (false);
        };
        this.getPrerequisites = function () {
            var prerequisites = [];
            for (var i = 0; i < this.edges.length; i++) {
                if (this.edges[i].to.id == this.id) {
                    prerequisites.push(this.edges[i].from);
                }
            }
            return prerequisites;
        };
        this.addEdge = function (edge) {
            this.edges.push(edge);
        };
        this.deleteEdgeFromThisNodeToNode = function (node) {
            var pos = -1;
            for (var i = 0; i < this.edges.length; i++) {
                if (this.edges[i].to.id == node.id && this.edges[i].from.id == this.id) {
                    pos = i;
                    break;
                }
            }
            if (pos != -1)
                this.edges.splice(pos, 1);
            var pos = -1;
            for (var i = 0; i < node.edges.length; i++) {
                if (node.edges[i].to.id == node.id && node.edges[i].from.id == this.id) {
                    pos = i;
                    break;
                }
            }
            if (pos != -1)
                node.edges.splice(pos, 1);
        };
        this.redraw = function () {
            this.elementCircle.remove();
            this.elementLable.remove();
            for (var i = 0; i < this.edges.length; i++) {
                this.edges[i].elementShaft.remove();
                if (this.edges[i].elementPeak)
                    this.edges[i].elementPeak.remove();
            }
            this.draw();
            for (var i = 0; i < this.edges.length; i++) {
                this.edges[i].draw();
            }
        };
        this.draw = function () {
            this.elementSet = graph.paper.set();
            var el = graph.paper.circle(this.x * graph.paperWidth, (1 - this.y) * graph.paperHeight, this.radius * Math.min(graph.paperWidth, graph.paperHeight))
		    /*.attr({ fill: "#0FF" })
            .data("id", node.id)
            .mousedown(function () {
                graph.nodeMousedown(this.data("id"));
            })
            .click(function () {
                graph.nodeClicked(this.data("id"));
            })*/;
            this.elementCircle = el;
            el.node.setAttribute('fill', this.color);
            var txt = graph.paper.text(this.x * graph.paperWidth, (1 - this.y) * graph.paperHeight, this.id.substring(0, 3))
                .data("id", this.id)
                .drag(graph.dragMove, graph.dragStart, graph.dragStop)
                .mousedown(function () {
                    graph.mousedownNodeId = this.data("id");
                })
                .click(function () {
                    graph.nodeClicked(this.data("id"));
                });
            txt.hover(function () { txt.attr({ 'cursor': 'pointer' }) },
                function () { txt.attr({ 'cursor': 'default' }) }, txt, txt);

            this.elementLable = txt;
            this.elementSet.push(el);
            this.elementSet.push(txt);
        };
        /*identifies the max path length just going from competence to prerequisite or successor*/
        this.maxNumberOfPrerequisites = -1;
        this.getMaxNumberOfPrerequisites = function () {
            if (this.maxNumberOfPrerequisites == -1) {
                prerequisites = this.getPrerequisites();
                if (prerequisites.length == 0) {
                    this.maxNumberOfPrerequisites = 0;
                } else {
                    var maxVal = 0;
                    for (var loop = 0; loop < prerequisites.length; loop++) {
                        var newVal = prerequisites[loop].getMaxNumberOfPrerequisites();
                        if (newVal > maxVal)
                            maxVal = newVal;
                    }
                    this.maxNumberOfPrerequisites = maxVal + 1;
                }
            }
            return this.maxNumberOfPrerequisites;
        };
        this.setColor = function (color) {
            this.color = color;
            this.redraw();
        };
        this.highlightNode = function () {
            this.radius = this.radius * 1.5;
            this.redraw();
        };
        this.unhighlightNode = function () {
            this.radius = this.radius / 1.5;
            this.redraw();
        };
    };

    function Edge(from, to) {
        this.elementShaft;
        this.elementPeak;
        this.from = from;
        this.to = to;
        this.draw = function (nodeId) {
            if (nodeId == null || this.from.id == nodeId) {
                var fromX = this.from.x * graph.paperWidth;
                var fromY = (1 - this.from.y) * graph.paperHeight;
                var toX = this.to.x * graph.paperWidth;
                var toY = (1 - this.to.y) * graph.paperHeight;
                var radiusFrom = this.from.radius * Math.min(graph.paperWidth, graph.paperHeight);
                var radiusTo = this.to.radius * Math.min(graph.paperWidth, graph.paperHeight);

                if (toX - fromX != 0 && toY - fromY != 0) {
                    var reductionXFrom = graph.getProperEdgeReductionX(radiusFrom, toX - fromX, toY - fromY);
                    var reductionYFrom = reductionXFrom * ((toY - fromY) / (toX - fromX)) * sign(-(toY - fromY));
                    var reductionXTo = graph.getProperEdgeReductionX(radiusTo, toX - fromX, toY - fromY);
                    var reductionYTo = reductionXTo * ((toY - fromY) / (toX - fromX)) * sign(-(toY - fromY));
                    fromX += reductionXFrom * sign((toX - fromX));
                    fromY += reductionYFrom * sign((toY - fromY)) * sign(-(toX - fromX));
                    toX -= reductionXTo * sign((toX - fromX));
                    toY -= reductionYTo * sign((toY - fromY)) * sign(-(toX - fromX));
                } else if (toX - fromX == 0) {
                    fromY += radiusFrom * sign((toY - fromY));
                    toY += radiusTo;
                } else {
                    fromX += radiusFrom * sign((toX - fromX));
                    toX += radiusTo * sign(-(toX - fromX));
                }
                this.elementShaft = graph.paper.path("M" + fromX + "," + fromY + "L" + toX + "," + toY);
                if (graph.isDirected) {
                    this.elementPeak = graph.drawArrowPeak(toX, toY, fromX, fromY);
                }
            }
        }
    };
}
