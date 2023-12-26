// custom.js
window.closeSidebarOnClickOutside = () => {
    document.addEventListener('click', function (event) {
        const sidebar = document.getElementById('sidebar');
        const menuIcon = document.querySelector('.menu-icon');

        if (!sidebar.contains(event.target) && event.target !== menuIcon) {
            // Close the sidebar if it's open
            const sidebarClass = sidebar.getAttribute('class');
            if (sidebarClass && sidebarClass.includes('active')) {
                sidebar.classList.remove('active');
            }
        }
    });
};
