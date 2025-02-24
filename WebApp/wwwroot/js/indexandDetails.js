const { type, get } = require("jquery");

function calculatetotal() {
    var price = parseFloat(document.getElementById("popup-price").value);
    var quantity = parseInt(document.getElementById("popup-qty").value);
    var total = price * quantity;
    document.getElementById("popup-total").value = total;
};

function calculateNet() {
    var total = document.getElementById("popup-total").value;
    var discount = (document.getElementById("popup-discount").value) / 100;
    var net = total - (total * discount);

    document.getElementById("popup-net").value = net;
};
function calculatetotalnet() {
    let netInputs = document.querySelectorAll('.net');
    let totalNet = document.getElementById('totalNet');

    let sum = 0;
    netInputs.forEach(input => {
        let value = parseFloat(input.value) || 0;
        sum += value;
    });

    totalNet.value = sum;
}
function calculatetotalnetaftertaxes() {
    let Totalnet = parseFloat(document.getElementById("totalNet").value);
    console.log(Totalnet);
    let Taxes = parseFloat((document.getElementById("Taxes").value) / 100);
    console.log(Taxes);
    let totalNetaftertaxes = document.getElementById("totalNetAftertaxes");
    totalNetaftertaxes.value = Totalnet + (Totalnet * Taxes);

}
function SaveInvoice() {
    console.log("enter SAveinvoice function");
    let store = document.getElementById("storeDropdown");
    let storeid = store.options[store.selectedIndex].value;
    var invoiceData = {
        InvoiceNO: document.getElementById("invoiceNo").value,
        InvoiceDate: document.getElementById("invoiceDate").value,
        nettotal: document.getElementById("totalNet").value,
        taxes: document.getElementById("Taxes").value,
        NetAftertaxes: document.getElementById("totalNetAftertaxes").value,
        storeid: storeid,
        invoiceitems: []
    };

    $("tbody tr").each(function () {
        var item = {
            itemid: $(this).find("[name='itemid']").val(),
            unitid: $(this).find("[name='unitid']").val(),
            price: $(this).find("[name='price']").val(),
            qty: $(this).find("[name='qty']").val(),
            total: $(this).find("[name='total']").val(),
            discount: $(this).find("[name='discount']").val(),
            net: $(this).find("[name='itemnet']").val()
        };

        if (item.itemid && item.unitid) {
            invoiceData.invoiceitems.push(item);
        }
    });



    $.ajax({
        url: '/Invoice/Create',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(invoiceData),
        success: function (response) {
            debugger;
            if (response.success) {




         
                    Swal.fire({
                        title: "Success",
                        text: "Invoice Saved Succefully",
                        icon: "success",
                        showCancelButton: false,
                        confirmButtonColor: "#d33",
                        confirmButtonText: "OK"
                    }).then(
                        (result) => {
                            if (result.isConfirmed) {
                                window.location.href = "/Store/Index"; // Proceed with deletion
                            }
                        }
                    )





                    
                







            }
        }
    });
}




function updateDropdowns() {
    var storeDropdown = document.getElementById("storeDropdown");
    var selectedOption = storeDropdown.options[storeDropdown.selectedIndex];

    var itemDropdown = document.getElementById("itemDropdown");
    var unitDropdown = document.getElementById("unitDropdown");

    // Clear previous options
    itemDropdown.innerHTML = '<option value="">-- Select Item --</option>';
    unitDropdown.innerHTML = '<option value="">-- Select Unit --</option>';

    if (selectedOption.value) {
        var items = JSON.parse(selectedOption.getAttribute("data-items"));
        var units = JSON.parse(selectedOption.getAttribute("data-units"));

        items.forEach(item => {
            var option = document.createElement("option");
            option.value = item.itemid;
            option.text = item.itemname;
            itemDropdown.appendChild(option);
        });

        units.forEach(unit => {
            var option = document.createElement("option");
            option.value = unit.unitid;
            option.text = unit.unitname;
            unitDropdown.appendChild(option);
        });
    }
}


function openPopup() {
    let storedropdown = document.getElementById("storeDropdown");
    var selectedOption = storeDropdown.options[storeDropdown.selectedIndex];

    if (selectedOption.value) {
        document.getElementById("popup").style.display = "block";
        document.getElementById("overlay").style.display = "block";
        updateDropdowns();
    }
    else {
        window.alert("You Must Select A Store First");
    }

}

function closePopup() {

    var itemDropdown = document.getElementById("itemDropdown");
    var unitDropdown = document.getElementById("unitDropdown");
    itemDropdown.innerHTML = '<option value="">-- Select Item --</option>';
    unitDropdown.innerHTML = '<option value="">-- Select Unit --</option>';
    document.getElementById("popup-price").value = 0;
    document.getElementById("popup-qty").value = 1;
    document.getElementById("popup-total").value = 0;
    document.getElementById("popup-discount").value = 0;
    document.getElementById("popup-net").value = 0;
    document.getElementById("popup").style.display = "none";
    document.getElementById("overlay").style.display = "none";
}



function additemtoinvoice() {
    var tableBody = document.getElementById("tablebodyid");

    var itemDropdown = document.getElementById("itemDropdown");
    var unitDropdown = document.getElementById("unitDropdown");

    var itemText = itemDropdown.options[itemDropdown.selectedIndex].text;
    var itemValue = itemDropdown.options[itemDropdown.selectedIndex].value;

    var unitText = unitDropdown.options[unitDropdown.selectedIndex].text;
    var unitValue = unitDropdown.options[unitDropdown.selectedIndex].value;

    var price = document.getElementById("popup-price").value;
    var qty = document.getElementById("popup-qty").value;
    var total = document.getElementById("popup-total").value;

    var discount = document.getElementById("popup-discount").value;
    var net = document.getElementById("popup-net").value;

    // Ensure an item and unit are selected before adding
    if (itemValue === "" || unitValue === "") {
        alert("Please select both an item and a unit.");
        return;
    }

    // Create a new row
    var newRow = document.createElement("tr");
    // Add table columns with values
    newRow.innerHTML = `
                        <td><input type="hidden" name="itemid" value="${itemValue}" />
                            <input value="${itemText}" name="Item" type="text" class="item" readonly /></td>

                        <td><input type="hidden" name="unitid" value="${unitValue}" />
                            <input value="${unitText}" name="Unit" type="text" class="unit" readonly /></td>

                        <td><input value="${price}" name="price" type="number" class="price" readonly /></td>
                        <td><input value="${qty}" name="qty" type="number" class="qty" readonly /></td>
                        <td><input value="${total}" name="total" type="text" class="total" readonly /></td>
                        <td><input value="${discount}" name="discount" type="number" class="discount" readonly /></td>
                        <td><input value="${net}" name="itemnet" type="number" class="net" onchange="calculatetotalnet()" readonly /></td>
                    `;
    tableBody.appendChild(newRow);

    closePopup();
    itemDropdown.innerHTML = '<option value="">-- Select Item --</option>';
    unitDropdown.innerHTML = '<option value="">-- Select Unit --</option>';
    document.getElementById("popup-price").value = 0;
    document.getElementById("popup-qty").value = 1;
    document.getElementById("popup-total").value = 0;
    document.getElementById("popup-discount").value = 0;
    document.getElementById("popup-net").value = 0;

    calculatetotalnet();
    calculatetotalnetaftertaxes();

}





function resetFields() {


    document.querySelectorAll('input').forEach(input => {
        if (input.type === 'number' || input.type === 'text') {
            input.value = '';
        }

        else if (input.type === 'date') {
            input.value = new Date().toISOString().split('T')[0];
        }
        if (input.id === ('Taxes')) {
            input.value = 14;
        }
        if (input.id === ('popup-qty')) {
            input.value = 1;
        }
    });


    document.getElementById('tablebodyid').innerHTML = '';
    document.getElementById('totalNet').value = '';
    document.getElementById('totalNetAftertaxes').value = '';


}








