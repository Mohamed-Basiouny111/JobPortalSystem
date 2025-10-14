let sidebarFilters = document.querySelectorAll(".sidebar li");
let pages = document.querySelectorAll(".content .page");

sidebarFilters.forEach((li) => {
  li.addEventListener("click", removeSidebarActive);
  li.addEventListener("click", managePages);
});
function removeSidebarActive() {
  sidebarFilters.forEach((li) => {
    li.classList.remove("active");
    this.classList.add("active");
  });
}
function managePages() {
  pages.forEach((box) => {
    box.style.cssText = "display: none";
  });
  document.querySelectorAll(this.dataset.filter).forEach((item) => {
    item.style.cssText = "display: block";
  });
}
