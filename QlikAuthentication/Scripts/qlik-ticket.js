$(document).ready(function () {
    var form = $('#qlik-ticket');

    $('#btn-getTicket').click(function () {
        form.validate();
        if (form.valid()) {
            $.ajax({
                type: 'POST',
                url: homeUrl + '/Home/RequestTicket',
                data: form.serialize(),
                success: function (response) {
                    $('.qlik-url').val(response);
                }
            });
        }
    });

    $('#btn-go').click(function () {
        window.open($('.qlik-url').val(), '_blank');
    });
});