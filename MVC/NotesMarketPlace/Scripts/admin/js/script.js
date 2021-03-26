/*==================================================
                    Toggle-Password
==================================================*/

//For Login and Old password field in Change password page

$("#toggle-password").click(function() {
  var x = document.getElementById("exampleInputPassword1");
  //    var x = $($(this).attr("toggle"));
  if (x.type === "password") {
    x.type = "text";
  } else {
    x.type = "password";
  }
});

//For New password field in Change password page

$("#toggle-password2").click(function() {
  var x = document.getElementById("exampleInputPassword2");
  //    var x = $($(this).attr("toggle"));
  if (x.type === "password") {
    x.type = "text";
  } else {
    x.type = "password";
  }
});

//For Confirm password field in Change password page

$("#toggle-password3").click(function() {
  var x = document.getElementById("exampleInputPassword3");
  //    var x = $($(this).attr("toggle"));
  if (x.type === "password") {
    x.type = "text";
  } else {
    x.type = "password";
  }
});

/*==================================================
                    FAQ
==================================================*/

//faq toggle stuff
$(".question-toggle").click(function(e) {
  e.preventDefault();
  var notthis = $(".active").not(this);
  notthis
    .find(".fa-minus")
    .addClass("fa-plus")
    .removeClass("fa-minus");
  notthis
    .toggleClass("active")
    .next(".faqanswer")
    .slideToggle(300);
  $(this)
    .toggleClass("active")
    .next()
    .slideToggle("fast");
  $(this)
    .find("i")
    .toggleClass("fa-plus fa-minus");
});

/*==================================================
                    Navbar
==================================================*/
$(function() {
  //Show/Hide nav on page load
  ShowHideNav();

  $(window).scroll(function() {
    //Show/Hide nav on window's Scroll
    ShowHideNav();
  });

  function ShowHideNav() {
    if ($(window).scrollTop() > 50) {
      // Show white nav
      $("header").removeClass("navbar-transparent");

      // Show dark logo
      $(".navbar-brand-2 img").attr("src", "images/Login/top-logo_2.png");
    } else {
      // Hide white nav
      $("header").addClass("navbar-transparent");

      // Show dark logo
      $(".navbar-brand-2 img").attr("src", "images/Login/top-logo.png");
    }
    if ($(window).width() < 768) {
      $("header").removeClass("navbar-transparent");
      // Show dark logo
      $(".navbar-brand img").attr("src", "images/Login/top-logo_2.png");
    } else {
      //
    }
  }
});

/*==================================================
                    add and remove active class in navbar
==================================================*/
/*$(".navbar .navbar-nav a").on("click", function() {
  $(".navbar .navbar-nav")
    .find("a.active")
    .removeClass("active");
  $(this).addClass("active");
});*/

$(document).ready(function () {
    var url2 = window.location.href.split('/');
    var url4 = url2[4].split('?');
    var url5 = url4[0];
    var url = url2.slice(0, 4).join('/') + '/' + url5;
    $('.navbar .navbar-nav a[href="' + url + '"]').addClass('active');
    $('.navbar .navbar-nav a').filter(function () {
        return this.href == url;
    }).addClass('active');
});

/*dropdown - navbar*/

$(document).ready(function () {
    var url2 = window.location.href.split('/');
    var url4 = url2[4].split('?');
    var url5 = url4[0];
    var url = url2.slice(0, 4).join('/') + '/' + url5;
    $('.navbar .navbar-nav .dropdown-navbar .dropdown-content-navbar  a[href="' + url + '"]').parent().parent().addClass('active');
    $('.navbar .navbar-nav .dropdown-navbar .dropdown-content-navbar  a').filter(function () {
        return this.href == url;
    }).parent().parent().addClass('active');
});

$(document).ready(function () {
    var url2 = window.location.href.split('/');
    var url4 = url2[4].split('?');
    var url5 = url4[0];
    var url = url2.slice(0, 4).join('/') + '/' + url5;  
    $('.navbar .navbar-nav .dropdown-content-profile  a[href="' + url + '"]').parent().parent().addClass('active');
    $('.navbar .navbar-nav .dropdown-content-profile  a').filter(function () {
        return this.href == url;
    }).parent().parent().addClass('active');
});
/*==================================================
                    File input
==================================================*/

$(".custom-file-input").on("change", function() {
  var fileName = $(this)
    .val()
    .split("\\")
    .pop();
  $(this)
    .siblings(".custom-file-label")
    .addClass("selected")
    .html(fileName);
});

/*==================================================
                    Count
==================================================*/

$(".count").each(function () {
    $(this)
        .prop("Counter", 0)
        .animate(
            {
                Counter: $(this).text()
            },
            {
                duration: 2000,
                easing: "swing",
                step: function (now) {
                    $(this).text(Math.ceil(now));
                }
            }
        );
});
