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

$(document).ready(function() {
    $(".clickRow").click(function() {
        var href = $(this).data("href");
        window.location.href = href;
    });
});

