// Scroll-reveal helper: watches elements with the "reveal" class and adds
// "reveal--visible" once they enter the viewport. Also handles the
// gallery lightbox toggle used on the Gallery page.
window.siteInterop = {
    registerScrollReveal: function () {
        const elements = document.querySelectorAll('.reveal:not([data-reveal-bound])');
        if (!('IntersectionObserver' in window)) {
            elements.forEach(el => el.classList.add('reveal--visible'));
            return;
        }

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('reveal--visible');
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.15 });

        elements.forEach(el => {
            el.setAttribute('data-reveal-bound', 'true');
            observer.observe(el);
        });
    },

    toggleNav: function (isOpen) {
        const nav = document.querySelector('.nav-links');
        if (!nav) return;
        nav.classList.toggle('open', isOpen);
    }
};
