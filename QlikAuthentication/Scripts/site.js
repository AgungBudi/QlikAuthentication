
$(document).ready(function () {
    $('#loading-screen').hide();
    $(document)
        .ajaxStart(function () {
            $('#loading-screen').show();
        })
        .ajaxStop(function () {
            $('#loading-screen').hide();
    });
});
