/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('roleListController', roleListController);

    roleListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox']

    function roleListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.listRoles = [];

        $scope.getListRoles = getListRoles;
        function getListRoles() {
            apiService.get('/api/Roles/get-list-roles', null, (result) => {
                $scope.listRoles = result.data;
            }, () => {
                alert('Get list roles failed');
            });
        }
        $scope.getListRoles();

        $scope.deleteRole = (id) => {
            $ngBootbox.confirm('Bạn có muốn xóa vai trò có id = ' + id).then(() => {
                apiService.delete('/api/Roles/delete-role/' + id, null, () => {
                    notificationService.displaySuccess('Xóa vai trò thành công');
                    $scope.getListRoles();
                }, () => {
                    notificationService.displayError('Xóa vai trò thất bại');
                })
            });
        };

    };
}) (angular.module('FashionShopRole'));
