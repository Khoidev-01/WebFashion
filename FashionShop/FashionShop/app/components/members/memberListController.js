/// <reference path="../../../assets/nhân viên/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('memberListController', memberListController);

    memberListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox']

    function memberListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.listMembers = [];
        $scope.searchByName;

        $scope.getlistMembers = getlistMembers;
        function getlistMembers() {
            var config = {
                params: {
                    filterRole: "3195156e-ef20-4c3d-9406-7bc7e87fd6f6",
                    searchByName: $scope.searchByName,
                }
            };

            apiService.get('/api/Users/get-list-users', config, (result) => {
                $scope.listMembers = result.data;
            }, (error) => {
                console.log('Lấy danh sách nhân viên thất bại');
            });
        };
        $scope.getlistMembers();

        $scope.deleteMember = function (id) {
            $ngBootbox.confirm('Bạn có muốn xóa nhân viên có id = ' + id).then(() => {
                apiService.delete('/api/Users/delete-user/' + id, null, () => {
                    notificationService.displaySuccess('Xóa nhân viên thành công');
                    $scope.getlistMembers();
                }, () => {
                    notificationService.displayError('Xóa nhân viên thất bại');
                })
            });
        };

        $scope.lockMember = lockMember;
        function lockMember(id) {
            $ngBootbox.confirm('Bạn có muốn khóa nhân viên có id = ' + id).then(() => {
                apiService.put('/api/Users/account-lock/' + id, null, () => {
                    notificationService.displaySuccess('Khóa thành công');
                    $scope.getlistMembers();
                }, () => {
                    notificationService.displayError('Khóa thất bại');
                })
            });
        };

        $scope.unlockMember = unlockMember;
        function unlockMember(id) {
            $ngBootbox.confirm('Bạn có muốn mở khóa nhân viên có id = ' + id).then(() => {
                apiService.put('/api/Users/account-unlock/' + id, null, () => {
                    notificationService.displaySuccess('Mở khóa thành công');
                    $scope.getlistMembers();
                }, () => {
                    notificationService.displayError('Mở khóa thất bại');
                })
            });
        };
    };


})(angular.module('FashionShopMember'));