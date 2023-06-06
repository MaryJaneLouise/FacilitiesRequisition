$(document).ready(function() {
    $(".clickable-row").click(function() {
        var href = $(this).data("href");
        window.location.href = href;
    });

    $('#search-input').on('input', function () {
        $('form').submit();
    });

    
    var startDateInput = document.getElementById("StartDateRequested");
    var endDateInput = document.getElementById("EndDateRequested");

    // Add event listeners to the input fields
    startDateInput.addEventListener("change", validateDateRange);
    endDateInput.addEventListener("change", validateDateRange);

    function validateDateRange() {
        var startDate = new Date(startDateInput.value);
        var endDate = new Date(endDateInput.value);

        var today = new Date();
        today.setHours(0, 0, 0, 0);

        if (startDate < today || endDate < today) {
            startDateInput.setCustomValidity("Invalid date. Please select a date starting from today or later.");
            endDateInput.setCustomValidity("Invalid date.");
        } else if (startDate > endDate) {
            startDateInput.setCustomValidity("");
            endDateInput.setCustomValidity("End date must be greater than or equal to start date.");
        } else {
            startDateInput.setCustomValidity("");
            endDateInput.setCustomValidity("");
        }
    }
});

function updateClock() {
    var timeElement = document.getElementById('time');
    var now = new Date();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var seconds = now.getSeconds();
    var ampm = (hours < 12) ? "AM" : "PM";
    hours = (hours > 12) ? hours - 12 : hours;
    hours = (hours == 0) ? 12 : hours;
    minutes = (minutes < 10) ? "0" + minutes : minutes;
    seconds = (seconds < 10) ? "0" + seconds : seconds;
    var timeString = hours + ":" + minutes + ":" + seconds + " " + ampm;
    timeElement.innerHTML = timeString;
}
updateClock();
setInterval(updateClock, 1000);

