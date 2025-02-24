
function calculateupdatedtotal() {
    let totalNet = 0; // لتجميع إجمالي الفاتورة
    let rows = document.querySelectorAll("#tablebodyid tr");

    rows.forEach(row => {
        let priceInput = row.querySelector(".price");
        let qtyInput = row.querySelector(".qty");
        let totalInput = row.querySelector(".total");

        if (priceInput && qtyInput && totalInput) {
            let price = parseFloat(priceInput.value) || 0;
            let qty = parseFloat(qtyInput.value) || 0;
            let total = price * qty;

            totalInput.value = total.toFixed(2); // تحديث قيمة الإجمالي لكل صف
        }
    });
}
function calculateupdatedNet() {
    let rows = document.querySelectorAll("#tablebodyid tr");

    rows.forEach(row => {
        let totalInput = row.querySelector(".total");
        let discountInput = row.querySelector(".discount");
        let netInput = row.querySelector(".net");

        if (totalInput && discountInput && netInput) {
            let total = parseFloat(totalInput.value) || 0;
            let discount = parseFloat(discountInput.value) || 0;

       
            let net = total - (total * (discount / 100));
            netInput.value = net.toFixed(2);

        }
    });

     
}

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
function enableEditing() {

    document.querySelectorAll('input[readonly], select[readonly]').forEach(element => {
        if (element.id !== "invoiceNo") {
            element.removeAttribute('readonly');
        }
    });

}
function updateDropdownswhenstoreisSelected() {
    console.log("Store dropdown changed!");
    var storeDropdown = document.getElementById("storeDropdown");
    var selectedOption = storeDropdown.options[storeDropdown.selectedIndex];

    if (!selectedOption.value) return; // Exit if no store is selected

    var items = JSON.parse(selectedOption.getAttribute("data-items"));
    var units = JSON.parse(selectedOption.getAttribute("data-units"));

    // Loop through all table rows and update dropdowns
    document.querySelectorAll("#tablebodyid tr").forEach((row) => {
        var itemDropdown = row.querySelector(".itemDropdown");
        var unitDropdown = row.querySelector(".unitDropdown");

        var selectedItemId = itemDropdown.getAttribute("data-selected");
        var selectedUnitId = unitDropdown.getAttribute("data-selected");

        // Clear and refill the item dropdown
        itemDropdown.innerHTML = '<option value="">-- Select Item --</option>';
        items.forEach(item => {
            var option = document.createElement("option");
            option.value = item.itemid;
            option.text = item.itemname;
            if (item.itemid == selectedItemId) {
                option.selected = true;
            }
            itemDropdown.appendChild(option);
        });

        // Clear and refill the unit dropdown
        unitDropdown.innerHTML = '<option value="">-- Select Unit --</option>';
        units.forEach(unit => {
            var option = document.createElement("option");
            option.value = unit.unitid;
            option.text = unit.unitname;
            if (unit.unitid == selectedUnitId) {
                option.selected = true;
            }
            unitDropdown.appendChild(option);
        });
    });
}

function addnewitemtoinvoice() {
    debugger;

    var itemData = {
        itemid: $('#itemDropdown').val(),
        unitid: $('#unitDropdown').val(),
        price: $('#popup-price').val(),
        qty: $('#popup-qty').val(),
        total: $('#popup-total').val(),
        discount: $('#popup-discount').val(),
        net: $('#popup-net').val()
    };

    if (itemData.itemid === "" || itemData.unitid === "") {
        alert("Please select both an item and a unit.");
        return;
    }

    // Create a new row
    var newRow = document.createElement("tr");
    // Add table columns with values
    newRow.innerHTML = `<tr>
                        <td>
                            <select class="form-control itemDropdown">
                                <option value="${itemData.itemid}" selected>${$('#itemDropdown option:selected').text()}</option>
                            </select>
                        </td>
                        <td>
                            <select class="form-control unitDropdown">
                                <option value="${itemData.unitid}" selected>${$('#unitDropdown option:selected').text()}</option>
                            </select>
                        </td>
                        <td><input name="price" type="number" value="${itemData.price}" class="price" readonly /></td>
                        <td><input name="qty" type="number" value="${itemData.qty}" class="qty" readonly /></td>
                        <td><input type="text" class="total" value="${itemData.total}" readonly /></td>
                        <td><input name="discount" type="number" value="${itemData.discount}" class="discount" readonly /></td>
                        <td><input name="net" type="number" value="${itemData.net}" class="net" readonly /></td>
                    </tr>`;


    $('#tablebodyid').append(newRow);

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
function UpdateInvoice() {
    console.log("EnterUpdateFunction");
    var invoiceData = {
        invoiceid: $('#invoiceidfield').val(),
        InvoiceNO: $('#invoiceNo').val(),
        InvoiceDate: $('#invoiceDate').val(),
        nettotal: $('#totalNet').val(),
        taxes: $('#Taxes').val(),
        NetAftertaxes: $('#totalNetAftertaxes').val(),
        storeid: $('#storeDropdown').val(),
        invoiceitems: []
    };

    // Iterate over each row in the table body
    $('#tablebodyid tr').each(function () {
        var item = {
            itemid: $(this).find('.itemDropdown').val(),
            unitid: $(this).find('.unitDropdown').val(),
            price: $(this).find('.price').val(),
            qty: $(this).find('.qty').val(),
            total: $(this).find('.total').val(),
            discount: $(this).find('.discount').val(),
            net: $(this).find('.net').val()
        };

        // Ensure both itemid and unitid are present before adding
        if (item.itemid && item.unitid) {
            invoiceData.invoiceitems.push(item);
        }
    });

    // Send the collected data via AJAX
    $.ajax({
        url: '/Invoice/Update',
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(invoiceData),
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    title: "Success",
                    text: "Invoice Updated Successfully",
                    icon: "success",
                    confirmButtonColor: "#d33",
                    confirmButtonText: "OK"
                });
            } else {
                Swal.fire({
                    title: "Failed",
                    text: "Update Failed",
                    icon: "error",
                    confirmButtonColor: "#3085d6",
                    confirmButtonText: "OK"
                });
            }
        }
      
        
    });
}


////function openUpdatedPopup() {
////    console.log("enter openUpdatedPopup()");
////    document.getElementById("Updatepopup").style.display = "block";
////    document.getElementById("updateoverlay").style.display = "block";
////}

////function closeUpdatedPopup() {
////    document.getElementById("Updatepopup").style.display = "none";
////    document.getElementById("popup").style.display = "none";
////    document.getElementById("updateoverlay").style.display = "none";
////}

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