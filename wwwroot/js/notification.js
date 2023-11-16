"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start().then(() => {
}).catch(function (err) {
    return console.error(err.toString());
});

const notificationList = document.getElementById("notificationList");
const notificationBadge = document.getElementById("notificationBadge");

connection.on("ReceiveJobNotification", (notification) => {
    notification = JSON.parse(notification);
    if (notification.URL && notification.ObjectId) {
        var li = document.createElement("li");
        var a = document.createElement("a");

        li.className = "dropdown-item";

        a.href = notification.URL + "?id=" + notification.ObjectId;
        a.innerHTML = notification.Message;

        li.appendChild(a);

        notificationList.appendChild(li);

        const currentCount = parseInt(notificationBadge.innerHTML);
        notificationBadge.innerHTML = (currentCount + 1).toString();
    } else {
        console.log("Notification could not send.");
    }
});