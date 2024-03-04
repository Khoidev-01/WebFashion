/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('adminEditController', adminEditController);

    adminEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams'];

    function adminEditController($scope, apiService, notificationService, $state, $stateParams) {
        $scope.admin = {};

        $scope.loadUserById = () => {
            apiService.get('api/Users/get-user-by-id/' + $stateParams.id, null, (result) => {
                $scope.admin = result.data;
            }, (error) => {
                notificationService.displayError('Không tải người dùng được');
            });
        }
        $scope.loadUserById();


        $scope.editAdmin = () => {
            apiService.put('/api/Users/update-user/' + $stateParams.id, $scope.admin, (result) => {
                notificationService.displaySuccess('Chỉnh sửa quản trị viên thành công');
                $state.go('admin_list');
            }, (error) => {
                notificationService.displayError(error.data);
            });
        }
    }


})(angular.module('FashionShopAdmin'));