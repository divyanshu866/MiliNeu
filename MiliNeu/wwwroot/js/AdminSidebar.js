
    document.addEventListener('DOMContentLoaded', () => {
        // Correctly select the checkbox element by ID
        const checkbox = document.querySelector('#sidebarToggle');
    let sidebar = document.querySelector('.sidebar-wrapper');
        // Use the 'change' event listener to capture state changes
        checkbox.addEventListener('change', () => {
            if (checkbox.checked) {
        console.log("The checkbox is now checked.");
    sidebar.classList.add('sidebar-hide');
    sidebar.classList.remove('sidebar-show');
            } else {
        console.log("The checkbox is now unchecked.");
    sidebar.classList.add('sidebar-show');
    sidebar.classList.remove('sidebar-hide');
            }
        });
    });
