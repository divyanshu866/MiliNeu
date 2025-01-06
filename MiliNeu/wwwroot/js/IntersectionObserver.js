const products = document.querySelectorAll('.fade-out-left');
const columns = 4; // Set the number of columns in your grid layout
const delayStep = 0.15; // Delay step in seconds (e.g., 0.1s, 100ms)
const rowBaseDelay = 0.4; // Base delay increment for rows

const observer = new IntersectionObserver((entries) => {
	entries.forEach((entry) => {

		if (entry.isIntersecting) {
            // Check for specific classes or attributes to apply different animations
            if (entry.target.classList.contains('fade-out-left')) {
                products.forEach((product, index) => {
                    const column = index % columns; // Column number
                    const row = Math.floor(index / columns); // Row number

                    const delay = row * rowBaseDelay + column * delayStep; // Total delay
                    product.style.transitionDelay = `${delay}s`;

                    // Add the animation class
                    product.classList.add('fade-in-left');
                });
                

                

                entry.target.classList.add('fade-in-left');
            } else if (entry.target.classList.contains('fade-out-bottom')) {
                entry.target.classList.add('fade-in-bottom');
            }else if (entry.target.classList.contains('fade-out')) {
                entry.target.classList.add('fade-in');
            } else if (entry.target.classList.contains('fade-out-right')) {
                entry.target.classList.add('fade-in-right');
            }

            else if (entry.target.dataset.animation === 'slide') {
                entry.target.classList.add('slide-show');
            }





            


		}
	

	});
});

const hiddenElements = document.querySelectorAll(".fade-out-left");
hiddenElements.forEach((el) => observer.observe(el))

const hiddenElements1 = document.querySelectorAll(".fade-out-bottom");
hiddenElements1.forEach((el) => observer.observe(el))

const hiddenElements2 = document.querySelectorAll(".fade-out");
hiddenElements2.forEach((el) => observer.observe(el))

const hiddenElements3 = document.querySelectorAll(".fade-out-right");
hiddenElements3.forEach((el) => observer.observe(el))