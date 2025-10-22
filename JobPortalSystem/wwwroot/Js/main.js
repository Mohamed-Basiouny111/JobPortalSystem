// <!---------- Dark Mode ---------->
let darkMode = document.querySelector(".dark-mode");
let icon1 = document.querySelector(".dark-mode .dark");
let icon2 = document.querySelector(".dark-mode .light");
let getMode = localStorage.getItem("mode");

if (getMode === "dark") {
    document.body.classList.toggle("dark-mode-active");
    icon1.classList.toggle("active");
    icon2.classList.toggle("active");
}

darkMode.addEventListener("click", function () {
    icon1.classList.toggle("active");
    icon2.classList.toggle("active");
    document.body.classList.toggle("dark-mode-active");

    if (!document.body.classList.contains("dark-mode-active")) {
        return localStorage.setItem("mode", "light");
    }
    localStorage.setItem("mode", "dark");
});

// <!---------- Mobile Menu Bar ---------->
let burger = document.querySelector(".burger-list");
let nav = document.querySelector(".nav-bar");
let navSpan1 = document.querySelector(".burger-list span:nth-of-type(1)");
let navSpan2 = document.querySelector(".burger-list span:nth-of-type(2)");
let navSpan3 = document.querySelector(".burger-list span:nth-of-type(3)");
let links = document.querySelectorAll(".nav-bar .link");

let navBurger = () => {
    nav.classList.toggle("left");
    navSpan1.classList.toggle("close");
    navSpan1.classList.toggle("close1");
    navSpan2.classList.toggle("close2");
    navSpan3.classList.toggle("close");
    navSpan3.classList.toggle("close3");
};
burger.onclick = function (e) {
    navBurger();
};
if (innerWidth <= 767) {
    links.forEach((link) => {
        link.onclick = () => {
            navBurger();
        };
    });
}

// <!---------- Active NavBar Links With Scrolling ---------->
let sections = document.querySelectorAll("section");
let navLinks = document.querySelectorAll(".nav-bar .link");

window.addEventListener("scroll", function () {
    sections.forEach((sec) => {
        let top = window.scrollY;
        let offset = sec.offsetTop;
        let height = sec.offsetHeight;
        let id = sec.getAttribute("id");

        if (top >= offset - 70 && top < offset + height) {
            navLinks.forEach((links) => {
                links.classList.remove("active");
                document
                    .querySelector(".nav-bar .link[href*=" + id + "]")
                    .classList.add("active");
            });
        }
    });
});

// <!---------- Scroll To Top ---------->
let scrollBtn = document.querySelector(".scroll-top");
let header = document.querySelector("header");

window.onscroll = function () {
    if (this.scrollY >= 100) {
        scrollBtn.classList.add("show-scroll");
        header.classList.add("background");
    } else {
        scrollBtn.classList.remove("show-scroll");
        header.classList.remove("background");
    }
};
scrollBtn.onclick = function () {
    window.scrollTo({
        top: 0,
        behavior: "smooth",
    });
};

// Reload Website When Resizing page
window.onresize = function () {
    window.location.reload();
};

// <!--------------- Section Swipers --------------->
if (innerWidth < 768) {
    // <!---------- Team Swiper ---------->
    var swiper = new Swiper(".team .mySwiper", {
        slidesPerView: 1,
        spaceBetween: 30,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false,
        },
        slidesPerGroup: 1,
        loop: true,
        loopFillGroupWithBlank: true,
        pagination: {
            el: ".team .swiper-pagination",
            clickable: true,
        },
        navigation: {
            nextEl: ".team .swiper-button-next",
            prevEl: ".team .swiper-button-prev",
        },
    });
    // <!---------- Testimonials Swiper ---------->
    var swiper = new Swiper(".testimonials .mySwiper", {
        slidesPerView: 1,
        spaceBetween: 30,
        autoplay: {
            delay: 4000,
            disableOnInteraction: false,
        },
        slidesPerGroup: 1,
        loop: true,
        loopFillGroupWithBlank: true,
        pagination: {
            el: ".testimonials .swiper-pagination",
            clickable: true,
        },
    });
    // <!---------- Blogs Swiper ---------->
    var swiper = new Swiper(".blogs .mySwiper", {
        slidesPerView: 1,
        spaceBetween: 10,
        slidesPerGroup: 1,
        navigation: {
            nextEl: ".blogs .swiper-button-next",
            prevEl: ".blogs .swiper-button-prev",
        },
    });
} else if (innerWidth < 992) {
    // <!---------- Team Swiper ---------->
    var swiper = new Swiper(".team .mySwiper", {
        slidesPerView: 2,
        spaceBetween: 30,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false,
        },
        slidesPerGroup: 1,
        loop: true,
        loopFillGroupWithBlank: true,
        pagination: {
            el: ".team .swiper-pagination",
            clickable: true,
        },
        navigation: {
            nextEl: ".team .swiper-button-next",
            prevEl: ".team .swiper-button-prev",
        },
    });
    // <!---------- Testimonials Swiper ---------->
    var swiper = new Swiper(".testimonials .mySwiper", {
        slidesPerView: 2,
        spaceBetween: 30,
        autoplay: {
            delay: 4000,
            disableOnInteraction: false,
        },
        slidesPerGroup: 1,
        loop: true,
        loopFillGroupWithBlank: true,
        pagination: {
            el: ".testimonials .swiper-pagination",
            clickable: true,
        },
    });
    // <!---------- Blogs Swiper ---------->
    var swiper = new Swiper(".blogs .mySwiper", {
        slidesPerView: 3,
        spaceBetween: 10,
        slidesPerGroup: 1,
        navigation: {
            nextEl: ".blogs .swiper-button-next",
            prevEl: ".blogs .swiper-button-prev",
        },
    });
} else if (innerWidth >= 992) {
    // <!---------- Team Swiper ---------->
    var swiper = new Swiper(".team .mySwiper", {
        slidesPerView: 3,
        spaceBetween: 30,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false,
        },
        slidesPerGroup: 1,
        loop: true,
        loopFillGroupWithBlank: true,
        pagination: {
            el: ".team .swiper-pagination",
            clickable: true,
        },
        navigation: {
            nextEl: ".team .swiper-button-next",
            prevEl: ".team .swiper-button-prev",
        },
    });
    // <!---------- Testimonials Swiper ---------->
    var swiper = new Swiper(".testimonials .mySwiper", {
        slidesPerView: 3,
        spaceBetween: 30,
        autoplay: {
            delay: 4000,
            disableOnInteraction: false,
        },
        slidesPerGroup: 1,
        loop: true,
        loopFillGroupWithBlank: true,
        pagination: {
            el: ".testimonials .swiper-pagination",
            clickable: true,
        },
    });
    // <!---------- Blogs Swiper ---------->
    var swiper = new Swiper(".blogs .mySwiper", {
        slidesPerView: 4,
        spaceBetween: 10,
        slidesPerGroup: 1,
        navigation: {
            nextEl: ".blogs .swiper-button-next",
            prevEl: ".blogs .swiper-button-prev",
        },
    });
}

// <!---------- Convert To Arabic ---------->
let lang = document.querySelector(".lang");
let getLang = localStorage.getItem("language");
let styleSheetAR = document.querySelector(".ar-link");

let convertToArabic = () => {
    lang.classList.toggle("arabic");
    if (styleSheetAR.hasAttribute("href")) {
        styleSheetAR.removeAttribute("href");
        lang.innerHTML = `<i class="fa-solid fa-globe"></i> AR`;
    } else {
        styleSheetAR.setAttribute("href", "Css/styleRTL.css");
        lang.innerHTML = `<i class="fa-solid fa-globe"></i> EN`;
    }
    document.body.classList.toggle("rtl");
    document.querySelector("#team").classList.toggle("ltr");
    document.querySelector("#testimonials").classList.toggle("ltr");
    document.querySelector("#blogs").classList.toggle("ltr");
    document.querySelector("#skills").classList.toggle("ltr");
    document.querySelectorAll(".logo").forEach((logo) => {
        logo.classList.toggle("ltr");
    });
};

if (getLang === "arabic") {
    convertToArabic();
}
lang.addEventListener("click", () => {
    convertToArabic();

    if (!lang.classList.contains("arabic")) {
        return localStorage.setItem("language", "english");
    }
    localStorage.setItem("language", "arabic");
});

// <!---------- Smoothing Scroll ---------->
window.addEventListener("scroll", reveal);

function reveal() {
    let reveals = document.querySelectorAll(".reveal");

    for (let i = 0; i < reveals.length; i++) {
        let windowHeight = window.innerHeight;
        let revealTop = reveals[i].getBoundingClientRect().top;
        let revealPoint = 100;

        if (revealTop < windowHeight - revealPoint) {
            reveals[i].classList.add("active");
        } else {
            reveals[i].classList.remove("active");
        }
    }
}

// <!---------- Section About Number Counter ---------->
let sectionAbout = document.querySelector("#about");
let nums = document.querySelectorAll("#about .card .num-counter");
let started = false;

window.addEventListener("scroll", () => {
    if (window.scrollY >= sectionAbout.offsetTop - 200) {
        if (!started) {
            nums.forEach((num) => {
                startCount(num);
            });
        }
        started = true;
    }
});

function startCount(ele) {
    let goal = ele.dataset.goal;
    let count = setInterval(() => {
        ele.textContent++;
        if (ele.textContent == goal) {
            clearInterval(count);
        }
    }, 4000 / goal);
}

// <!---------- Section Skills Skill Counter ---------->
let sectionSkills = document.querySelector("#skills");
let skills = document.querySelectorAll("#skills .bar span");

window.addEventListener("scroll", () => {
    if (window.scrollY >= sectionSkills.offsetTop - 200) {
        skills.forEach((skill) => {
            skill.style.width = skill.dataset.progress;
        });
    }
});

// <!---------- Section Works Filter ---------->
let filters = document.querySelectorAll("#work .menu li");
let workBoxes = document.querySelectorAll("#work .box");

filters.forEach((li) => {
    li.addEventListener("click", removeActive);
    li.addEventListener("click", manageBoxes);
});

function removeActive() {
    filters.forEach((li) => {
        li.classList.remove("active");
        this.classList.add("active");
    });
}
function manageBoxes() {
    workBoxes.forEach((box) => {
        box.style.display = "none";
    });
    document.querySelectorAll(this.dataset.filter).forEach((item) => {
        item.style.display = "block";
    });
}

// <!---------- Alert Contact Section ---------->
if (document.querySelector(".btn-submit")) {

    let nameInput = document.querySelector("input[name='name']");
    let emailInput = document.querySelector("input[name='email']");
    let textArea = document.querySelector("textarea");
    let btnSubmit = document.querySelector(".btn-submit");
    let alert = document.querySelector(".alert");
    let alertTimer = document.querySelector(".alert span");
    let alertExit = document.querySelectorAll(".alert .exit");

    function show(alert, alertTimer) {
        alertTimer.style.transition = "width 5s linear";
        alertTimer.style.width = 0;
        alert.style.transform = "translateX(0)";
    }

    function close(alert, alertTimer) {
        alertTimer.style.transition = "width 0s linear";
        alertTimer.style.width = "100%";
        alert.style.transform = "translateX(calc(350px + 1rem))";
    }

    btnSubmit.addEventListener("click", (e) => {
        if (
            nameInput.value.length !== 0 &&
            emailInput.value.includes("@") &&
            emailInput.value.slice(emailInput.value.indexOf("@"), -1).length >= 1 &&
            textArea.value.length !== 0
        ) {
            setTimeout(() => close(alert, alertTimer), 5000);
            show(alert, alertTimer);
        } else {
            e.preventDefault();
            setTimeout(() => {
                close(
                    document.querySelector(".alert.error"),
                    document.querySelector(".alert.error span")
                );
            }, 5000);
            show(
                document.querySelector(".alert.error"),
                document.querySelector(".alert.error span")
            );
        }
    });

    if (alertExit.length >= 2) {
        alertExit[0]?.addEventListener("click", () => {
            close(
                document.querySelector(".alert.success"),
                document.querySelector(".alert.success span")
            );
        });

        alertExit[1]?.addEventListener("click", () => {
            close(
                document.querySelector(".alert.error"),
                document.querySelector(".alert.error span")
            );
        });
    }
}
