loadClicked = function () {
    var url = document.getElementById('url').value;
    var user = document.getElementById('user').value;
    var password = document.getElementById('password').value;
    var tid = document.getElementById('tid').value;
    var html = getHtmlData(url, user, password, tid);

    document.getElementById('visual').innerHTML = html;
}

getHtmlData = function (url, user, password, tid) {
    //var html = "<h3>Result:</h3>";
    //html += url + "," + user + "," + password + "," + tid;
    var link = url + "/rest/competenceservice/getcompetencestatehtml/" + tid
    var client = new XMLHttpRequest();
    client.open("GET", link, false);
    //client.open("GET", link, false, user, password);
    client.setRequestHeader("Content-Type", "text/plain");
    var encodedpwd = btoa(text2Binary(user + ":" + password));
    //client.setRequestHeader("Authorization", "Authorization");
    client.send();
    if (client.status == 200)
        html = "The request succeeded!\n\nThe response representation was:\n\n" + client.responseText;
    else
        html = "The request did not succeed!\n\nThe response status was: " + client.status + " " + client.statusText + ".";

    return html;
}

function text2Binary(string) {
    return string.split('').map(function (char) {
        return char.charCodeAt(0).toString(2);
    }).join(' ');
}