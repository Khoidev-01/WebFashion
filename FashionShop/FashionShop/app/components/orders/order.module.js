/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function () {
    angular.module('FashionShopOrder', ['FashionShopCommon']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('order_list', {
                url: '/order_list',
                parent: 'base',
                templateUrl: '/app/components/orders/orderListView.html',
                controller: 'orderListController'
            })
            .state('order_detail', {
                url: '/order_detail/:id',
                parent: 'base',
                templateUrl: '/app/components/orders/orderDetailView.html',
                controller: 'orderDetailController'
            })
    }
})();