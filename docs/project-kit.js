/*
 * project-kit.js — v1.1
 * Highlights the current section in .pk-subnav. Copy verbatim; load with
 * <script src="./project-kit.js" defer></script>. No dependencies.
 */
(function () {
  var nav = document.querySelector(".pk-subnav");
  if (!nav) return;
  var list = nav.querySelector("ul");
  var links = Array.prototype.slice.call(nav.querySelectorAll('a[href^="#"]'));
  var items = links
    .map(function (a) {
      return { link: a, section: document.getElementById(decodeURIComponent(a.getAttribute("href").slice(1))) };
    })
    .filter(function (it) { return it.section; });
  if (!items.length) return;

  var current = null;
  var ticking = false;

  function setActive(item) {
    if (item === current) return;
    if (current) current.link.removeAttribute("aria-current");
    current = item;
    if (!item) return;
    item.link.setAttribute("aria-current", "true");
    // Keep the active link visible inside the bar without scrolling the page.
    var l = item.link.offsetLeft;
    var r = l + item.link.offsetWidth;
    if (l < list.scrollLeft + 16) list.scrollLeft = l - 16;
    else if (r > list.scrollLeft + list.clientWidth - 24) list.scrollLeft = r - list.clientWidth + 24;
  }

  function update() {
    ticking = false;
    var line = nav.getBoundingClientRect().bottom + Math.max(48, window.innerHeight * 0.15);
    var active = null;
    for (var i = 0; i < items.length; i++) {
      if (items[i].section.getBoundingClientRect().top <= line) active = items[i];
    }
    var atBottom = window.innerHeight + window.scrollY >= document.documentElement.scrollHeight - 2;
    if (atBottom) active = items[items.length - 1];
    setActive(active);
  }

  function onScroll() {
    if (!ticking) {
      ticking = true;
      requestAnimationFrame(update);
    }
  }

  items.forEach(function (it) {
    it.link.addEventListener("click", function () { setActive(it); });
  });

  window.addEventListener("scroll", onScroll, { passive: true });
  window.addEventListener("resize", onScroll);
  update();
})();
