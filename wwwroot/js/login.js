document.addEventListener("DOMContentLoaded", function (event) {
    /*let btn = document.querySelector('.fa-eye')

    btn.addEventListener('click', () => {
        let inputSenha = document.querySelector('#idSenha')

        if (inputSenha.getAttribute('type') == 'password') {
            inputSenha.setAttribute('type', 'text')
        } else {
            inputSenha.setAttribute('type', 'password')
        }
    })
    
    
    function entrar() {
      let usuario = document.querySelector('#usuario')
      let userLabel = document.querySelector('#userLabel')
    
      let senha = document.querySelector('#senha')
      let senhaLabel = document.querySelector('#senhaLabel')
    
      let msgError = document.querySelector('#msgError')
      let listaUser = []
    
      let userValid = {
        nome: null,
        user: null,
        senha: null
      }
      listaUser = JSON.parse(localStorage.getItem('listaUser'))
    
      listaUser?.forEach((item) => {
        if (usuario.value == item.userCad && senha == item.senhaCad) {
    
          userValid = {
            nome: item.nomeCad,
            user: item.userCad,
            senha: item.senhaCad
          }
        }
      })
    
    
      if (usuario.value == userValid.user && senha.value == userValid.senha) {
        window.localStorage.href = './index.html'
    
        let mathRandom = Math.random().toString(16).substr(2)
        let token = mathRandom + mathRandom
    
        localStorage.setItem('token', token)
        localStorage.setItem('userLogado', JSON.stringify(userValid))
      } else {
        userLabel.setAttribute('style', 'color:red')
        usuario.setAttribute('style', 'color:red')
        senhaLabel.setAttribute('style', 'color:red')
        senha.setAttribute('style', 'display:block')
        msgError.innerHTML = 'Usuário ou senha incorretos'
        usuario.focus()
    
      }
    }*/

    if ($("#failMessage").length) {
        setTimeout(function () {
            $("#failMessage").fadeOut();
        }, 10000); // Esconde após 5 segundos (pode ajustar esse valor)
    }
    $("#CloseButtonOps").click(function () {
        $("#failMessage").fadeOut();
    })

    // ParticlesJS Config.
    particlesJS("particles-js", {
        "particles": {
            "number": {
                "value": 60,
                "density": {
                    "enable": true,
                    "value_area": 800
                }
            },
            "color": {
                "value": "#ffffff"
            },
            "shape": {
                "type": "circle",
                "stroke": {
                    "width": 0,
                    "color": "#000000"
                },
                "polygon": {
                    "nb_sides": 5
                },
                "image": {
                    "src": "img/github.svg",
                    "width": 99,
                    "height": 99
                }
            },
            "opacity": {
                "value": 0.1,
                "random": false,
                "anim": {
                    "enable": false,
                    "speed": 1,
                    "opacity_min": 0.1,
                    "sync": false
                }
            },
            "size": {
                "value": 6,
                "random": false,
                "anim": {
                    "enable": false,
                    "speed": 40,
                    "size_min": 0.1,
                    "sync": false
                }
            },
            "line_linked": {
                "enable": true,
                "distance": 150,
                "color": "#ffffff",
                "opacity": 0.1,
                "width": 2
            },
            "move": {
                "enable": true,
                "speed": 1.5,
                "direction": "top",
                "random": false,
                "straight": false,
                "out_mode": "out",
                "bounce": false,
                "attract": {
                    "enable": false,
                    "rotateX": 600,
                    "rotateY": 1200
                }
            }
        },
        "interactivity": {
            "detect_on": "canvas",
            "events": {
                "onhover": {
                    "enable": false,
                    "mode": "repulse"
                },
                "onclick": {
                    "enable": false,
                    "mode": "push"
                },
                "resize": true
            },
            "modes": {
                "grab": {
                    "distance": 400,
                    "line_linked": {
                        "opacity": 1
                    }
                },
                "bubble": {
                    "distance": 400,
                    "size": 40,
                    "duration": 2,
                    "opacity": 8,
                    "speed": 3
                },
                "repulse": {
                    "distance": 200,
                    "duration": 0.4
                },
                "push": {
                    "particles_nb": 4
                },
                "remove": {
                    "particles_nb": 2
                }
            }
        },
        "retina_detect": true
    });
});

function checkStuff() {
    var email = $("#idUsuario").val();
    var password = $("#idSenha").val();
    var msg = $("#msg");
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (email == "") {
        msg.css("display", "block");
        msg.html("Please enter your email");
        $("#idUsuario").focus();
        return false;
    } else if (password == "") {
        msg.html("Please enter your password");
        $("#idSenha").focus();
        return false;
    } else if (!re.test(email)) {
        msg.html("Please enter a valid email");
        $("#idUsuario").focus();
        return false;
    } else {
        msg.css("display", "none");
        msg.html("");
    }
}

$("#loginForm").submit(function (event) {
    event.preventDefault();

    // Obtém os valores dos campos de login e senha
    var email = $("#idUsuario").val();
    var password = $("#idSenha").val();

    // Efetua a submissão do formulário diretamente, sem AJAX
    this.submit();
});