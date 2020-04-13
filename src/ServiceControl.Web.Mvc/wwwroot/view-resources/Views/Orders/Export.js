(function ($) {

    $('.export-button').on('click', (e) => {
        var filter = $('#OrdersSearchForm').serializeFormToObject(true);
        filter.maxResultCount = 10000;
        filter.skipCount = 0;
        abp.services.app.order.getExportExcel(filter).done(function (result) {
        saveAsFile(result.fileName, result.data);
        }).always(function () {
            
        });
    });

    function saveAsFile(filename, bytes) {
        var link = document.createElement('a');
        link.download = filename;
        link.href = "data:application/octet-stream;base64," + bytes;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }

   
})(jQuery);
