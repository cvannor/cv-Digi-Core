// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

var expanded = true;
var header = document.getElementsByClassName("sidenav")[0];
var nav = document.getElementsByTagName("nav")[0];
var body = document.getElementsByClassName("main")[0];
var index = document.getElementsByClassName("index")[0];
var toggler = document.getElementsByClassName("toggled-toggler")[0]
var navLinks = document.getElementsByClassName("nav-link");
var current = document.getElementsByClassName("activeLink");
var active = 0;

$(document).ready(function () {
    var clicked = localStorage.getItem("clicked");
    if (clicked == "true") {
        active = localStorage.getItem("activeLink");
        localStorage.setItem("clicked", false);
    }
    active = localStorage.getItem("activeLink");
    if (active != 0 && active !=null) {
        current[0].className = current[0].className.replace(" activeLink", "");

    }

    navLinks[active].classList.add("activeLink");



});

document.addEventListener('DOMContentLoaded', function (event) {
    var fullWidth = document.documentElement.clientWidth;


    if (fullWidth < 576) {
        expanded = false;
        header.classList.add('collapsed');
    }


})

function displayWindowSize() {
    var fullWidth = document.documentElement.clientWidth;


    if (fullWidth <= 576) {
        expanded = false;
        header.classList.add('collapsed');
        toggler.style.left = 0;
    }

    if (fullWidth >= 576) {
        expanded = true;
        header.classList.remove('collapsed');
        toggler.style.left = 82 + "px";
    }

}

for (var i = 0; i < navLinks.length; i++) {
    navLinks[i].addEventListener("click", function () {
        localStorage.setItem('activeLink', this.id);
        localStorage.setItem('clicked', true);
        console.log(localStorage);
    });
}



function toggleNav() {
    if (expanded) {
        header.classList.add('collapsed');
        toggler.style.left = 0;
        expanded = false;
    } else {
        header.classList.remove('collapsed');
        toggler.style.left = 82 + "px";
        expanded = true;
    }
}

window.addEventListener("resize", displayWindowSize);

//displayWindowSize();


