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

    const notificationList = document.getElementById("notificationList");
    const notificationBadge = document.getElementById("notificationBadge");

    notifications.forEach(n => {
        console.log(n);
        var notification = createNotification(n);
        
        notificationList.appendChild(notification);
        const currentCount = parseInt(notificationBadge.innerHTML);
        notificationBadge.innerHTML = (currentCount + 1).toString();
    });    
}

function createNotification(n) {

    var div = document.createElement("div");
    div.className = "row overflow-hidden";

    var divText = document.createElement("div");
    divText.className = "col-10 col-sm-10 col-xl-10 overflow-hidden";

    var divButton = document.createElement("div");
    divButton.className = "col-2 col-sm-2 col-xl-2";

    var li = document.createElement("li");
    li.appendChild(div);

    var a = document.createElement("a");
    a.className = "dropdown-item overflow-hidden";
    a.href = n.url + "?id=" + n.objectId;
    a.innerHTML = n.message;

    var deleteButton = document.createElement("button");
    deleteButton.className = "btn btn-danger ms-auto";
    deleteButton.type = "button";
    deleteButton.innerHTML = "X";

    //li.appendChild(a);
    //li.appendChild(deleteButton);
    divText.appendChild(a);
    divButton.appendChild(deleteButton);

    div.appendChild(divText);
    div.appendChild(divButton);

    deleteButton.onclick = function (event) {
        // Prevent notification list from closing after every delete click
        event.stopPropagation();
        deleteNotification(n, li);
    };   

    return li;
}

function deleteNotification(notification, li) {
    fetch(`/api/Notifications/DeleteNotification/${notification.notificationId}`, function (data) {

    })
    .then(response => {
        if (response.ok) {
            // Remove notification from list
            var notificationList = document.getElementById("notificationList");
            notificationList.removeChild(li);

            // Decrease notification count by 1
            var notificationBadge = document.getElementById("notificationBadge");
            const currentCount = parseInt(notificationBadge.innerHTML);
            notificationBadge.innerHTML = (currentCount - 1).toString();

            console.log("Notification delted", notification);
        } else {
            console.error("Failed to delete notification", response.statusText);
        }
    })
    .catch(error => {
        console.error("Error deleting notification: ", error);
    });
}

fetchUserNotifications();