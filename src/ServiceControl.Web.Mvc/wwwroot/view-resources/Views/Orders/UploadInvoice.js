(function ($) {
    l = abp.localization.getSource('ServiceControl'),

    $("#but_upload").click(function () {
        var invoiceDetails = "#InvoiceDetails";
        $(invoiceDetails).html("");
        var fd = new FormData();
        var files = $('#file')[0].files[0];
        fd.append('file', files);
        _$table = $(invoiceDetails);

        abp.ui.setBusy(_$table);

        $.ajax({
            url: "/api/services/app/Order/UploadFileInvoice",
            type: 'post',
            data: fd,
            contentType: false,
            processData: false,
        })
        .done(function (response) {
            $(invoiceDetails).html(response);
            abp.notify.info(l('ProcessedSuccessfully'));
            abp.ui.clearBusy(_$table);
        })
        .fail(function (xhr, status, error) {
            abp.ui.clearBusy(_$table);
            abp.message.error(l('FileNoProcessed'), error);
            abp.notify.error(xhr.responseJSON.error.details);
        });

    });
})(jQuery);

