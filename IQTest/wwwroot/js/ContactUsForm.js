const inputs = document.querySelectorAll(".input");

function focusFunc() {
    let parent = this.parentNode;
    parent.classList.add("focus");
}

function blurFunc() {
    let parent = this.parentNode;
    if (this.value == "") {
        parent.classList.remove("focus");
    }
}

inputs.forEach((input) => {
    input.addEventListener("focus", focusFunc);
    input.addEventListener("blur", blurFunc);
});


// handling form submition

window.addEventListener("DOMContentLoaded", function () {

    // get the form elements defined in your form HTML above

    var form = document.getElementById("ContactUsForm");
    var button = document.getElementById("ContactUsForm-button");
    var status = document.getElementById("ContactUsForm-status");

    // Success and Error functions for after the form is submitted

    function success() {
        form.reset();
        Swal.fire({
            title: 'Done',
            text: 'your submission was sent successfully ',
            icon: 'success',
            confirmButtonText: 'OK'
        });
    }

    function error() {
        Swal.fire({
            title: 'Oops! An Error Occurred',
            text: ' There was a problem please try again later or contact us throw social media',
            icon: 'error',
            confirmButtonText: 'OK'
        });
    }

    // handle the form submission event

    form.addEventListener("submit", function (ev) {
        ev.preventDefault();
        var data = new FormData(form);
        var url = "https://formspree.io/f/mzbkknkr";
        ajax(form.method, url, data, success, error);
    });
});

// helper function for sending an AJAX request

function ajax(method, url, data, success, error) {
    var xhr = new XMLHttpRequest();
    xhr.open(method, url);
    xhr.setRequestHeader("Accept", "application/json");
    xhr.onreadystatechange = function () {
        if (xhr.readyState !== XMLHttpRequest.DONE) return;
        if (xhr.status === 200) {
            success(xhr.response, xhr.responseType);
        } else {
            error(xhr.status, xhr.response, xhr.responseType);
        }
    };
    xhr.send(data);
}