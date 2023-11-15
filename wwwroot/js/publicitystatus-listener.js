let teamGroup = document.getElementById("team-group");
let projectGroup = document.getElementById("project-group");

teamGroup.style.display = "none";
projectGroup.style.display = "none";

$(document).ready(function () {
    // Hide groups on page load
    const publicityStatusDropdown = document.getElementById("publicityStatusDropdown");

    if (publicityStatusDropdown.value != null) {
        console.log("Publicity status: ", publicityStatusDropdown.value);
        ChangeSelectListDisplay(publicityStatusDropdown.value);
    }    

    // Add an event listener to the PublicityStatus dropdown
    publicityStatusDropdown.addEventListener("change", function () {
        var selectedValue = publicityStatusDropdown.value;
        console.log("wtf!");
        ChangeSelectListDisplay(selectedValue);
    });

    function ChangeSelectListDisplay(selectedValue) {
        // Depending on the selected value, show/hide the form groups
        if (selectedValue === "Company") {
            teamGroup.style.display = "none"; // Hide TeamId
            projectGroup.style.display = "none"; // Hide ProjectId
        } else if (selectedValue === "Project") {
            projectGroup.style.display = "block"; // Show TeamId
            teamGroup.style.display = "none"; // Hide ProjectId
        } else if (selectedValue === "Team") {
            projectGroup.style.display = "none"; // Hide TeamId
            teamGroup.style.display = "block"; // Show ProjectId
        }
    }
});
