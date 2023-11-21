function fetchUserNotifications() {
    $.getJSON(`/api/Notifications`, function (data) {
        if (data === null || !Array.isArray(data)) {
            console.log("Notifications are null or not an array");
            return;
        }
        updateNotificationBadge(data);
        updateNotificationList(data);
    })
    .fail(function (error) {
        console.error('Error fetching notifications: ', error);
    });
}

// Function to update the notification badge
function updateNotificationBadge(notifications) {
    const notificationBadge = document.getElementById("notificationBadge");

    if (Object.values(notifications).some(n => n.isRead === false)) {
        console.log("ayyyy");
        notificationBadge.style.display = "inline-block";
    } else {
        console.log("there we go");
        notificationBadge.style.display = "none";
    }
}

// Function to update the notification list on the page
function updateNotificationList(notifications) {
    const notificationWrapper = document.getElementById("notificationWrapper");    

    notifications.forEach(n => {
        var notification = createNotification(n);        
        notificationWrapper.appendChild(notification);
    });
}

function createNotification(n) {

    var card = document.createElement("div");

    if (n.isRead) {
        card.className = "card notification-read";
    } else {
        card.className = "card";
    }

    var cardBody = document.createElement("div");
    cardBody.className = "card-body";

    var notificationURL = document.createElement("a");
    notificationURL.href = n.url + "?id=" + n.objectId;
    notificationURL.className = "notificationUrl";

    var cardTitle = document.createElement("h5");
    cardTitle.className = "card-title";
    cardTitle.innerHTML = n.message;

    var cardDate = document.createElement("h6");
    cardDate.className = "card-subtitle mb-2 text-muted";
    let timeStamp = new Date(n.timeStamp).toLocaleString('en-GB');
    cardDate.innerHTML = timeStamp;

    var deleteCardLink = document.createElement("a");
    deleteCardLink.className = "card-link delete-link";
    deleteCardLink.innerHTML = "Delete";
    deleteCardLink.onclick = function (event) {
        // Prevent notification list from closing after every delete click
        event.stopPropagation();
        deleteNotification(n, card);
    };   

    notificationURL.appendChild(cardTitle);
    notificationURL.appendChild(cardDate);

    if (n.isRead == false) {
        notificationURL.onclick = function () {
            UpdateNotificationToRead(n);
        };
    }    

    cardBody.appendChild(notificationURL);
    cardBody.appendChild(deleteCardLink);

    card.appendChild(cardBody);

    return card;
}

function deleteNotification(notification, card) {
    fetch(`/api/Notifications/DeleteNotification/${notification.notificationId}`)
    .then(response => {
        if (response.ok) {
            // Remove notification from list
            var notificationWrapper = document.getElementById("notificationWrapper");
            notificationWrapper.removeChild(card);

        } else {
            console.error("Failed to delete notification", response.statusText);
        }
    })
    .catch(error => {
        console.error("Error deleting notification: ", error);
    });
}

function UpdateNotificationToRead(notification) {
    fetch(`/api/Notifications/MarkNotificationAsRead/${notification.notificationId}`)
    .then(response => {
        if (response.ok) {
            notification.isRead = true;
        } else {
            console.error("Failed to mark notification as read", response.statusText);
        }
    })
    .catch(error => {
        console.error("Error marking notification as read: ", error);
    });
}

fetchUserNotifications();