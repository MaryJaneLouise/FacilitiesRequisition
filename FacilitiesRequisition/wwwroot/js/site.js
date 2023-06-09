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

function togglePassword() {
    var passwordField = document.getElementById("passwordField");
    passwordField.type = passwordField.type === "password" ? "text" : "password";
    
}

function toggleRepeatPassword() {
    var repeatpasswordField = document.getElementById("repeatPasswordField");
    repeatpasswordField.type = repeatpasswordField.type === "password" ? "text" : "password";
}
function generateRandomColor() {
    var red = Math.floor(Math.random() * 156); 
    var green = Math.floor(Math.random() * 156);
    var blue = Math.floor(Math.random() * 156);
    return 'rgb(' + red + ', ' + green + ', ' + blue + ')';
}

// Apply random background color to profile icons
document.addEventListener('DOMContentLoaded', function() {
    var profileIcons = document.getElementsByClassName('profile-icon');
    for (var i = 0; i < profileIcons.length; i++) {
        profileIcons[i].style.backgroundColor = generateRandomColor();
    }
});

function showModal(event) {
    // Create the modal HTML content
    let modalContent = `
        <div class="modal fade" id="eventModal" tabindex="-1" role="dialog" aria-labelledby="eventModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="eventModalLabel">Event Details</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <p><strong>Name:</strong> ${event.text}</p>
                        <p><strong>Start Date:</strong> ${event.start.toString()}</p>
                        <p><strong>End Date:</strong> ${event.end.toString()}</p>
                        <!-- Add more details as needed -->
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    `;

    // Append the modal HTML content to the document body
    $('body').append(modalContent);

    // Show the modal
    let modalElement = $('#eventModal');
    modalElement.modal('show');

    // Set focus to the modal
    modalElement.on('shown.bs.modal', function () {
        modalElement.focus();
    });

    // Remove the modal from the DOM when it's closed
    modalElement.on('hidden.bs.modal', function () {
        modalElement.remove();
    });
}

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

