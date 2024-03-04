/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('voucherDetailController', voucherDetailController);

    voucherDetailController.$inject = ['$scope', 'apiService', 'notificationService', '$stateParams']

    function voucherDetailController($scope, apiService, notificationService, $stateParams) {
        $scope.voucher = [];

        $scope.loadVoucherById = () => {
            apiService.get('api/Vouchers/get-voucher-by-id/' + $stateParams.id, null, (result) => {
                $scope.voucher = result.data;
            }, (error) => {
                notificationService.displayError('Không tải mã giảm giá được');
            });
        }
        $scope.loadVoucherById();
    };


})(angular.module('FashionShopProduct'));