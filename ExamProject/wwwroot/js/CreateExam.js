$(document).ready(function () {
    if ($(".success-login:visible")) {
        setTimeout(() => {
            $(".success-login").hide();
        }, 3000);
    }
});
