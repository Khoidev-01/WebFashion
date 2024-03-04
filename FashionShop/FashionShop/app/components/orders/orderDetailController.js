/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('orderDetailController', orderDetailController);

    orderDetailController.$inject = ['$scope', 'apiService', 'notificationService', '$stateParams']

    function orderDetailController($scope, apiService, notificationService, $stateParams) {
        $scope.order = [];

        $scope.loadOrderById = function () {
            apiService.get('/api/Orders/get-order-by-id/' + $stateParams.id, null, (result) => {
                console.log(result.data);
                $scope.order = result.data;

                $scope.totalMoney = 0;
                $scope.order.orderDetails.forEach((item, index) => {
                    $scope.quantity = item.quantity;
                    $scope.price = item.price;
                    $scope.totalMoney += $scope.price * $scope.quantity;
                });

                $scope.voucherValue = 0;
                if ($scope.order.voucher != null) {
                    if ($scope.order.voucher.discountAmount == true) {
                        $scope.voucherValue = $scope.order.voucher.discountValue;
                    }
                    else {
                        $scope.voucherValue = totalMoney * $scope.order.voucher.discountValue / 100;
                    }
                }

                $scope.totalPayment = $scope.totalMoney + $scope.order.deliveryFee - $scope.voucherValue;

                console.log($scope.order.orderDetails);

            }, (error) => {
                notificationService.displayError('Không tải đơn hàng được');
            });
        };
        $scope.loadOrderById();
    };


})(angular.module('FashionShopOrder'));