document.addEventListener("DOMContentLoaded", function (event) {
    if ($("#successMessage").length) {
        setTimeout(function () {
            $("#successMessage").fadeOut();
        }, 10000);
    }

    $("#fullname").on("input", function () {
        var nome = $(this).val().replace(/[^a-zA-Z\sÀ-ÿ]/g, '');

        // Formate o nome
        var nomeFormatado = nome.split(' ').map(function (palavra) {
            return palavra.charAt(0).toUpperCase() + palavra.slice(1).toLowerCase();
        }).join(' ');

        $(this).val(nomeFormatado);
    });

    $("#fullname").on("blur", function () {
        if ($(this).val() != '') {
            var nome = $(this).val().replace(/[^a-zA-Z\sÀ-ÿ]/g, '');

            // Divida o nome em palavras
            var palavras = nome.split(' ');

            // Verifique se o primeiro nome tem pelo menos 3 letras
            if (palavras.length > 0 && palavras[0].length < 3) {
                return alert("O primeiro nome deve conter pelo menos 3 letras.");
            }
        }
    });

    $("#datebirth").on("input", function () {
        if ($(this).val() != '') {
            var dataNascimento = $(this).val();

            var ano = new Date(dataNascimento).getFullYear();
            var mes = (new Date(dataNascimento).getMonth() + 1).toString().padStart(2, '0');
            var dia = (new Date(dataNascimento).getDate()).toString().padStart(2, '0');
            if (ano.length > 4) return $(this).val(`aaaa-${mes}-${dia}`);

            // Verifique se a data de nascimento é maior de 13 anos
            var hoje = new Date();
            var trezeAnosAtras = new Date();
            trezeAnosAtras.setFullYear(hoje.getFullYear() - 13);

            if (new Date(dataNascimento) > trezeAnosAtras) {
                alert("Você deve ter pelo menos 13 anos de idade.");
                return;
            }

            // Verifique se 29 de fevereiro é válido (caso não seja ano bissexto)
            if (!isBissexto(ano)) {
                if (mes === '02' && dia === '29') {
                    alert("O dia 29 de fevereiro não é válido em anos não bissextos.");
                    return;
                }
            }
        }
    });

    function isBissexto(ano) {
        return (ano % 4 === 0 && ano % 100 !== 0) || ano % 400 === 0;
    }

    $("#email").on("blur", function () {
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        var valor = $(this).val();
        if (valor != '') {
            if (!regex.test(valor)) {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'center',
                    showConfirmButton: false,
                    timer: 1500,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })

                Toast.fire({
                    icon: 'error',
                    title: 'Email Inválido!'
                })
                setTimeout(function () {
                    $(this).focus();
                }, 1750);
            }
        }
    });

    $("#rg").on("blur", function () {
        var valor = $(this).val().replace(/\D/g, '');
        if (valor.length > 0 && valor.length < 9) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'center',
                showConfirmButton: false,
                timer: 1500,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })

            Toast.fire({
                icon: 'error',
                title: 'RG Inválido!'
            })
            setTimeout(function () {
                $(this).focus();
            }, 1750);

        }
    });

    $("#cpf").on("blur", function () {
        s = $(this).val();
        filteredValues = ".-/";
        var i;
        var returnString = "";
        for (i = 0; i < s.length; i++) {
            var c = s.charAt(i);
            if (filteredValues.indexOf(c) == -1)
                returnString += c;
        }
        if (returnString == '11111111111' || returnString == '22222222222' ||
            returnString == '33333333333' || returnString == '44444444444' ||
            returnString == '55555555555' || returnString == '66666666666' ||
            returnString == '77777777777' || returnString == '88888888888' ||
            returnString == '99999999999' || returnString == '00000000000' ||
            returnString == '00000000191') {
            const Toast = Swal.mixin({
                toast: true,
                position: 'center',
                showConfirmButton: false,
                timer: 1500,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'error',
                title: 'CPF Inválido!'
            })
            setTimeout(function () {
                $(this).focus();
            }, 1750);
        }
        if (returnString.length != 11) {
            sim = false
        }
        else {
            sim = true
        }
        if (sim) {
            for (i = 0; ((i <= (returnString.length - 1)) && sim); i++) {
                val = returnString.charAt(i)
                if ((val != "9") && (val != "0") && (val != "1") && (val != "2") && (val != "3") && (val != "4")
                    && (val != "5") && (val != "6") && (val != "7") && (val != "8")) {
                    sim = false
                }
            }
            if (sim) {
                soma = 0
                for (i = 0; i <= 8; i++) {
                    val = eval(returnString.charAt(i))
                    soma = soma + (val * (i + 1))
                }
                resto = soma % 11
                if (resto > 9)
                    dig = resto - 10
                else
                    dig = resto
                if (dig != eval(returnString.charAt(9))) {
                    sim = false
                }
                else {
                    soma = 0
                    for (i = 0; i <= 7; i++) {
                        val = eval(returnString.charAt(i + 1))
                        soma = soma + (val * (i + 1))
                    }
                    soma = soma + (dig * 9)
                    resto = soma % 11
                    if (resto > 9)
                        dig = resto - 10
                    else
                        dig = resto
                    if (dig != eval(returnString.charAt(10))) {
                        sim = false
                    }
                    else {
                        sim = true;
                    }
                }
            }
        }
        if (sim != true) {
            if (s != '') {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'center',
                    showConfirmButton: false,
                    timer: 1500,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })

                Toast.fire({
                    icon: 'error',
                    title: 'CPF Inválido!'
                })
                setTimeout(function () {
                    $(this).focus();
                }, 1750);
                return false;
            }
            else {
                return false;
            }
        }
    });

    $("#password").on("blur", function () {
        var valor = $(this).val();
        if (valor.length > 0 && valor.length < 6) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'center',
                showConfirmButton: false,
                timer: 2200,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })

            Toast.fire({
                icon: 'error',
                title: 'Insira uma senha maior que 5 caracteres!'
            })
            setTimeout(function () {
                $(this).focus();
            }, 2200);

        }
    });

    $("#phone").on("blur", function () {
        var valor = $(this).val().replace(/\D/g, '');
        if (valor.length > 0 && valor.length < 11) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'center',
                showConfirmButton: false,
                timer: 1500,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })

            Toast.fire({
                icon: 'error',
                title: 'Celular Inválido!'
            })
            setTimeout(function () {
                $(this).focus();
            }, 1750);

        }
    });

    $('#datebirth').inputmask('99/99/9999');
    $("#rg").inputmask('99.999.999-9');
    $("#cpf").inputmask('999.999.999-99');
    $("#phone").inputmask('(99) 99999-9999');
    $("#postalcode").inputmask('99999-999');

    $("#postalcode").on("input", function () {
        var cepReplaced = $(this).val().replace(/\D/g, '');
        if (cepReplaced.length === 8) {
            consultarCep(cepReplaced);
        }
    });

    function consultarCep(cep) {
        $.ajax({
            url: `https://viacep.com.br/ws/${cep}/json/`,
            type: "GET",
            dataType: "json",
            success: function (data) {
                if (data.erro) {

                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'center',
                        showConfirmButton: false,
                        timer: 2000,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })

                    Toast.fire({
                        icon: 'error',
                        title: 'CEP não encontrado!'
                    })
                    $(this).val("");
                    setTimeout(function () {
                        $(this).focus();
                    }, 2000);

                } else {
                    $("#streetname").val(data.logradouro);
                    $("#district").val(data.bairro);
                    $("#city").val(data.localidade);
                    $("#state").val(data.uf);
                    $("#additional").val(data.complemento);
                }
            },
            error: function () {
                alert("Erro ao buscar informações do CEP. Tente novamente mais tarde.");
            }
        });
    }

    var app = angular.module('myApp', []);
    app.controller('namesCtrl', function ($scope, $http) {
        $scope.selectedUser = null;
        $scope.setSelectedUser = function (user) {

            $scope.selectedUser = null; 
            $http.get('users/' + user)
                .then(function (response) {
                    $scope.selectedUser = response.data;
                    console.log($scope.selectedUser)
                })
                .catch(function (error) {
                    console.log('Erro ao buscar detalhes do usuário:', error);
                });
        };
    });



    
}); 