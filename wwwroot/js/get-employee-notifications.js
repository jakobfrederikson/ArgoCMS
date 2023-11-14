function fetchUserNotifications() {
    $.getJSON(`/api/Notifications`, function (data) {
        updateNotificationList(data);
    })
    .fail(function (error) {
        console.error('Error fetching notifications: ', error);
    });
}

// Function to update the notification list on the page
function updateNotificationList(notifications) {
    if (notifications === null || !Array.isArray(notifications)) {
        console.log("Notifications are null or not an array");
        return;
    }

    const notificationWrapper = document.getElementById("notificationWrapper");
    //const notificationBadge = document.getElementById("notificationBadge");

    notifications.forEach(n => {
        console.log(n);
        var notification = createNotification(n);
        
        notificationWrapper.appendChild(notification);
        //const currentCount = parseInt(notificationBadge.innerHTML);
        //notificationBadge.innerHTML = (currentCount + 1).toString();
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

            // Decrease notification count by 1
            //var notificationBadge = document.getElementById("notificationBadge");
            //const currentCount = parseInt(notificationBadge.innerHTML);
            //notificationBadge.innerHTML = (currentCount - 1).toString();

            console.log("Notification delted", notification);
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