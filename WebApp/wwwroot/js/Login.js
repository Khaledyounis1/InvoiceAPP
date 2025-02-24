function Login() {
    debugger

    console.log("enter Login function");
   
    var LoginData = {
   
        UserName: document.getElementById("usernamefield").value,
        Password: document.getElementById("passwordfield").value
    };

    $.ajax({
        url: '/Account/Login',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(LoginData),
        success: function (response) {
            if (response.success) {
                window.location.href = '/Invoice/AllInvoices';
            }
            else {
                // Display error message if login fails
                $("#error-message").text("Incorrect username or password").show();
            }
        },
        error: function () {
            // Handle error response
            $("#error-message").text("Incorrect username or password").show();
        
        }
    });
}