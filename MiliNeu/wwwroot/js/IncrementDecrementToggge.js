function incrementQuantity() {
    const input = document.querySelector('.quantity-input');
    let currentValue = parseInt(input.value);
    input.value = currentValue + 1;

    // Ensure the decrement button is enabled when incrementing
    const decrementButton = document.querySelector('.quantity-decrement');
    decrementButton.classList.remove('deactive');
}

function decrementQuantity() {
    const input = document.querySelector('.quantity-input');
    let currentValue = parseInt(input.value);

    if (currentValue > 1) { // Prevent decrementing below 1
        input.value = currentValue - 1;
    }

    // Disable the decrement button if value is 1
    if (parseInt(input.value) === 1) {
        const decrementButton = document.querySelector('.quantity-decrement');
        decrementButton.classList.add('deactive');
    }
}
