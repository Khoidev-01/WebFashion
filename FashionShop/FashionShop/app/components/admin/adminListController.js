/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('adminListController', adminListController);

    adminListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox']

    function adminListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.listAdmins = [];
        $scope.searchByName;

        $scope.getListAdmins = getListAdmins;
        function getListAdmins() {
            var config = {
                params: {
                    filterRole: "25d9875c-878d-414e-8e6f-b4c28815f739",
                    searchByName: $scope.searchByName,
                }
            };

            apiService.get('/api/Users/get-list-users', config, (result) => {
                $scope.listAdmins = result.data;
            }, (error) => {
                console.log('Lấy danh sách admin thất bại');
            });
        };
        $scope.getListAdmins();

        $scope.deleteAdmin = function (id) {
            $ngBootbox.confirm('Bạn có muốn xóa quản trị viên có id = ' + id).then(() => {
                apiService.delete('/api/Users/delete-user/' + id, null, () => {
                    notificationService.displaySuccess('Xóa quản trị viên thành công');
                    $scope.getListAdmins();
                }, () => {
                    notificationService.displayError('Xóa quản trị viên thất bại');
                })
            });
        };

        $scope.lockAdmin = lockAdmin;
        function lockAdmin(id) {
            $ngBootbox.confirm('Bạn có muốn khóa admin có id = ' + id).then(() => {
                apiService.put('/api/Users/account-lock/' + id, null, () => {
                    notificationService.displaySuccess('Khóa thành công');
                    $scope.getListAdmins();
                }, () => {
                    notificationService.displayError('Khóa thất bại');
                })
            });
        };

        $scope.unLockAdmin = unLockAdmin;
        function unLockAdmin(id) {
            $ngBootbox.confirm('Bạn có muốn mở khóa admin có id = ' + id).then(() => {
                apiService.put('/api/Users/account-unlock/' + id, null, () => {
                    notificationService.displaySuccess('Mở khóa thành công');
                    $scope.getListAdmins();
                }, () => {
                    notificationService.displayError('Mở khóa thất bại');
                })
            });
        };
    };

})(angular.module('FashionShopAdmin'));