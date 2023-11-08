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
    console.log("notis: " + notifications);

    const notificationList = document.getElementById("notificationList");
    const notificationBadge = document.getElementById("notificationBadge");

    notifications.forEach(n => {
        console.log(n);
        var li = document.createElement("li");
        var a = document.createElement("a");

        li.className = "dropdown-item";

        a.href = n.url + "?id=" + n.objectId;
        a.innerHTML = n.message;

        li.appendChild(a);

        notificationList.appendChild(li);

        const currentCount = parseInt(notificationBadge.innerHTML);
        notificationBadge.innerHTML = (currentCount + 1).toString();
        notificationBadge.style.display = "inline";

    });
    
}

fetchUserNotifications();