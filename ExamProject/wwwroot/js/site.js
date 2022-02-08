// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    if ($(".success-login:visible")) {
        setTimeout(() => {
            $(".success-login").hide();
        }, 3000);
    }
    $(".sorular").hide();
    $(".sorular").first().show();
});

$('select').on('change', function (e) {
    debugger;
    var asd = $('#SoruCevapModel_SoruTextId option:selected').val()
    $(".sorular").hide();
    $('.' + asd).show();
});