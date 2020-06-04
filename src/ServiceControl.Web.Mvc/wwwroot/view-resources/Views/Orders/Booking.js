(function ($) {

})(jQuery);

$("#CompanyId").bsMultiSelect();
$("#OrderStateId").bsMultiSelect();

var l = abp.localization.getSource('ServiceControl');
var _spreadsheet = 'spreadsheet';
var isBookingAdmin = abp.auth.isGranted('Pages.Booking.Admin');
var isOrderAdminReady = abp.auth.isGranted('Order.Admin.Ready');
var isOrderAdminInvoice = abp.auth.isGranted('Order.Admin.Invoice');
var FOLLOWED = 0;
var EXPLANATION = 0;
var IS_READY_COLUMN = 0;
var isLoaded = false;
var PRODUCTS_COUNT = 0;
var TOTAL_COLUMNS = 0;

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
    column = column + PRODUCTS_COUNT;
    model.timeSlot = myTable.getValueFromCoords([column++], [y]);
    model.notes = myTable.getValueFromCoords([column++], [y]);
    model.orderNo = myTable.getValueFromCoords([column++], [y]);
    model.accountNo = myTable.getValueFromCoords([column++], [y]);
    model.installDate = myTable.getValueFromCoords([column++], [y]);
    model.orderStateName = myTable.getValueFromCoords([column++], [y]);
    model.remarks = myTable.getValueFromCoords([column++], [y]);
    model.followed = myTable.getValueFromCoords([column++], [y]);
    model.explanation = myTable.getValueFromCoords([column++], [y]);
    if (isOrderAdminInvoice) {
        model.PaymentStatusName = myTable.getValueFromCoords([column++], [y]);
        model.invoiceNo = myTable.getValueFromCoords([column++], [y]);
    }
    if (isOrderAdminReady) {
        model.isReady = myTable.getValueFromCoords([column++], [y]);
    }

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
    search: true,
    pagination: 10,
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
        createColumns(result.result.productType);
        myTable.insertRow([[]], 0);//insert new empty row after first row
        myTable.deleteRow(0, 1);//delete first row
        if (myTable.records.length !== 1) {
            myTable.deleteRow(1, myTable.records.length);//delete all rows except first row
        }
        var row = 2;
        result.result.data.items.forEach(function (item) {
            var data = [];
            data.push(item.id);
            data.push(item.company.name);
            data.push(item.serial);
            data.push(item.dateBooked);
            data.push(item.sgi);
            data.push(item.customerFirstName);
            data.push(item.customerLastName);
            data.push(item.contactPhone);
            data.push(item.email);
            data.push(item.dateOfBirth);
            data.push(item.firstIdentification.name);
            data.push(item.secondIdentification.name);
            data.push(item.existingAccountNo);
            data.push(item.streetNo);
            data.push(item.customerAddress);
            data.push(item.unit);
            data.push(item.city);
            data.push(item.postalCode);
            data.push(item.promoDetails);
            setProductType(result.result.productType, item.ordersProductType, data);
            data.push(item.timeSlot.name);
            data.push(item.notes);
            data.push(item.orderNo);
            data.push(item.accountNo);
            data.push(item.installDate);
            data.push(item.orderState.name);
            data.push(item.remarks);
            data.push(item.followed);
            data.push(item.explanation);
            if (isOrderAdminInvoice) {
                data.push(item.paymentStatus.name);
                data.push(item.invoiceNo);
            }

            if (isOrderAdminReady) {
                data.push($.parseJSON(item.isReady.toLowerCase()));
            }
            

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
    if (orderState === l('Disconnected'))
        color = l('Purple');

    var style = myTable.getStyle('A' + row);

    if (!style.includes(color)) {
        for (var i = 0; i < TOTAL_COLUMNS; i++) {
            myTable.setStyle(jexcel.getColumnNameFromId([i, row - 1]) , 'background-color', color);
        }
        if (isOrderAdminReady) {
            style = myTable.getStyle(getColumnName(IS_READY_COLUMN, row - 1)); 
            if (orderState === l('Booked') && !style.includes('hidden'))
                myTable.setStyle(getColumnName(IS_READY_COLUMN, row - 1), 'visibility', 'hidden');
            if (orderState !== l('Booked') && !style.includes('visible'))
                myTable.setStyle(getColumnName(IS_READY_COLUMN, row - 1), 'visibility', 'visible');
        }

    }
}

function getColumnName(col, row) {
    return jexcel.getColumnNameFromId([col, row]);
}

//create product columns
function createColumns(products) {
    if (!isLoaded) {
        isLoaded = true;        

        var column = 1;
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '50', title: l('Id'), readOnly: true });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('Company'), readOnly: true, });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('Serial'), readOnly: true, });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('DateBooked'), readOnly: true, });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('Sgi'), readOnly: true, });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('CustomerFirstName') });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('CustomerLastName') });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('ContactPhone') });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('Email') });
        myTable.insertColumn(1, column++, 0, { type: 'calendar', width: '150', title: l('DateOfBirth'), options: { format: l('DateFormatView') } });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('FirstIdentification'), readOnly: true, });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('SecondIdentification'), readOnly: true, });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('ExistingAccountNo') });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('StreetNo') });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('CustomerAddress') });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('Unit') });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('City') });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('PostalCode') });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('PromoDetails') });
        products.forEach(function (item) {
            myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: item.name, readOnly: true });
            PRODUCTS_COUNT++;
        });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('TimeSlot'), readOnly: true, });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '200', title: l('Notes'), wordWrap: true });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('OrderNo'), readOnly: !isBookingAdmin });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('AccountNo'), readOnly: !isBookingAdmin });
        myTable.insertColumn(1, column++, 0, { type: 'calendar', width: '200', title: l('InstallDate'), options: { format: l('DateTimeFormatView'), time: 1 }, readOnly: !isBookingAdmin });
        myTable.insertColumn(1, column++, 0, {
            type: 'dropdown', width: '150', title: l('OrderState'), readOnly: !isBookingAdmin, source: [
                l("Booked"),
                l("Cancelled"),
                l("Delayed"),
                l("Follow"),
                l("Disconnected"),
            ]
        });
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('Remarks'), wordWrap: true, readOnly: !isBookingAdmin });
        myTable.insertColumn(1, column++, 0, {
                type: 'dropdown', width: '150', title: l('Followed'), source: [
                    l("Yes"),
                    l("No"),
                ]
        });
        FOLLOWED = column - 2;
        myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('Explanation'), wordWrap: true });
        EXPLANATION = column - 2;
        if (isOrderAdminInvoice) {
            myTable.insertColumn(1, column++, 0, {
                type: 'dropdown', width: '150', title: l('PaymentStatus'), source: [
                    l("Pending"),
                    l("Done"),
                    l("Deduction"),
                ]
            });
            myTable.insertColumn(1, column++, 0, { type: 'text', width: '150', title: l('InvoiceNo'), readOnly: !isOrderAdminInvoice });
        }
        if (isOrderAdminReady) {
            myTable.insertColumn(1, column++, 0, { type: 'checkbox', width: '150', title: l('IsReady') });
            IS_READY_COLUMN = column - 2;
        }
        TOTAL_COLUMNS = column;
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

