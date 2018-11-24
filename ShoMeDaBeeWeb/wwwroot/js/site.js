"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dabeehub").build();
var lastEventContent = "";
var trackedEntities = {};

connection.on("Reset", function (contextName) {
    addEvent("Session starting for " + contextName);
    document.getElementById("mainTitle").textContent = "DbContext Tracking Visualizer: " + contextName;
});

connection.on("AddEvent", function (message) {
    addEvent(message);
});

connection.on("PatchEvent", function (message) {
    patchEvent(message);
});

connection.on("Track", function (entityEntry) {
    var entityTypeName = entityEntry.entityTypeName;

    if (trackedEntities[entityTypeName] == null) {
        trackedEntities[entityTypeName] = true;
        createTrackingBox(entityTypeName);
    }

    var entityList = document.getElementById("entities:" + entityTypeName);
    var entityDiv = document.createElement("div");
    entityDiv.textContent = "Key: " + entityEntry.keyString + " State: " + entityEntry.stateString;
    entityDiv.id = entityEntry.trackingGuid;
    entityList.appendChild(entityDiv);
});

connection.on("Untrack", function (entityEntry) {
});

connection.on("Update", function (entityEntry) {
    var entityDiv = document.getElementById(entityEntry.trackingGuid);
    entityDiv.textContent = "Key: " + entityEntry.keyString + " State: " + entityEntry.stateString;
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

function createTrackingBox(entityTypeName) {
    var entityTypeHeader = document.createElement("th");
    entityTypeHeader.style = "width: 250px; padding: 5px";
    entityTypeHeader.textContent = "Entity type: " + entityTypeName;

    var entityList = document.createElement("td");
    entityList.style = "padding-left: 15px";
    entityList.id = "entities:" + entityTypeName;

    var headerRow = document.createElement("tr");
    headerRow.appendChild(entityTypeHeader);

    var entitiesRow = document.createElement("tr");
    entitiesRow.appendChild(entityList);

    var innerTable = document.createElement("table");
    innerTable.style = "border: 1px solid #ccc; background: white";
    innerTable.appendChild(headerRow);
    innerTable.appendChild(entitiesRow);

    var outerTd = document.createElement("td");
    outerTd.style = "width: 25%; padding: 5px; vertical-align: top;";
    outerTd.appendChild(innerTable);

    var trackingList = document.getElementById("trackingList");
    if (trackingList.childElementCount == 0) {
        var outerTr = document.createElement("tr");
        outerTr.appendChild(outerTd);
        trackingList.appendChild(outerTr);
    } else {
        trackingList.lastChild.appendChild(outerTd);
    }
}

function addEvent(message) {
    lastEventContent = document.getElementById("eventLog").textContent;
    patchEvent(message);
}

function patchEvent(message) {
    var eventLog = document.getElementById("eventLog");
    eventLog.textContent = (lastEventContent + message + '\r\n');
    eventLog.scrollTop = eventLog.scrollHeight;
}
