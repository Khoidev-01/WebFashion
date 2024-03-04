/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('memberAddController', memberAddController);

    memberAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state']

    function memberAddController($scope, apiService, notificationService, $state) {
        $scope.member = {};

        $scope.addMember = () => {
            apiService.post('/api/Users/register-member', $scope.member, (result) => {
                notificationService.displaySuccess('Đăng ký nhân viên thành công');
                $state.go('member_list');
            }, (error) => {
                notificationService.displayError(error.data);
            });
        };
    };


})(angular.module('FashionShopMember'));