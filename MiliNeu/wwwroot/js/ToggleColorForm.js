function toggleColorForm(show) {
    var colorForm = document.querySelector('.new-color-form');
    var colorSelector = document.querySelector('#colorSelector');
    if (show) {
        colorForm.style.display = 'block';
        colorSelector.style.display = 'none';

    } else {
        colorForm.style.display = 'none';
        colorSelector.style.display = 'block';
    }
}