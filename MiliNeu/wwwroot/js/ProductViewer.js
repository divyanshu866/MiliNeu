function changeImage(imageUrl) {
    // Change the main image
    const mainImage = document.getElementById("mainImage");
    mainImage.src = imageUrl;

    // Remove "active" class from all thumbnails
    const thumbnails = document.querySelectorAll(".thumbnail");
    thumbnails.forEach(thumbnail => {
        thumbnail.classList.remove("active");
    });

    // Add "active" class to the clicked thumbnail
    const clickedThumbnail = Array.from(thumbnails).find(thumb => thumb.src.includes(imageUrl));
    clickedThumbnail.classList.add("active");
}
