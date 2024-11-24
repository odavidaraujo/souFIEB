var app = angular.module('myApp', []);

app.controller('namesCtrl', function ($scope, $http) {
    let scanner = new Instascan.Scanner({ video: document.getElementById('preview') });
    var selectedQrCode = {};
    scanner.addListener('scan', function (content) {
        let e = content.split("/");

        $http.get('event/' + e[0])
            .then(function (response) {
                selectedQrCode = response.data;

                executeQrCode();
                executeQuery();

            })
            .catch(function (error) {
                console.log('Erro ao buscar detalhes do usuário:', error);
            });
    });

    Instascan.Camera.getCameras().then(function (cameras) {
        if (cameras.length > 0) {
            scanner.start(cameras[0]); // Use a câmera disponível
        } else {
            alert('Nenhuma câmera encontrada');
        }
    }).catch(function (e) {
        console.error(e);
    });

    function executeQrCode() {
        var dataAtual = new Date();
        var horaAtual = dataAtual.getHours();

        var periodoDoDia = null;
        if (horaAtual >= 5 && horaAtual < 13) {
            periodoDoDia = 'manhã';
        } else if (horaAtual >= 13 && horaAtual < 19) {
            periodoDoDia = 'tarde';
        } else {
            periodoDoDia = 'noite';
        }

        document.getElementById('nome').value = selectedQrCode.nome;
        document.getElementById('rm').value = selectedQrCode.rm;
        document.getElementById('periodo').value = periodoDoDia;
        document.getElementById('reset').value = Date.now();
        $('.form-control').addClass('active');

        setTimeout(function () {
            document.getElementById('nome').value = "";
            document.getElementById('rm').value = "";
            document.getElementById('periodo').value = "";
        }, 2000);

        let src = "https://script.google.com/a/stis.ac.id/macros/s/AKfycbzaIKZ9PTAxlWISy00ypCNzjxG8TO8Z6CTBH1S3kPqsdlNmhdli/exec?nome=" + selectedQrCode.nome + "&rm=" + selectedQrCode.rm + "&periodo=" + periodoDoDia;
        let html = document.getElementById('tabel').innerHTML;

        html = `<tr>
            <td>${selectedQrCode.nome}</td>
            <td>${selectedQrCode.rm}</td>
            <td>${periodoDoDia}</td>
            <td>${dataAtual}</td>
        </tr>` + html;

        document.getElementById('tabel').innerHTML = html;
    };

    function executeQuery() {
        $http.post('event/add/' + selectedQrCode.codigoQR + '/' + selectedQrCode.rm)
            .then(function (response) {

            })
            .catch(function (error) {
                alert(error.data);
            });
    }
});