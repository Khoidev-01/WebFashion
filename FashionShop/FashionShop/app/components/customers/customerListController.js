/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('customerListController', customerListController);

    customerListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox']

    function customerListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.listCustomers = [];
        $scope.searchByName;

        $scope.getlistCustomers = getlistCustomers;
        function getlistCustomers() {
            var config = {
                params: {
                    filterRole: "9cd0f7a2-741d-405a-a8a3-a34b22da200c",
                    searchByName: $scope.searchByName,
                }
            };

            apiService.get('/api/Users/get-list-users', config, (result) => {
                $scope.listCustomers = result.data;
            }, (error) => {
                console.log('Lấy danh sách khách hàng thất bại');
            });
        };
        $scope.getlistCustomers();

        $scope.lockCustomer = lockCustomer;
        function lockCustomer(id) {
            $ngBootbox.confirm('Bạn có muốn khóa khách hàng có id = ' + id).then(() => {
                apiService.put('/api/Users/account-lock/' + id, null, () => {
                    notificationService.displaySuccess('Khóa thành công');
                    $scope.getlistCustomers();
                }, () => {
                    notificationService.displayError('Khóa thất bại');
                })
            });
        };

        $scope.unLockCustomer = unLockCustomer;
        function unLockCustomer(id) {
            $ngBootbox.confirm('Bạn có muốn mở khóa khách hàng có id = ' + id).then(() => {
                apiService.put('/api/Users/account-unlock/' + id, null, () => {
                    notificationService.displaySuccess('Mở khóa thành công');
                    $scope.getlistCustomers();
                }, () => {
                    notificationService.displayError('Mở khóa thất bại');
                })
            });
        };
    };


})(angular.module('FashionShopCustomer'));