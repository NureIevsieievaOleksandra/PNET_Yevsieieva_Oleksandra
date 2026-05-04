// Highlight active nav link
document.addEventListener('DOMContentLoaded', function () {
    const path = window.location.pathname.split('/')[1];
    document.querySelectorAll('.nav-link').forEach(link => {
        const href = link.getAttribute('href').replace('/', '');
        if (href && path.toLowerCase() === href.toLowerCase()) {
            link.style.color = 'var(--accent)';
            link.style.background = 'rgba(232,180,0,0.08)';
        }
    });
});
