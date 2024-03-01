const toggleButton = document.getElementsByClassName("toggle-button")[0]
const navbarLinks1 = document.getElementsByClassName("navbar-links")[0]
const navbarLinks2 = document.getElementsByClassName("navbar-links")[1]

toggleButton.addEventListener("click",()=>{
    navbarLinks1.classList.toggle("active")
    navbarLinks2.classList.toggle("active")
})



let subMenu = document.getElementById("subMenu");

function toggleMenu() {
    console.log(subMenu);
    subMenu.classList.toggle("open-menu");
}
