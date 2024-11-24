document.addEventListener("DOMContentLoaded", function (event) {
    if ($("#successMessage").length) {
        setTimeout(function () {
            $("#successMessage").fadeOut();
        }, 10000); // Esconde após 5 segundos (pode ajustar esse valor)
    }

    var app = angular.module('myApp', []);
    app.controller('namesCtrl', function ($scope, $http) {

        $scope.getStatusIconClass = {
            "0": 'bx-x-circle red-icon',
            "1": 'bx-check-circle green-icon'
        };

        $scope.setSelectedFood = function (id) {

            $http.get('food/' + id)
                .then(function (response) {
                    $scope.selectedFood = response.data;
                    console.log($scope.selectedFood);
                })
                .catch(function (error) {
                    console.log('Erro ao buscar detalhes do usuário:', error);
                });


        };


    });


}); 