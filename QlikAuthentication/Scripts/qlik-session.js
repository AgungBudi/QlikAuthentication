$(document).ready(function () {
    var form = $('#qlik-session');

    $('#btn-getSession').click(function () {
        form.validate();
        if (form.valid()) {
            $.ajax({
                type: 'POST',
                url: homeUrl + '/QlikSession/RequestSession',
                data: form.serialize(),
                success: function (response) {
                    $('.qlik-url').val(response);
                }
            });
        }
    });

    $('#btn-session').click(function () {
        $.ajax({
            type: 'POST',
            url: 'QlikSession/GetSession',
            success: function (response) {
                $('.session-id').val(response);
            }
        });
    });

    $('#btn-go').click(function () {
        window.open($('.qlik-url').val(), '_blank');
    });
});