/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('memberEditController', memberEditController);

    memberEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams'];

    function memberEditController($scope, apiService, notificationService, $state, $stateParams) {
        $scope.member = {};

        $scope.loadUserById = () => {
            apiService.get('api/Users/get-user-by-id/' + $stateParams.id, null, (result) => {
                $scope.member = result.data;
            }, (error) => {
                notificationService.displayError('Không tải người dùng được');
            });
        }
        $scope.loadUserById();


        $scope.editMember = () => {
            apiService.put('/api/Users/update-user/' + $stateParams.id, $scope.member, (result) => {
                notificationService.displaySuccess('Chỉnh sửa nhân viên thành công');
                $state.go('member_list');
            }, (error) => {
                notificationService.displayError(error.data);
            });
        }
    }


})(angular.module('FashionShopMember'));