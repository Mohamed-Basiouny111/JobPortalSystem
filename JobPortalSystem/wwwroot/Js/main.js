// <!---------- Dark Mode ---------->
let darkMode = document.querySelector(".dark-mode");
let icon1 = document.querySelector(".dark-mode .dark");
let icon2 = document.querySelector(".dark-mode .light");
let getMode = localStorage.getItem("mode");

if (darkMode && icon1 && icon2) {
    if (getMode === "dark") {
        document.body.classList.add("dark-mode-active");
        icon1.classList.add("active");
        icon2.classList.add("active");
    }

    darkMode.addEventListener("click", function () {
        icon1.classList.toggle("active");
        icon2.classList.toggle("active");
        document.body.classList.toggle("dark-mode-active");

        localStorage.setItem("mode", document.body.classList.contains("dark-mode-active") ? "dark" : "light");
    });
}

// <!---------- Mobile Menu Bar ---------->
let burger = document.querySelector(".burger-list");
let nav = document.querySelector(".nav-bar");
let navSpan1 = document.querySelector(".burger-list span:nth-of-type(1)");
let navSpan2 = document.querySelector(".burger-list span:nth-of-type(2)");
let navSpan3 = document.querySelector(".burger-list span:nth-of-type(3)");
let links = document.querySelectorAll(".nav-bar .link");

if (burger && nav && navSpan1 && navSpan2 && navSpan3) {
    let navBurger = () => {
        nav.classList.toggle("left");
        navSpan1.classList.toggle("close");
        navSpan1.classList.toggle("close1");
        navSpan2.classList.toggle("close2");
        navSpan3.classList.toggle("close");
        navSpan3.classList.toggle("close3");
    };
    burger.addEventListener("click", navBurger);

    if (innerWidth <= 767) {
        links.forEach((link) => {
            link.addEventListener("click", navBurger);
        });
    }
}


