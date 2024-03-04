(function (app) {
    app.controller('loginController', ['$scope', 'loginService', '$injector', 'notificationService',
        function ($scope, loginService, $injector, notificationService) {

            $scope.loginData = {
                email: "",
                password: ""
            };

            $scope.loginSubmit = function () {
                loginService.login($scope.loginData.email, $scope.loginData.password).then(function (response) {
                    if (response != null && (response.status == 400 || response.status == 500)) {
                        notificationService.displayError("Đăng nhập không thành công.");
                    }
                    else {
                        var stateService = $injector.get('$state');
                        stateService.go('statistic_revenue');
                    }
                });
            }
        }]);

})(angular.module('FashionShop'));