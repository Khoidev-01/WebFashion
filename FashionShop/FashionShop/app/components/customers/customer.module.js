/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function () {
    angular.module('FashionShopCustomer', ['FashionShopCommon']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('customer_list', {
                url: '/customer_list',
                parent: 'base',
                templateUrl: '/app/components/customers/customerListView.html',
                controller: 'customerListController'
            })
    }
})();