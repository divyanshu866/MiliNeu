
/*SIZE OVERLAY.............................*/




// Function to show the size chart overlay
function showChart() {
    const Chart = document.getElementById('sizeChartOverlay');
    const navbar = document.getElementsByTagName('nav')[0];
    Chart.classList.remove('chart-hide');
    Chart.classList.add('chart-visible'); // Remove the 'visible' class to hide the overlay
    document.documentElement.style.overflow = "hidden";
    if (!navbar.classList.contains("hidden")) {
        navbar.classList.add("hidden");

	}
}
// Function to close the size chart overlay
function closeChart(btn) {
    const Chart = document.getElementById('sizeChartOverlay');
    

    document.documentElement.style.overflow = "auto";


    Chart.classList.add('chart-hide');
    Chart.classList.remove('chart-visible'); // Remove the 'visible' class to hide the overlay
}
