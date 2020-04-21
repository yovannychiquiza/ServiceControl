(function ($) {

    $("#CompanyId").bsMultiSelect();
    $("#OrderStateId").bsMultiSelect();

    var displayAdmin = abp.auth.isGranted('Pages.Orders.Admin') === true ? '': 'none';
    var displayBooking = abp.auth.isGranted('Pages.Booking') === true ? '': 'none';

    var _orderService = abp.services.app.order,
        l = abp.localization.getSource('ServiceControl'),
        _$modal = $('#OrderCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#OrdersTable');

    jSuites.calendar(document.getElementById('dateFrom'), {
        format: l('DateFormatView')
    });
    jSuites.calendar(document.getElementById('dateTo'), {
        format: l('DateFormatView')
    });
    $('.datepicker').datepicker({
        format: l('DateFormatView')
    });

    var _$ordersTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        ajax: function (data, callback, settings) {
            var filter = $('#OrdersSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;

            filter.companyId = getSelectValues('#CompanyId');
            filter.orderStateId = getSelectValues('#OrderStateId');

            abp.ui.setBusy(_$table);
            _orderService.getAll(filter).done(function (result) {
                callback({
                    recordsTotal: result.totalCount,
                    recordsFiltered: result.totalCount,
                    data: result.data.items
                });
            }).always(function () {
                abp.ui.clearBusy(_$table);
            });
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$ordersTable.draw(false)
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                defaultContent: '',
            },
            {
                targets: 1,
                data: 'company.name',
                sortable: false
            },
            {
                targets: 2,
                data: 'serial',
                sortable: false
            },
            {
                targets: 3,
                data: 'dateBooked',
                sortable: false,
            },
            {
                targets: 4,
                data: 'orderState.name',
                sortable: false
            },
            {
                targets: 5,
                data: 'sgi',
                sortable: false,
            },
            {
                targets: 6,
                data: 'salesRep.fullName',
                sortable: false,
            },
            {
                targets: 7,
                data: 'customerFirstName',
                sortable: false,
            },
            {
                targets: 8,
                data: 'customerLastName',
                sortable: false,
            },
            {
                targets: 9,
                data: 'contactPhone',
                sortable: false,
            },
            {
                targets: 10,
                data: 'email',
                sortable: false,
            },
            {
                targets: 11,
                data: 'dateOfBirth',
                sortable: false,
            },
            {
                targets: 12,
                data: 'firstIdentification.name',
                sortable: false,
            },
            {
                targets: 13,
                data: 'secondIdentification.name',
                sortable: false,
            },
            {
                targets: 14,
                data: 'existingAccountNo',
                sortable: false,
            },
            {
                targets: 15,
                data: 'streetNo',
                sortable: false,
            },
            {
                targets: 16,
                data: 'customerAddress',
                sortable: false,
            },
            {
                targets: 17,
                data: 'unit',
                sortable: false,
            },
            {
                targets: 18,
                data: 'city',
                sortable: false,
            },
            {
                targets: 19,
                data: 'postalCode',
                sortable: false,
            },
            {
                targets: 20,
                data: 'promoDetails',
                sortable: false,
            },
            {
                targets: 21,
                data: 'notes',
                sortable: false,
            },
            {
                targets: 22,
                data: 'timeSlot.name',
                sortable: false,
            },
            {
                targets: 23,
                data: 'orderNo',
                sortable: false,
            },
            {
                targets: 24,
                data: 'accountNo',
                sortable: false,
            },
            {
                targets: 25,
                data: 'installDate',
                sortable: false,
            },
            {
                targets: 26,
                data: 'remarks',
                sortable: false,
            },
            {
                targets: 27,
                data: 'followed',
                sortable: false,
            },
            {
                targets: 28,
                data: 'explanation',
                sortable: false,
            },
            {
                targets: 29,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" style="display:${displayAdmin}" class="btn btn-sm bg-secondary edit-order" data-order-id="${row.id}" data-toggle="modal" data-target="#OrderEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" style="display:${displayAdmin}" class="btn btn-sm bg-danger edit-order delete-order" data-order-id="${row.id}" data-order-serial="${row.serial}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>',
                        `   <button type="button" style="display:${displayBooking}" class="btn btn-sm bg-primary booking-order" data-order-id="${row.id}" data-toggle="modal" data-target="#OrderEditModal">`,
                        `       <i class="far fa-calendar-check"></i> ${l('Booking')}`,
                        '   </button>',
                    ].join('');
                }
            }
        ]
    });

    _$form.validate({
        rules: {
            Password: "required",
            ConfirmPassword: {
                equalTo: "#Password"
            }
        }
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var order = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _orderService.create(order).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
            _$ordersTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-order', function () {
        var orderId = $(this).attr("data-order-id");
        var orderSerial = $(this).attr('data-order-serial');
        deleteOrder(orderId, orderSerial);
    });

    function deleteOrder(orderId, orderSerial) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                orderSerial),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    $.ajax({
                        url: "api/services/app/Order/GetOrderDelete",
                        data: { id: orderId }
                    })
                    .done(function (msg) {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$ordersTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-order', function (e) {
        var orderId = $(this).attr("data-order-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Orders/EditModal?orderId=' + orderId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#OrderEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    });

    $(document).on('click', 'a[data-target="#OrderCreateModal"]', (e) => {
        $('.nav-tabs a[href="#order-details"]').tab('show')
    });

    abp.event.on('order.edited', (data) => {
        _$ordersTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$ordersTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which === 13) {
            _$ordersTable.ajax.reload();
            return false;
        }
    });

   

    $(document).on('click', '.booking-order', function (e) {
        var orderId = $(this).attr("data-order-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Orders/BookingModal?orderId=' + orderId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#OrderEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    });
})(jQuery);
