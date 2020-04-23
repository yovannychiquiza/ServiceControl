(function ($) {
    var _userService = abp.services.app.subSalesRep,
        l = abp.localization.getSource('ServiceControl'),
        _$modal = $('#UserEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var user = _$form.serializeFormToObject();
        user.subSalesRepList = [];
        var _$userCheckboxes = $("input[name='user']:checked");
        if (_$userCheckboxes) {
            for (var userIndex = 0; userIndex < _$userCheckboxes.length; userIndex++) {
                var _$userCheckbox = $(_$userCheckboxes[userIndex]);
                var id = _$userCheckbox.val();
                var model = {};
                model.subSalesRepId = id;
                user.subSalesRepList.push(model);
                
            }
        }

        abp.ui.setBusy(_$form);
        _userService.update(user).done(function () {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            abp.event.trigger('user.edited', user);
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
