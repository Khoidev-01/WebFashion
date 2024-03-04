/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function () {
    angular.module('FashionShopVoucher', ['FashionShopCommon']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('voucher_list', {
                url: '/voucher_list',
                parent: 'base',
                templateUrl: '/app/components/vouchers/voucherListView.html',
                controller: 'voucherListController'
            })
            .state('voucher_add', {
                url: '/voucher_add',
                parent: 'base',
                templateUrl: '/app/components/vouchers/voucherAddView.html',
                controller: 'voucherAddController'
            })
            .state('voucher_edit', {
                url: '/voucher_edit/:id',
                parent: 'base',
                templateUrl: '/app/components/vouchers/voucherEditView.html',
                controller: 'voucherEditController'
            })
            .state('voucher_detail', {
                url: '/voucher_detail/:id',
                parent: 'base',
                templateUrl: '/app/components/vouchers/voucherDetailView.html',
                controller: 'voucherDetailController'
            })
    }
})();