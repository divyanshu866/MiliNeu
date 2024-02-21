

$(document).ready(function () {
    $("#addToCartForm").submit(function (event) {
        event.preventDefault();

        var formData = $(this).serialize();

        // Now you can send the data to your cart controller via AJAX or any other method
        $.ajax({
            type: "POST",
            url: "/Carts/AddToCart", // Change this URL to your controller endpoint
            data: formData,
            success: function (response) {
                // Handle the response from the server
                console.log(response);
            },
            error: function (error) {
                console.log("Error:", error);
            }
        });
    });
});
