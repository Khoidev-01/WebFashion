/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('voucherListController', voucherListController);

    voucherListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox']

    function voucherListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.listVouchers = [];

        $scope.getListVouchers = getListVouchers;
        function getListVouchers() {
            apiService.get('/api/Vouchers/get-list-vouchers', null, (result) => {
                $scope.listVouchers = result.data;
            }, () => {
                
            });
        }
        $scope.getListVouchers();

        $scope.deleteVoucher = (id) => {
            $ngBootbox.confirm('Bạn có muốn xóa mã giảm giá có id = ' + id).then(() => {
                apiService.delete('/api/Vouchers/delete-voucher/' + id, null, () => {
                    notificationService.displaySuccess('Xóa mã giảm giá thành công');
                    $scope.getListVouchers();
                }, () => {
                    notificationService.displayError('Xóa mã giảm giá thất bại');
                })
            });
        };

    };
}) (angular.module('FashionShopVoucher'));
