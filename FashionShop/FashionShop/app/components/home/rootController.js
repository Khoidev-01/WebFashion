(function (app) {
    app.controller('rootController', rootController);

    rootController.$inject = ['$state', 'authData', 'loginService', '$scope'];

    function rootController($state, authData, loginService, $scope) {
        $scope.logOut = function () {
            loginService.logOut();

            $state.go('login');
        }
        $scope.authentication = authData.authenticationData;
    }

})(angular.module('FashionShop'));