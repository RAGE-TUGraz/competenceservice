 $(document).ready(function () {
    //alert(pageXOffset);
    activeMethod = localStorage.getItem("activeMethod");
    if (activeMethod == null)
        activeMethod = "postdm";
    //localStorage.setItem("activeMethod", "postdm");
    
    handleRBClick(document.getElementById(activeMethod));
    $('#' + activeMethod).prop("checked", true);
    selectChange("competence");
});

showMethodDiv = function (id) {
    var elements = document.getElementsByClassName('methoddiv');
    for (var i = 0; i < elements.length; i++) {
        if (elements[i].id == id) {
            elements[i].style.display = 'block';
        } else {
            elements[i].style.display = 'none';
        }
    }
}

handleRBClick = function (myRB) {
    localStorage.setItem("activeMethod", myRB.value);
    showMethodDiv(myRB.value + "div");
    displayMethodInfo(myRB.value);
}

displayMethodInfo = function (method) {
    var html = "";
    html += "<h3>Method description</h3>"
    html += "<p>";
    if (method == "postdm") {
        html += "Used to store a domain model in the database. Returns a domain model id.";
    } else if (method == "getdm") {
        html += "Used to receive a stored domain model from the database with id {dmid}.";
    } else if (method == "deletedm") {
        html += "Used to delete a stored domain model from the database with id {dmid}. ";
        html += "Also deletes all tracking id's for this domain model.";
    } else if (method == "gettid") {
        html += "Used to create, store, and receive a tracking id to a stored domainmodel with id {dmid}. Returns a tracking id.";
        html += " This tracking id corresponds to a competence state, also stored in the database.";
    } else if (method == "postcu") {
        html += "Used to update a competence state identified by a tracking id, {tid}.";
    } else if (method == "deletetid") {
        html += "Used to delete a tracking id {tid} from the database. Also deletes all related competence state information from the database. ";
    } else if (method == "getcs") {
        html += "Used to receive the current competence state identified by the tracking id {tid} from the database.";
    }
    html += "</p>";
    document.getElementById('methodinfo').innerHTML = html;
}

selectChange = function (val) {
    var html = "";
    if (val == "competence") {
        html += "<table><tr><td>";
        html += "Competence id:";
        html += "</td><td>";
        html += "<input type='text' id='textfieldid' value='Enter Id here!'>";
        html += "</td></tr><tr><td>";
        html += "Direction:";
        html += "</td><td>";
        html += "<select id='selectdirection'><option value='t'>True</option><option value='f'>False</option></select>";
        html += "</td></tr><tr><td>";
        html += "Power:";
        html += "</td><td>";
        html += "<select id='selectpower'><option value='l'>Low</option><option value='m'>Medium</option><option value='h'>High</option></select>";
        html += "</td></tr></table>";
    } else if (val == "activity") {
        html += "<p>Activity id: &nbsp; &nbsp; &nbsp; <input type='text' id='textfieldid' value='Enter Id here!'></p>";
    } else if (val == "gamesituation") {
        html += "<table><tr><td>";
        html += "Gamesituation id:";
        html += "</td><td>";
        html += "<input type='text' id='textfieldid' value='Enter Id here!'>";
        html += "</td></tr><tr><td>";
        html += "Direction:";
        html += "</td><td>";
        html += "<select id='selectdirection'><option value='t'>True</option><option value='f'>False</option></select>";
        html += "</td></tr></table>";
    }
    html += "<p> <button class='button' type='button' onclick=\"submitEvidence('" + val + "');\">Add Evidence</button></p> ";

    document.getElementById('updatecreationdiv').innerHTML = html;
}

submitEvidence = function (val) {
    var xml = "<evidence>";
    var id = document.getElementById('textfieldid').value;
    if (val == "competence") {
        //add type
        xml += "<type>Competence</type>";
        //add id
        xml += "<id>"+id+"</id>";
        //add direction
        var e = document.getElementById("selectdirection");
        var direction = e.options[e.selectedIndex].value;
        if (direction == 't')
            xml += "<direction>true</direction>";
        else if(direction == 'f')
            xml += "<direction>false</direction>";
        //add power
        var e = document.getElementById("selectpower");
        var power = e.options[e.selectedIndex].value;
        if (power == 'l')
            xml += "<power>Low</power>";
        else if (power == 'm')
            xml += "<power>Medium</power>";
        else if (power == 'h')
            xml += "<power>High</power>";

    } else if (val == "activity") {
        //add type
        xml += "<type>Activity</type>";
        //add id
        xml += "<id>" + id + "</id>";

    } else if (val == "gamesituation") {
        //add type
        xml += "<type>Gamesituation</type>";
        //add id
        xml += "<id>" + id + "</id>";
        //add direction
        var e = document.getElementById("selectdirection");
        var direction = e.options[e.selectedIndex].value;
        if (direction == 't')
            xml += "<direction>true</direction>";
        else if (direction == 'f')
            xml += "<direction>false</direction>";
    }
    xml += "</evidence>";
    addEvidence(xml);
}

addEvidence = function (xml) {
    var element = document.getElementById('updatecsxml');
    if (element.value == "") {
        element.value = "<evidenceset>" + xml +"</evidenceset>";
    } else if (element.value.indexOf("<evidenceset>")>=0) {
        var pos = element.value.indexOf("<evidenceset>");
        var before = element.value.substring(0, pos);
        var after = element.value.substring(pos + 13, element.value.length - pos);
        alert(after);
        element.value = before + "<evidenceset>" + xml + after;
    } else {
        alert("Already entered evidenceset data not readable!");
    }
}