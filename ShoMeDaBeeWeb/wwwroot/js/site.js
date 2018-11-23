"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dabeehub").build();
var lastEventContent = "";

connection.on("Reset", function (contextName) {
    addEvent("Session starting for" + contextName);
    document.getElementById("mainTitle").textContent = "DbContext Tracking Visualizer: " + contextName;
});

connection.on("AddEvent", function (message) {
    addEvent(message);
});

connection.on("PatchEvent", function (message) {
    patchEvent(message);
});

connection.on("UpdateTracking", function (tracked) {
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

function addEvent(message) {
    lastEventContent = eventLog.textContent;
    patchEvent(message);
}

function patchEvent(message) {
    var eventLog = document.getElementById("eventLog");
    eventLog.textContent = (lastEventContent + message + '\r\n');
    eventLog.scrollTop = eventLog.scrollHeight;
}
