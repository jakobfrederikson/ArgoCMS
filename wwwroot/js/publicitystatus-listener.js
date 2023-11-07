// Hide groups on page load
let teamGroup = document.getElementById("team-group");
let projectGroup = document.getElementById("project-group");

teamGroup.style.display = "none";
projectGroup.style.display = "none";

// Add an event listener to the PublicityStatus dropdown
document.getElementById("publicityStatusDropdown").addEventListener("change", function () {
    var selectedValue = this.value;

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
});