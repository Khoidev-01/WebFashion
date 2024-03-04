/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('voucherAddController', voucherAddController);

    voucherAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state'];

    function voucherAddController($scope, apiService, notificationService, $state) {
        $scope.voucher = {
            discountAmount: true,
            discountPercentage: false,
            status: true,
            minimumValue: 0,
            createdBy: 'Quang Hào'
        };

        $scope.addVoucher = () => {
                console.log($scope.voucher);
            apiService.post('/api/Vouchers/create-voucher', $scope.voucher, (result) => {
                notificationService.displaySuccess('Tạo mã giảm giá thành công');
                $state.go('voucher_list');
            }, (error) => {
                notificationService.displayError(error.data);
            });
        };
    }

})(angular.module('FashionShopVoucher'));