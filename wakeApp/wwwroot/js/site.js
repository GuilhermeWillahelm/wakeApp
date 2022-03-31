// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var video = document.querySelector(".video");
var juice = document.querySelector(".orange-juice");
var btn = document.getElementById("play-pause");

function tooglePlayPause() {
    if (video.paused) {
        btn.className = "pause";
        video.play();
    }
    else {
        btn.className = "play";
        video.pause();
    }
}

btn.onclick = function () {
    tooglePlayPause();
};

video.addEventListener("timeupdate", function () {
    var juicePos = video.currentTime / video.duration;
    juice.style.width = juicePos * 100 + "%";
});