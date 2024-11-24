document.addEventListener("DOMContentLoaded", function (event) {
    if ($("#successMessage").length) {
        setTimeout(function () {
            $("#successMessage").fadeOut();
        }, 10000); // Esconde após 5 segundos (pode ajustar esse valor)
    }

    //document.getElementById("dataEdit").disabled = true;

    var app = angular.module('myApp', []);
    app.controller('namesCtrl', function ($scope, $http) {

        $scope.getStatusIconClass = {
            0: 'bx-x-circle red-icon',
            1: 'bx-check-circle green-icon'
        };

        $scope.setSelectedInfo = function (id) {

            $http.get('informative/' + id)
                .then(function (response) {
                    $scope.selectedInfo = response.data;
                })
                .catch(function (error) {
                    console.log('Erro ao buscar detalhes do usuário:', error);
                });


        };

        $scope.deleteInfo = function (id) {
            $http.delete('informative/del/' + id)
                .then(function (response) {
                    window.location.href = '/informative'; // Redireciona para a página de usuários após a exclusão
                })
                .catch(function (error) {
                    // Lide com erros, por exemplo, exiba uma mensagem de erro
                    alert("Não foi possível deletar o usuário!")
                });
        };

        $scope.setUser = function () {
            $http.get('/Home/GetUsers')
                .then(response => {
                    if (!response.ok) {
                        throw new Error("Erro na solicitação.");
                    }
                    return response.json();
                })
                .then(data => {
                    $scope.selectedUser = data;
                })
                .catch(error => {
                    console.error("Erro ao buscar dados:", error);
                });
        }


    });


}); 