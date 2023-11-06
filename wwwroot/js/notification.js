"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start().then(() => {
    console.log("SignalR Hub connected");
}).catch(function (err) {
    return console.error(err.toString());
});

const notificationList = document.getElementById("notificationList");
const notificationBadge = document.getElementById("notificationBadge");

connection.on("ReceiveJobNotification", (notification) => {
    console.log("notification: " + notification);
    notification = JSON.parse(notification);
    if (notification.URL && notification.ObjectId) {
        console.log("Sending notification");
        var li = document.createElement("li");
        var a = document.createElement("a");

        a.href = notification.URL + "?id=" + notification.ObjectId;
        a.innerHTML = notification.Message;

        li.appendChild(a);

        notificationList.appendChild(li);

        const currentCount = parseInt(notificationBadge.innerHTML);
        notificationBadge.innerHTML = (currentCount + 1).toString();
        notificationBadge.style.display = "inline";
    } else {
        console.log("Notification could not send.");
    }
});

connection.on("ReceiveNoticeNotification", (notification) => {
    console.log("notification: " + notification);
    notification = JSON.parse(notification);
    if (notification.URL && notification.ObjectId) {
        console.log("Sending notification");
        var li = document.createElement("li");
        li.className = "dropdown-item";
        var a = document.createElement("a");

        a.href = notification.URL + "?id=" + notification.ObjectId;
        a.innerHTML = notification.Message;

        li.appendChild(a);

        notificationList.appendChild(li);

        const currentCount = parseInt(notificationBadge.innerHTML);
        notificationBadge.innerHTML = (currentCount + 1).toString();
        notificationBadge.style.display = "inline";
    } else {
        console.log("Notification could not send.");
    }
});