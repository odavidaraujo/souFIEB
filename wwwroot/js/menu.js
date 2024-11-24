/*===== LINK ACTIVE =====*/
document.addEventListener("DOMContentLoaded", function (event) {

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

    const showNavbar = (toggleId, navId, subToggleCl, subId, bodyId, headerId, submenu_icon) => {
        const toggle = document.getElementById(toggleId),
            nav = document.getElementById(navId),
            subToggle = document.getElementsByClassName(subToggleCl),
            sub = document.getElementById(subId),
            bodypd = document.getElementById(bodyId),
            headerpd = document.getElementById(headerId)
            iconSubMenu = document.getElementsByClassName(submenu_icon)

        if (toggle && nav && bodypd && headerpd) {
            toggle.addEventListener('click', () => {
                nav.classList.toggle('show')
                toggle.classList.toggle('bx-x')
                bodypd.classList.toggle('body-pd')
                headerpd.classList.toggle('body-pd')

            })
        }
        if (subToggle.length > 0) {
            for (let i = 0; i < subToggle.length; i++) {
                subToggle[i].addEventListener('click', () => {
                    if (!sub.classList.contains("active")) {
                        if (!nav.classList.contains("show")) {
                            nav.classList.toggle('show')
                            toggle.classList.toggle('bx-x')
                            bodypd.classList.toggle('body-pd')
                            headerpd.classList.toggle('body-pd');
                        }
                    
                        sub.classList.add("active");

                        for (var i = 0; i < iconSubMenu.length; i++) {
                            iconSubMenu[i].classList.remove("bx-chevron-down");
                            iconSubMenu[i].classList.add("bx-chevron-up");
                        }
                    } else {
                        sub.classList.remove("active");
                        for (var i = 0; i < iconSubMenu.length; i++) {
                            iconSubMenu[i].classList.remove("bx-chevron-up");
                            iconSubMenu[i].classList.add("bx-chevron-down");
                        }
                    }
                });
            }
        }
    }

    showNavbar('header-toggle', 'nav-bar', 'submenu_toggle', 'submenu', 'body-pd', 'header', 'submenu_icon')

    /*===== LINK ACTIVE =====*/
    function setActiveLink() {
        const linkColor = document.querySelectorAll('.nav_link');
        const currentLocation = window.location.href;

        if (linkColor) {
            linkColor.forEach(link => {
                if (link.href === currentLocation) {
                    link.classList.add('active_menu');
                } else {
                    link.classList.remove('active_menu');
                }
            });
        }
    }

    window.onload = setActiveLink;

    var aluno = [];
    var infos = [];

    function fetchUser() {
        fetch("/Home/GetUsers")
            .then(response => {
                if (!response.ok) {
                    throw new Error("Erro na solicitação.");
                }
                return response.json();
            })
            .then(data => {
                aluno = data;
                attributes();
            })
            .catch(error => {
                console.error("Erro ao buscar dados:", error);
            });
    }

    function fetchInfo() {
        fetch("/informative/getinfo")
            .then(response => {
                if (!response.ok) {
                    throw new Error("Erro na solicitação.");
                }
                return response.json();
            })
            .then(data => {
                infos = data;
                numInfo();
                
            })
            .catch(error => {
                console.error("Erro ao buscar dados:", error);
            });
    }

    async function attributes() {
        var elemento = document.getElementById("helloworld");
        var elemento_img = document.getElementById("img_user");
        var palavras = aluno.nome.split(" ");
        var primeiraPalavra = palavras[0];
        var ultimaPalavra = palavras[palavras.length - 1];
        elemento.textContent = "Olá, " + primeiraPalavra + " " + ultimaPalavra;

        elemento_img.src = aluno.foto;
    }

    async function numInfo() {
        var elemento = document.getElementById("badge-infos");
        var lastInformativo = document.getElementById("lastInformativo");
        
        elemento.textContent = infos.totalCount;

        var dataDoUltimoInformativo = moment(infos.lastData);
        var dataFormatada = dataDoUltimoInformativo.fromNow();

        lastInformativo.textContent = dataFormatada;

    }
    fetchUser();
    fetchInfo();
});