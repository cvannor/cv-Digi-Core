// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

var expanded = true;
var header = document.getElementsByClassName("sidenav")[0];
var nav = document.getElementsByTagName("nav")[0];
var body = document.getElementsByClassName("main")[0];
var index = document.getElementsByClassName("index")[0];
var toggler = document.getElementsByClassName("toggled-toggler")[0];
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
  if (active != 0 && active != null) {
    current[0].className = current[0].className.replace(" activeLink", "");
  }

  navLinks[active].classList.add("activeLink");
});

document.addEventListener("DOMContentLoaded", function (event) {
  var fullWidth = document.documentElement.clientWidth;

  if (fullWidth < 576) {
    expanded = false;
    header.classList.add("collapsed");
  }
});


function _(x) {
  return document.getElementById(x);
}

function phonenumber(inputtxt) {
  var phoneno = /^\d{10}$/;
  if (inputtxt.match(phoneno)) {
    return true;
  } else {
    return false;
  }
}

function ValidateEmail(mail) {
  if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
    return true;
  }
  return false;
}

function contactSubmit() {
  var name = _("name").value;
  var email = _("email").value;
  var phone = _("phone").value;
  var message = _("message").value;
  var save = _("save-contact").checked;
  var loader = _("load");
  var checked = _("checked");
  var alert = _("alert");
  var frm = _("contactForm");

  if (name == "" || email == "" || phone == "" || message == "") {
    checked.style.display = "none";
    alert.style.display = "inline-block";
    _("success").innerHTML = "Please fill out all required fields";
  } else if (phonenumber(phone) == false) {
    checked.style.display = "none";
    alert.style.display = "inline-block";
    _("success").innerHTML = "Invalid phone number";
  } else if (ValidateEmail(email) == false) {
    checked.style.display = "none";
    alert.style.display = "inline-block";
    _("success").innerHTML = "Invalid email address";
  } else {
    alert.style.display = "none";
    _("success").innerHTML = "";
    var ajax = ajaxObj("POST", "ContactAction");
    ajax.onreadystatechange = function () {
      if (ajaxReturn(ajax) == true) {
          _("success").innerHTML = "Thank you! I will be reaching out to you shortly!";
          checked.style.display = "inline-block";
          frm.reset(); // Reset all form data
      }
    };
    ajax.send(
      "name=" +
        name +
        "&email=" +
        email +
        "&phone=" +
        phone +
        "&message=" +
        message +
        "&save=" +
        save
    );
  }
}

function displayWindowSize() {
  var fullWidth = document.documentElement.clientWidth;

  if (fullWidth <= 576) {
    expanded = false;
    header.classList.add("collapsed");
    toggler.style.left = 0;
  }

  if (fullWidth >= 576) {
    expanded = true;
    header.classList.remove("collapsed");
    toggler.style.left = 82 + "px";
  }
}

for (var i = 0; i < navLinks.length; i++) {
  navLinks[i].addEventListener("click", function () {
    localStorage.setItem("activeLink", this.id);
    localStorage.setItem("clicked", true);
    console.log(localStorage);
  });
}

function toggleNav() {
  if (expanded) {
    header.classList.add("collapsed");
    toggler.style.left = 0;
    expanded = false;
  } else {
    header.classList.remove("collapsed");
    toggler.style.left = 82 + "px";
    expanded = true;
  }
}

window.addEventListener("resize", displayWindowSize);

//displayWindowSize();
