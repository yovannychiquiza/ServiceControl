(function ($) {

})(jQuery);

var l = abp.localization.getSource('ServiceControl');

var changed = function (instance, cell, x, y, value) {
    var model = {};
    var column = 0;

    var dateSaved = myTable.getValueFromCoords([x], [y]);
    model.id = myTable.getValueFromCoords([column++], [y]);
    model.serial = myTable.getValueFromCoords([column++], [y]);
    model.dateBooked = myTable.getValueFromCoords([column++], [y]);
    model.sgi = myTable.getValueFromCoords([column++], [y]);
    model.customerFirstName = myTable.getValueFromCoords([column++], [y]);
    model.customerLastName = myTable.getValueFromCoords([column++], [y]);
    model.contactPhone = myTable.getValueFromCoords([column++], [y]);
    model.email = myTable.getValueFromCoords([column++], [y]);
    model.dateOfBirth = myTable.getValueFromCoords([column++], [y]);
    model.firstIdentification = myTable.getValueFromCoords([column++], [y]);
    model.secondIdentification = myTable.getValueFromCoords([column++], [y]);
    model.existingAccountNo = myTable.getValueFromCoords([column++], [y]);
    model.streetNo = myTable.getValueFromCoords([column++], [y]);
    model.customerAddress = myTable.getValueFromCoords([column++], [y]);
    model.unit = myTable.getValueFromCoords([column++], [y]);
    model.city = myTable.getValueFromCoords([column++], [y]);
    model.postalCode = myTable.getValueFromCoords([column++], [y]);
    model.promoDetails = myTable.getValueFromCoords([column++], [y]);
    model.timeSlot = myTable.getValueFromCoords([column++], [y]);
    model.notes = myTable.getValueFromCoords([column++], [y]);
    model.orderNo = myTable.getValueFromCoords([column++], [y]);
    model.accountNo = myTable.getValueFromCoords([column++], [y]);
    model.installDate = myTable.getValueFromCoords([column++], [y]);
    model.orderStateName = myTable.getValueFromCoords([column++], [y]);
    model.remarks = myTable.getValueFromCoords([column++], [y]);
    model.followed = myTable.getValueFromCoords([column++], [y]);
    model.explanation = myTable.getValueFromCoords([column++], [y]);

    if ((x === '25' || x === '26') && model.orderStateName !== l('Cancelled')) {
        abp.notify.error(l('NotCancelled'));
    } else {

        $.ajax({
            url: "/api/services/app/Order/GetBookingUpdate",
            data: model
        })
        .done(function (msg) {
            abp.notify.info(l('SavedSuccessfully') + " " + dateSaved);
            setStyleSpread(model.orderStateName, parseInt(y) + 1);
        })
        .fail(function (xhr, status, error) {
            abp.message.error(xhr.responseJSON.error.details, xhr.responseJSON.error.message);
            abp.notify.error(xhr.responseJSON.error.details);
        });
    }

};

var data1 = [[]];

var myTable = jexcel(document.getElementById('spreadsheet'), {
    data : data1,
    rowResize: true,
    columnDrag: true,
    tableOverflow: true,
    tableWidth: ($('.card').width() - 2) +"px",
    columns: [
        { type: 'text', width: '50', title: l('Id'), readOnly: true },
        { type: 'text', width: '100', title: l('Serial'), readOnly: true, },
        { type: 'text', width: '100', title: l('DateBooked'), readOnly: true, },
        { type: 'text', width: '100', title: l('Sgi'), readOnly: true, },
        { type: 'text', width: '100', title: l('CustomerFirstName') },
        { type: 'text', width: '100', title: l('CustomerLastName') },
        { type: 'text', width: '100', title: l('ContactPhone') },
        { type: 'text', width: '100', title: l('Email') },
        { type: 'text', width: '100', title: l('DateOfBirth') },
        { type: 'text', width: '100', title: l('FirstIdentification'), readOnly: true, },
        { type: 'text', width: '100', title: l('SecondIdentification'), readOnly: true, },
        { type: 'text', width: '100', title: l('ExistingAccountNo') },
        { type: 'text', width: '100', title: l('StreetNo') },
        { type: 'text', width: '100', title: l('CustomerAddress') },
        { type: 'text', width: '100', title: l('Unit') },
        { type: 'text', width: '100', title: l('City') },
        { type: 'text', width: '100', title: l('PostalCode') },
        { type: 'text', width: '100', title: l('PromoDetails') },
        { type: 'text', width: '100', title: l('TimeSlot'), readOnly: true,},
        { type: 'text', width: '100', title: l('Notes') },
        { type: 'text', width: '100', title: l('OrderNo') },
        { type: 'text', width: '100', title: l('AccountNo') },
        { type: 'text', width: '200', title: l('InstallDate') },
        {
            type: 'dropdown', width: '150', title: l('OrderState'), source: [
                l("Booked"),
                l("Cancelled"),
                l("Delayed"),
                l("Follow"),
            ]
        },
        { type: 'text', width: '100', title: l('Remarks') },
        {
            type: 'dropdown', width: '100', title: l('Followed'), source: [
                l("Yes"),
                l("No"),
            ]
        },
        { type: 'text', width: '100', title: l('Explanation') },
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
            var row = 2;
        result.result.forEach(function (item) {
            myTable.insertRow([
                item.id,
                item.serial,
                item.dateBooked,
                item.sgi,
                item.customerFirstName,
                item.customerLastName,
                item.contactPhone,
                item.email,
                item.dateOfBirth,
                item.firstIdentification.name,
                item.secondIdentification.name,
                item.existingAccountNo,
                item.streetNo,
                item.customerAddress,
                item.unit,
                item.city,
                item.postalCode,
                item.promoDetails,
                item.timeSlot.name,
                item.notes,
                item.orderNo,
                item.accountNo,
                item.installDate,
                item.orderState.name,
                item.remarks,
                item.followed,
                item.explanation,
            ]);
            setStyleSpread(item.orderState.name, row++);
        });
        myTable.deleteRow(0, 1);
    });
}


function setStyleSpread(orderState, row) {
    var color = '';
    if (orderState === l('Booked'))
        color = l('Green');
    if (orderState === l('Cancelled'))
        color = l('Red');
    if (orderState === l('Delayed'))
        color = l('Yellow');
    if (orderState === l('Follow'))
        color = l('Yellow');

    var style = myTable.getStyle('A' + row);

    if (!style.includes(color)) {
        myTable.setStyle('A' + row, 'background-color', color);
        myTable.setStyle('B' + row, 'background-color', color);
        myTable.setStyle('C' + row, 'background-color', color);
        myTable.setStyle('D' + row, 'background-color', color);
        myTable.setStyle('E' + row, 'background-color', color);
        myTable.setStyle('F' + row, 'background-color', color);
        myTable.setStyle('G' + row, 'background-color', color);
        myTable.setStyle('H' + row, 'background-color', color);
        myTable.setStyle('I' + row, 'background-color', color);
        myTable.setStyle('J' + row, 'background-color', color);
        myTable.setStyle('K' + row, 'background-color', color);
        myTable.setStyle('L' + row, 'background-color', color);
        myTable.setStyle('M' + row, 'background-color', color);
        myTable.setStyle('N' + row, 'background-color', color);
        myTable.setStyle('O' + row, 'background-color', color);
        myTable.setStyle('P' + row, 'background-color', color);
        myTable.setStyle('Q' + row, 'background-color', color);
        myTable.setStyle('R' + row, 'background-color', color);
        myTable.setStyle('S' + row, 'background-color', color);
        myTable.setStyle('T' + row, 'background-color', color);
        myTable.setStyle('U' + row, 'background-color', color);
        myTable.setStyle('V' + row, 'background-color', color);
        myTable.setStyle('W' + row, 'background-color', color);
        myTable.setStyle('X' + row, 'background-color', color);
        myTable.setStyle('Y' + row, 'background-color', color);
        myTable.setStyle('Z' + row, 'background-color', color);
        myTable.setStyle('AA' + row, 'background-color', color);
    }

}

