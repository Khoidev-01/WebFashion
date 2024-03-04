/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('voucherEditController', voucherEditController);

    voucherEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams'];

    function voucherEditController($scope, apiService, notificationService, $state, $stateParams) {
        $scope.voucher = {};

        $scope.loadVoucherById = () => {
            apiService.get('api/Vouchers/get-voucher-by-id/' + $stateParams.id, null, (result) => {
                $scope.voucher = result.data;
                $scope.voucher.startDate = new Date(result.data.startDate);
                $scope.voucher.endDate = new Date(result.data.endDate);
            }, (error) => {
                notificationService.displayError('Không tải mã giảm giá được');
            });
        }
        $scope.loadVoucherById();

        $scope.editVoucher = () => {
            $scope.voucher.updatedBy = 'Quang Hào';
            apiService.put('/api/Vouchers/update-voucher/' + $stateParams.id, $scope.voucher, (result) => {
                notificationService.displaySuccess('Chỉnh sửa mã giảm giá thành công');
                $state.go('voucher_list');
            }, (error) => {
                notificationService.displayError(error.data);
            });
        }
    };

})(angular.module('FashionShopRole'));