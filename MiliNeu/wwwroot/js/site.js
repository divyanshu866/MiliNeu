// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


document.addEventListener("DOMContentLoaded", function () {
    const main = document.getElementsByTagName("main")[0];

    if (window.location.pathname == "/" || window.location.pathname == "/Home/Index" || window.location.pathname == "/Home/") {
        main.style.paddingTop = "0";
        console.log("Home page: main{" + window.location.pathname + "}");

    }





    // Define the cookie expiration time (e.g., 30 days)
    const cookieExpirationDays = 1;

    // Check if the user has already accepted cookies
    const cookiesAccepted = localStorage.getItem("cookiesAccepted");
    const cookieTimestamp = localStorage.getItem("cookiesTimestamp");

    // If cookies are not accepted or if the consent has expired, show the popup
    if (!cookiesAccepted || (new Date().getTime() - cookieTimestamp > cookieExpirationDays * 24 * 60 * 60 * 1000)) {
        document.documentElement.style.overflow = "hidden";
        document.getElementsByClassName("overlay")[0].style.display = "initial";
        document.getElementById("cookie-consent-popup").style.display = "block";
    }

    // Handle the click event when user accepts cookies
    document.getElementById("accept-cookies-btn").addEventListener("click", function () {
        //Enable Scrolling and remove overlay
        document.documentElement.style.overflow = "";
        document.getElementsByClassName("overlay")[0].style.display = "none";

        // Hide the popup
        document.getElementById("cookie-consent-popup").style.display = "none";

        // Set a flag in local storage to remember the user's decision
        localStorage.setItem("cookiesAccepted", "true");

        // Store the timestamp when the user accepted cookies
        localStorage.setItem("cookiesTimestamp", new Date().getTime().toString());
    });

});


function throttle(cb, delay = 1000) {
    let shouldWait = false
    let waitingArgs
    const timeoutFunc = () => {
        if (waitingArgs == null) {
            shouldWait = false
        } else {
            cb(...waitingArgs)
            waitingArgs = null
            setTimeout(timeoutFunc, delay)
        }
    }

    return (...args) => {
        if (shouldWait) {
            waitingArgs = args
            return
        }

        cb(...args)
        shouldWait = true
        setTimeout(timeoutFunc, delay)
    }
}














/*Collection scroller...............................*/


