getDescription = function (type) {
    var html = "<h2>Description</h2>";
    if (type == "buttonloadenterdm") {
        html += "<p>";
        html += "This sections allows you to create a domainmodel and to store it in the system.";
        html += "</p>";
    } else if (type == "buttonviewdomainmodel") {
        html += "<p>";
        html += "In this section you can have a look at stored domainmodels. The appropriate domainmodel id is needed.";
        html += "</p>";
    } else if (type == "buttonviewcompetencestate") {
        html += "<p>";
        html += "In this section you can have a look at the current competence state and also the competence developement of a learner. The appropriate tracking id is needed.";
        html += "</p>";
    } else if (type == "buttonentertestdata") {
        html += "<p>";
        html += "This section provides an overview about the system's functionality. Furthermore, it is possible to directly use the system's functionality.";
        html += "</p>";
    } else if (type == null) {
        html += "<p>";
        html += "This system allows you to easily incorporate competence assessment and adaptation functionality into your software. Please visit <a href=\"http://css-kti.tugraz.at/projects/rage/assets/\">this website</a> for more information.";
        html += "</p>";
    } else {
        html = "<h2>ERROR: type unknown.</h2>";
    }
    return html;
}

$(document).ready(function () {
    document.getElementById("infoDiv").innerHTML = getDescription();

    $(".rectangularbutton").hover(function () {
        document.getElementById("infoDiv").innerHTML = getDescription(this.id);
    }, function () {
        document.getElementById("infoDiv").innerHTML = getDescription();
    });
});