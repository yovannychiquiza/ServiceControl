(function ($) {

    $('.datepicker').datepicker({
        format: 'mm/dd/yyyy'
    });

    var _orderService = abp.services.app.order,
        l = abp.localization.getSource('ServiceControl'),
        _$modal = $('#OrderEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var order = _$form.serializeFormToObject();

        abp.ui.setBusy(_$form);
        _orderService.update(order).done(function () {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            abp.event.trigger('order.edited', order);
        }).always(function () {
            abp.ui.clearBusy(_$form);
        });
    }

    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first').focus();
    });
})(jQuery);
