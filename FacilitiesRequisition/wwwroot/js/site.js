$(document).ready(function() {
    $(".clickable-row").click(function() {
        var href = $(this).data("href");
        window.location.href = href;
    });
    

    $("#search-user").keyup(function () {
        var value = $(this).val().toLowerCase();
        var matchedItems = $('.user-container .hover-item').filter(function () {
            var isFound = $(this).find(".profile-icon span").text().toLowerCase().indexOf(value) > -1
                || $(this).find(".user-details").text().toLowerCase().indexOf(value) > -1;
            return isFound;
        });

        if (matchedItems.length === 0) {
            $('.no-results').show();
        } else {
            $('.no-results').hide();
        }

        $('.user-container .hover-item').hide();
        matchedItems.show();
    });

    $("#search-org").keyup(function () {
        var value = $(this).val().toLowerCase();
        var matchedItems = $('.user-container .hover-item').filter(function () {
            var isFound = $(this).find(".profile-icon span").text().toLowerCase().indexOf(value) > -1
                || $(this).find(".org-details").text().toLowerCase().indexOf(value) > -1;
            return isFound;
        });

        if (matchedItems.length === 0) {
            $('.no-results-org').show();
        } else {
            $('.no-results-org').hide();
        }

        $('.user-container .hover-item').hide();
        matchedItems.show();
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

document.addEventListener('keydown', function(event) {
    if (event.ctrlKey && event.key === 'p') {
        event.preventDefault();
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
    let modalContent = `
        <div class="modal fade" id="eventModal" tabindex="-1" role="dialog" aria-labelledby="eventModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="eventModalLabel">Event Details</h5>
                        <button type="button" class="btn-close close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <p><strong>Name:</strong> ${event.text}</p>
                        <p><strong>Start Date:</strong> ${event.start.toString()}</p>
                        <p><strong>End Date:</strong> ${event.end.toString()}</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary close" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    `;
    
    $('body').append(modalContent);
    
    let modalElement = $('#eventModal');
    modalElement.modal('show');
    
    modalElement.on('shown.bs.modal', function () {
        modalElement.focus();
    });
    
    modalElement.on('hidden.bs.modal', function () {
        modalElement.remove();
    });
    
    modalElement.find('.close').on('click', function () {
        modalElement.modal('hide');
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

function updateMonthYear(startDate) {
    // Get the current date
    var currentDate = new Date(startDate);

    // Extract the month and year from the current date
    var currentMonth = currentDate.toLocaleString('default', { month: 'long' });
    var currentYear = currentDate.getFullYear();

    // Set the current month and year in the HTML
    document.getElementById('current-month-year').textContent = currentMonth + ' ' + currentYear;
}

// Function to change the month
function changeMonth(offset) {
    dp.startDate = dp.startDate.addMonths(offset);
    dp.update();
    updateMonthYear(dp.startDate);
}

// Initial update of the current month and year
updateMonthYear(dp.startDate);

function previewImage(event) {
    var input = event.target;
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById('signaturePreview').src = e.target.result;
        };
        reader.readAsDataURL(input.files[0]);
    }
}

