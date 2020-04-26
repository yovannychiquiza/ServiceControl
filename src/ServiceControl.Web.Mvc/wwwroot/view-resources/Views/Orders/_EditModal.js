(function ($) {

    $(".multiselect").bsMultiSelect();

    var _orderService = abp.services.app.order,
        l = abp.localization.getSource('ServiceControl'),
        _$modal = $('#OrderEditModal'),
        _$form = _$modal.find('form');

    jSuites.calendar(document.getElementById('DateOfBirthView'), {
        format: l('DateFormatView')
    });

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var order = _$form.serializeFormToObject();
        order.ProductTypeId = getSelectValues('#ProductTypeIdView');

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
