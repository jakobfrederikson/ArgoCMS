// Function to show/hide groups based on selected value
function updateFormGroups() {
    var selectedValue = document.getElementById("publicityStatusDropdown").value;
    var teamGroup = document.getElementById("team-group");
    var projectGroup = document.getElementById("project-group");

    if (selectedValue === "Company") {
        teamGroup.style.display = "none"; // Hide Team group
        projectGroup.style.display = "none"; // Hide Project group
    } else if (selectedValue === "Project") {
        projectGroup.style.display = "block"; // Show Project group
        teamGroup.style.display = "none"; // Hide Team group
    } else if (selectedValue === "Team") {
        projectGroup.style.display = "none"; // Hide Project group
        teamGroup.style.display = "block"; // Show Team group
    }
}

// Add an event listener to execute the function on page load
document.addEventListener("DOMContentLoaded", updateFormGroups);

// Add an event listener to the PublicityStatus dropdown
document.getElementById("publicityStatusDropdown").addEventListener("change", updateFormGroups);