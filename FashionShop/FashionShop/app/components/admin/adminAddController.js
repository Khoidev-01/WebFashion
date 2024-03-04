/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('adminAddController', adminAddController);

    adminAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state']

    function adminAddController($scope, apiService, notificationService, $state) {
        $scope.admin = {};

        $scope.addAdmin = () => {
            apiService.post('/api/Users/register-admin', $scope.admin, (result) => {
                notificationService.displaySuccess('Đăng ký quản trị viên thành công');
                $state.go('admin_list');
            }, (error) => {
                notificationService.displayError(error.data);
            });
        };
    };


})(angular.module('FashionShopAdmin'));