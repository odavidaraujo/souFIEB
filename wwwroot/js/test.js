document.addEventListener("DOMContentLoaded", function (event) {
    // Hide submenus
    $("#body-row .collapse").collapse("hide");

    // Collapse/Expand icon
    $("#collapse-icon").addClass("fa-angle-double-left");

    // Collapse click
    $('nav.navbar').click(function () {
        SidebarCollapse();
    });

    $(document).ready(function () {
        let ultimaPosicaoScroll = 0;

        $(window).scroll(function () {
            const posicaoAtualScroll = $(window).scrollTop();

            if (posicaoAtualScroll > ultimaPosicaoScroll) {
                // Role para baixo
                $("#header").css("top", "-100px"); // ou "0" para esconder completamente
            } else {
                // Role para cima
                $("#header").css("top", "0");
            }

            ultimaPosicaoScroll = posicaoAtualScroll;
        });
    });


    function SidebarCollapse() {
        $(".menu-collapsed").toggleClass("d-none");
        $(".sidebar-submenu").toggleClass("d-none");
        $(".submenu-icon").toggleClass("d-none");
        $("#sidebar-container").toggleClass("sidebar-expanded sidebar-collapsed");

        // Treating d-flex/d-none on separators with title
        var SeparatorTitle = $(".sidebar-separator-title");
        if (SeparatorTitle.hasClass("d-flex")) {
            SeparatorTitle.removeClass("d-flex");
        } else {
            SeparatorTitle.addClass("d-flex");
        }

        // Collapse/Expand icon
        $("#collapse-icon").toggleClass("fa-angle-double-left fa-angle-double-right");
    }

});