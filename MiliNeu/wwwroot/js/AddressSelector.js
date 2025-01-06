
// Handle collapsible functionality
document.addEventListener('DOMContentLoaded', function () {
    var collapsible = document.querySelector('.collapsible');

    collapsible.addEventListener('click', function () {
        event.preventDefault(); // This will prevent the form submission behavior
        var content = this.nextElementSibling;
        content.classList.toggle('active');

    });
    ;
    // Handle the change of "Delivering to" address content when selecting an address
    var radioButtons = document.querySelectorAll('.address-radio');
    radioButtons.forEach(function (radio) {
        radio.addEventListener('click', function () {
            var selectedAddress = this.nextElementSibling;
            var deliveryAddressDiv = document.getElementById('delivery-address');
            // deliveryAddressDiv.innerHTML = `<p>Delivering to: ${selectedAddress}</p>`;
            deliveryAddressDiv.innerHTML = selectedAddress.innerHTML;
            var collapsible = document.querySelector('.collapsible');
            var content = collapsible.nextElementSibling;
            content.classList.toggle('active');
        });
    });
});
