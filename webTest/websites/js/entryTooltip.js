getDescription = function (type) {
    if (type == "buttonloadenterdm") {
        return "Description on what to do in 'Enter Domainmodel'";
    } else if (type == "buttonviewdomainmodel") {
        return "Description on what to do in 'View Domainmodel'";
    } else if (type == "buttonviewcompetencestate") {
        return "Description on what to do in 'View Competencestate'";
    } else if (type == "buttonentertestdata") {
        return "Description on what to do in 'Enter Testdata'";
    }
    if (type == null) {
        return "General description.";
    }
    return "";
}

$(document).ready(function () {
    document.getElementById("infoDiv").innerHTML = getDescription();

    $(".rectangularbutton").hover(function () {
        document.getElementById("infoDiv").innerHTML = getDescription(this.id);
    }, function () {
        document.getElementById("infoDiv").innerHTML = getDescription();
    });
});