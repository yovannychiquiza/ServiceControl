(function ($) {

})(jQuery);

var l = abp.localization.getSource('ServiceControl');

var changed = function (instance, cell, x, y, value) {
    var model = {};
    var column = 0;
   
    var dateSaved = myTable.getValueFromCoords([x], [y]);
    model.id = myTable.getValueFromCoords([column++], [y]);
    model.sgi = myTable.getValueFromCoords([column++], [y]);
    model.customerFirstName = myTable.getValueFromCoords([column++], [y]);
    model.customerLastName = myTable.getValueFromCoords([column++], [y]);
    model.contactPhone = myTable.getValueFromCoords([column++], [y]);
    model.email = myTable.getValueFromCoords([column++], [y]);
    model.dateOfBirth = myTable.getValueFromCoords([column++], [y]);
    model.existingAccountNo = myTable.getValueFromCoords([column++], [y]);
    model.streetNo = myTable.getValueFromCoords([column++], [y]);
    model.customerAddress = myTable.getValueFromCoords([column++], [y]);
    model.unit = myTable.getValueFromCoords([column++], [y]);
    model.city = myTable.getValueFromCoords([column++], [y]);
    model.postalCode = myTable.getValueFromCoords([column++], [y]);
    model.promoDetails = myTable.getValueFromCoords([column++], [y]);
    model.notes = myTable.getValueFromCoords([column++], [y]);
    model.orderStateName = myTable.getValueFromCoords([column++], [y]);
    model.orderNo = myTable.getValueFromCoords([column++], [y]);
    model.accountNo = myTable.getValueFromCoords([column++], [y]);
    model.installDate = myTable.getValueFromCoords([column++], [y]);
    model.remarks = myTable.getValueFromCoords([column++], [y]);
  
    $.ajax({
        url: "/api/services/app/Order/GetBookingUpdate",
        data: model
    })
    .done(function (msg) {
        abp.notify.info(l('SavedSuccessfully') +" "+ dateSaved);
    })
    .fail(function (xhr, status, error) {
        abp.message.error(xhr.responseJSON.error.details, xhr.responseJSON.error.message);
        abp.notify.error(xhr.responseJSON.error.details);
    });

};

var data1 = [[]];

var myTable = jexcel(document.getElementById('spreadsheet'), {
    data : data1,
    rowResize: true,
    columnDrag: true,
    columns: [
        { type: 'text', width: '50', title: l('Id'), readOnly: true},
        { type: 'text', width: '100', title: l('Sgi') },
        { type: 'text', width: '100', title: l('CustomerFirstName') },
        { type: 'text', width: '100', title: l('CustomerLastName') },
        { type: 'text', width: '100', title: l('ContactPhone') },
        { type: 'text', width: '100', title: l('Email') },
        { type: 'text', width: '100', title: l('DateOfBirth') },
        { type: 'text', width: '100', title: l('ExistingAccountNo') },
        { type: 'text', width: '100', title: l('StreetNo') },
        { type: 'text', width: '100', title: l('CustomerAddress') },
        { type: 'text', width: '100', title: l('Unit') },
        { type: 'text', width: '100', title: l('City') },
        { type: 'text', width: '100', title: l('PostalCode') },
        { type: 'text', width: '100', title: l('PromoDetails') },
        { type: 'text', width: '100', title: l('Notes') },
        {
            type: 'dropdown', width: '150', title: l('OrderState'), source: [
                "Booked",
                "Cancelled",
                "Delayed",
                "Follow",
            ]
        },
        { type: 'text', width: '100', title: l('OrderNo') },
        { type: 'text', width: '100', title: l('AccountNo') },
        { type: 'text', width: '200', title: l('InstallDate') },
        { type: 'text', width: '100', title: l('Remarks') },
        { type: 'text', width: '100', title: l('Serial'), readOnly: true, },
        { type: 'text', width: '100', title: l('DateBooked'), readOnly: true, },
        { type: 'text', width: '100', title: l('FirstIdentification'), readOnly: true,},
        { type: 'text', width: '100', title: l('SecondIdentification'), readOnly: true,},
        { type: 'text', width: '100', title: l('TimeSlot'), readOnly: true,},

    ],
    onchange: changed

});

$(document).on('click', '.booking', function () {
    start();
});

start();

function start() {
    $.ajax({
        url: "/api/services/app/Order/GetBooking"
    })
    .done(function (result) {
        result.result.forEach(function (item) {
            myTable.insertRow([
                item.id,
                item.sgi,
                item.customerFirstName,
                item.customerLastName,
                item.contactPhone,
                item.email,
                item.dateOfBirth,
                item.existingAccountNo,
                item.streetNo,
                item.customerAddress,
                item.unit,
                item.city,
                item.postalCode,
                item.promoDetails,
                item.notes,
                item.orderState.name,
                item.orderNo,
                item.accountNo,
                item.installDate,
                item.remarks,
                item.serial,
                item.dateBooked,
                item.firstIdentification.name,
                item.secondIdentification.name,
                item.timeSlot.name,
            ]);
        });
        myTable.deleteRow(0, 1);
    });
}

