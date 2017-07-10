$(document).ready(function () {
    var active = $('input[name=method]:checked').val();
    handleRBClick(document.getElementById(active));
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

