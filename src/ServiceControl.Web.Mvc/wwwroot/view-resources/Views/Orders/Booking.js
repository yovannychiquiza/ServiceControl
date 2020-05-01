(function ($) {

})(jQuery);

$("#CompanyId").bsMultiSelect();
$("#OrderStateId").bsMultiSelect();

var l = abp.localization.getSource('ServiceControl');
var _spreadsheet = 'spreadsheet';
var isBookingAdmin = abp.auth.isGranted('Pages.Booking.Admin');
var isOrderAdminReady = abp.auth.isGranted('Order.Admin.Ready');
var isOrderAdminInvoice = abp.auth.isGranted('Order.Admin.Invoice');
var FOLLOWED = 26;
var EXPLANATION = 27;
var IS_READY = 30;
var isLoaded = false;

jSuites.calendar(document.getElementById('dateFrom'), {
    format: l('DateFormatView')
});
jSuites.calendar(document.getElementById('dateTo'), {
    format: l('DateFormatView')
});

var changed = function (instance, cell, x, y, value) {
    var model = {};
    var column = 0;

    var dateSaved = myTable.getValueFromCoords([x], [y]);
    model.id = myTable.getValueFromCoords([column++], [y]);
    model.company = myTable.getValueFromCoords([column++], [y]);
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
    model.PaymentStatusName = myTable.getValueFromCoords([column++], [y]);
    model.invoiceNo = myTable.getValueFromCoords([column++], [y]);
    model.isReady = myTable.getValueFromCoords([column++], [y]);

    if ((x === FOLLOWED.toString() || x === EXPLANATION.toString()) && model.orderStateName !== l('Cancelled')) {
        abp.notify.error(l('NotCancelled'));
    } else {

        $.ajax({
            url: "/api/services/app/Order/GetBookingUpdate",
            data: model
        })
        .done(function (msg) {
            abp.notify.info(l('SavedSuccessfully') + " " + dateSaved);
            setStyleSpread(model.orderStateName, parseInt(y) + 1, model.followed);
        })
        .fail(function (xhr, status, error) {
            abp.message.error(xhr.responseJSON.error.details, xhr.responseJSON.error.message);
            abp.notify.error(xhr.responseJSON.error.details);
        });
    }

};

var data1 = [[]];

var myTable = jexcel(document.getElementById(_spreadsheet), {
    data : data1,
    rowResize: true,
    columnDrag: true,
    tableOverflow: true,
    tableWidth: ($('.card').width() - 2) + "px",
    tableHeight: '350px',
    columns: [
        { type: 'text', width: '50', title: l('Id'), readOnly: true },
        { type: 'text', width: '100', title: l('Company'), readOnly: true, },
        { type: 'text', width: '100', title: l('Serial'), readOnly: true, },
        { type: 'text', width: '100', title: l('DateBooked'), readOnly: true, },
        { type: 'text', width: '100', title: l('Sgi'), readOnly: true, },
        { type: 'text', width: '100', title: l('CustomerFirstName') },
        { type: 'text', width: '100', title: l('CustomerLastName') },
        { type: 'text', width: '100', title: l('ContactPhone') },
        { type: 'text', width: '100', title: l('Email') },
        { type: 'calendar', width: '100', title: l('DateOfBirth'), options: { format: l('DateFormatView')} },
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
        { type: 'text', width: '100', title: l('OrderNo'), readOnly: !isBookingAdmin },
        { type: 'text', width: '100', title: l('AccountNo'), readOnly: !isBookingAdmin },
        { type: 'calendar', width: '200', title: l('InstallDate'), options: { format: l('DateTimeFormatView'), time: 1 }, readOnly: !isBookingAdmin },
        {
            type: 'dropdown', width: '150', title: l('OrderState'), readOnly: !isBookingAdmin, source: [
                l("Booked"),
                l("Cancelled"),
                l("Delayed"),
                l("Follow"),
            ]
        },
        { type: 'text', width: '100', title: l('Remarks'), readOnly: !isBookingAdmin },
        {
            type: 'dropdown', width: '100', title: l('Followed'), source: [
                l("Yes"),
                l("No"),
            ]
        },
        { type: 'text', width: '100', title: l('Explanation') },
        {
            type: 'dropdown', width: '100', title: l('PaymentStatus'), readOnly: !isOrderAdminInvoice, source: [
                l("Done"),
                l("Deduction"),
            ]
        },
        { type: 'text', width: '100', title: l('InvoiceNo'), readOnly: !isOrderAdminInvoice},        
        { type: 'checkbox', width: '100', title: l('IsReady'), readOnly: !isOrderAdminReady},
    ],
    onchange: changed

});

search();

function search() {
    abp.ui.setBusy($('#' + _spreadsheet));
    var filter = $('#OrdersSearchForm').serializeFormToObject(true);
    filter.maxResultCount = 1000;
    filter.skipCount = 0;

    filter.companyId = getSelectValues('#CompanyId');
    filter.orderStateId = getSelectValues('#OrderStateId');

    $.ajax({
        url: "/api/services/app/Order/GetAll",
        data: filter
    })
    .done(function (result) {
        prductsColumn(result.result.productType);
        myTable.insertRow([[]], 0);//insert new empty row after first row
        myTable.deleteRow(0, 1);//delete first row
        if (myTable.records.length !== 1) {
            myTable.deleteRow(1, myTable.records.length);//delete all rows except first row
        }
        var row = 2;
        result.result.data.items.forEach(function (item) {
            var data = [
                item.id,
                item.company.name,
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
                item.paymentStatus.name,
                item.invoiceNo,
                $.parseJSON(item.isReady.toLowerCase()),
            ];
            setProductType(result.result.productType, item.ordersProductType, data);

            myTable.insertRow(data);
            setStyleSpread(item.orderState.name, row++, item.followed);
        });
        if (myTable.records.length > 1 )
            myTable.deleteRow(0, 1);//if table has more than one record, delete first row
    })
    .always(function () {
        abp.ui.clearBusy($('#' + _spreadsheet));
    });
}

$('.btn-search').on('click', (e) => {
    search();
});

$('.txt-search').on('keypress', (e) => {
    if (e.which === 13) {
        search();
        return false;
    }
});


function setStyleSpread(orderState, row, followed) {    
    var color = '';
    if (orderState === l('Booked'))
        color = l('Green');
    if (orderState === l('Cancelled'))
        color = l('Red');
    if (orderState === l('Delayed'))
        color = l('Yellow');
    if (orderState === l('Follow'))
        color = l('Yellow');
    if (orderState === l('Cancelled') && followed === 'Yes')
        color = l('Yellow');

    var style = myTable.getStyle('A' + row);

    if (!style.includes(color)) {
        for (var i = 0; i < IS_READY; i++) {
            myTable.setStyle(jexcel.getColumnNameFromId([i, row - 1]) , 'background-color', color);
        }
        if (orderState === l('Booked'))
            myTable.setStyle(jexcel.getColumnNameFromId([IS_READY, row - 1]), 'visibility', 'hidden');
        else
            myTable.setStyle(jexcel.getColumnNameFromId([IS_READY, row - 1]), 'visibility', 'visible');

    }
}

//create product columns
function prductsColumn(products) {
    var last = IS_READY;
    if (!isLoaded) {
        isLoaded = true;        
        products.forEach(function (item) {
            myTable.insertColumn(1, last++, 0, { type: 'text', width: '100', title: item.name, readOnly: true});
        });
    }
}
///function to populate products
function setProductType(products, productsOrder, arr) {
    products.forEach(function (product) {
        var isSelected = false;
        for (var i = 0; i < productsOrder.length; i++) {
            if (product.id === productsOrder[i].productTypeId.toString()) {
                isSelected = true;
                break;
            }
        }
        if (isSelected)
            arr.push("1");
        else
            arr.push("0");
    });        
    return arr; 
}

