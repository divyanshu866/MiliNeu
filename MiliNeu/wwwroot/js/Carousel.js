let currentSlide = 0;
const slides = document.querySelectorAll('.carousel-images .item-carousel');
const totalSlides = slides.length;

function showSlide(index) {
    // Loop to the beginning/end if index goes out of bounds
    currentSlide = (index + totalSlides) % totalSlides;

    // Move the carousel-images container to the correct position
    const carouselImages = document.querySelector('.carousel-images');
    carouselImages.style.transform = `translateX(${-currentSlide * 100}%)`;
}

function changeSlide(n) {
    showSlide(currentSlide + n);
}

// Auto-slide every 3 seconds
let autoSlide = setInterval(() => changeSlide(1), 10000000);

// Pause auto-slide on hover
const carousel = document.querySelector('.carousel');
carousel.addEventListener('mouseenter', () => clearInterval(autoSlide));
carousel.addEventListener('mouseleave', () => autoSlide = setInterval(() => changeSlide(1), 300000));

// Initial display of the first slide
showSlide(0);
