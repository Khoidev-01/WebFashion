/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('orderListController', orderListController);

    orderListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox']

    function orderListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.listOrders = [];

        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.num = 0;

        $scope.typePayment;
        $scope.searchByID;
        $scope.searchByName;
        $scope.searchBySDT;

        $scope.getListOrders = getListOrders;
        function getListOrders(page) {
            page = page || 0;
            var config = {
                params: {
                    page: page,
                    pageSize: $scope.pageSize,
                    typePayment: $scope.typePayment,
                    searchByID: $scope.searchByID,
                    searchByName: $scope.searchByName,
                    searchBySDT: $scope.searchBySDT
                }
            };

            apiService.get('/api/Orders/get-list-orders', config, (result) => {
                $scope.listOrders = result.data.list;

                $scope.page = result.data.page;
                $scope.num = result.data.count;
                $scope.pagesCount = result.data.pagesCount;
                $scope.totalCount = result.data.totalCount;

                if ($scope.num == 0) {
                    $scope.showTo = 0;
                }
                else {
                    $scope.showTo = ($scope.page * $scope.pageSize + 1);
                }
                $scope.showFrom = ($scope.page * $scope.pageSize) + $scope.num;

                if ($scope.showFrom % $scope.pageSize == 1) {
                    $scope.showEnd = true;
                }
                else {
                    $scope.showEnd = false;
                }

                $scope.listOrders.forEach((order, index) => {
                    apiService.get('/api/Orders/get-totalPayment-by-id/' + order.id, null, (result) => {
                        order.totalPayment = result.data;
                    }, () => {
                        alert('Get list order failed');
                    });
                });

            }, () => {
                alert('Get list order failed');
            });
        }
        $scope.getListOrders();


        //$scope.deleteRole = (id) => {
        //    $ngBootbox.confirm('Bạn có muốn xóa vai trò có id = ' + id).then(() => {
        //        apiService.delete('/api/Roles/delete-role/' + id, null, () => {
        //            notificationService.displaySuccess('Xóa vai trò thành công');
        //            $scope.getListOrders();
        //        }, () => {
        //            notificationService.displayError('Xóa vai trò thất bại');
        //        })
        //    });
        //};

        $scope.exportToExcel = function (id) {
            window.location.href = "/api/Orders/export-excel/" + id;
        }
    };
       



})(angular.module('FashionShopOrder'));