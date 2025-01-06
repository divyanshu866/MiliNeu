function submitForm() {
    // Get the selected orders
    const selectedOrders = Array.from(document.querySelectorAll('input[name="selectedOrders"]:checked'))
        .map(checkbox => checkbox.value);

    // Get the new status
    const newStatus = document.getElementById('new-status').value;

    // Check if there are selected orders and a status selected
    if (selectedOrders.length === 0 || !newStatus) {
        alert('Please select at least one order and a status.');
        return;
    }

    // Create a hidden input to hold the selected orders
    const form = document.getElementById('orderForm');

    // Create a hidden input to hold the new status
    const statusInput = document.createElement('input');
    statusInput.type = 'hidden';
    statusInput.name = 'newStatus';
    statusInput.value = newStatus;
    form.appendChild(statusInput);

    // Submit the form
    form.submit();
}