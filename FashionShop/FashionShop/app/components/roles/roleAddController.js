/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('roleAddController', roleAddController);

    roleAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state'];

    function roleAddController($scope, apiService, notificationService, $state) {
        $scope.role = {};

        $scope.addRole = () => {
            apiService.post('/api/Roles/create-role', $scope.role, (result) => {
                notificationService.displaySuccess('Tạo vai trò thành công');
                $state.go('role_list');
            }, (error) => {
                notificationService.displayError('Tạo vai trò thất bại');
            });
        };
    }

})(angular.module('FashionShopRole'));