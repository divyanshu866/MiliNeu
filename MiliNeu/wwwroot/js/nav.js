



























document.addEventListener("DOMContentLoaded", function () {
    const navbar = document.getElementsByTagName('nav')[0];
    if (window.location.pathname.includes("/Login") || window.location.pathname.includes("/Register")) {
        navbar.style.display = "none";
        console.log("nav-none");
    }
    else {
        if (window.location.pathname == "/" || window.location.pathname == "/Home/Index" || window.location.pathname == "/Home/") { // Adjust as necessary for your routing

            navbar.classList.add('nav-top');

            window.addEventListener('scroll', () => {
                const currentScrollY = window.scrollY;

                // If at the top of the page, make navbar transparent
                if (currentScrollY <= 200) {
                    navbar.classList.remove('nav-scrolled');
                    navbar.classList.add('nav-top');

                }
                // If scrolled down, make navbar white
                else {
                    navbar.classList.add('nav-scrolled');
                    navbar.classList.remove('nav-top');

                }
            });



        } else {
            navbar.classList.add('nav-scrolled');

            navbar.classList.remove('nav-top');
        }

        if (window.location.pathname.includes("/Identity/Account") || window.location.pathname.includes("/Orders")) {
            navbar.classList.add("nav-manage");
            navbar.classList.remove("nav-top");
            navbar.classList.remove("nav-scrolled");
            navbar.classList.remove("nav");
            console.log("/Identity :" + window.location.pathname.includes("/Identity"));
            console.log("/Orders :" + window.location.pathname.includes("/Orders"));
            console.log("PathName :" + window.location.pathname);

        }
        else {
            navbar.classList.remove("nav-manage");
            console.log(`Esledcnsnd`);
            console.log("PathName :" + window.location.pathname);

        }



        let lastScrollTop = 0;
        window.addEventListener('scroll', () => {
            const scrollTop = document.documentElement.scrollTop;
            if (lastScrollTop<50) {
                // Scrolling down
                if (navbar.classList.contains('hidden')) {
                    navbar.classList.remove('hidden');
                }
            }
            else if (scrollTop > lastScrollTop) {
                // Scrolling up
                if (!navbar.classList.contains('hidden')) {
                    navbar.classList.add('hidden');
				}
                
            } else if (scrollTop < lastScrollTop) {
                // Scrolling down
                if (navbar.classList.contains('hidden')) {
                    navbar.classList.remove('hidden');
                }
                
            }



            lastScrollTop = scrollTop;
        });


        //Block Scrolling
        const searchToggle = document.getElementById("search-toggle");
        const sidebarToggle = document.getElementById("sidebar-toggle");
        searchToggle.addEventListener("change", () => {
            document.documentElement.style.overflow = searchToggle.checked ? "hidden" : "";
        });
        sidebarToggle.addEventListener("change", () => {
            document.documentElement.style.overflow = sidebarToggle.checked ? "hidden" : "";
        });
    }
 
    

    
    
});








