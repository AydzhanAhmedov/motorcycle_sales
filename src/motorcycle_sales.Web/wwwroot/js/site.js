// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show');
        }
    })
};

jQueryAjaxPost = form => {

    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (!res.isValid) {
                    $("#form-modal .modal-body").html(res.html);
                } else {
                    window.location.replace(res.html);

                }
            },
            error: function (err) {
                console.log(err);
            }
        })
    } catch (ex) {
        console.log(ex);
    }
    // prevent default form submit evvent
    return false;
};

confirmDelete = (url, body) => {
    console.log(url);

    $("#form-modal-confirm .modal-body").html(body);
    $("#form-modal-confirm-button").click(function () {
        $.ajax({
            type: "POST",
            url: url,
            success: function (res) {
                window.location.reload()
            },
            error: function (err) {
                window.alert(err);
            }
        })
    })
    $("#form-modal-confirm").modal('show');
};