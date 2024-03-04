/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function () {
    angular.module('FashionShopProduct', ['FashionShopCommon']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('product_list', {
                url: '/product_list',
                parent: 'base',
                templateUrl: '/app/components/products/productListView.html',
                controller: 'productListController'
            })
            .state('product_add', {
                url: '/product_add',
                parent: 'base',
                templateUrl: '/app/components/products/productAddView.html',
                controller: 'productAddController'
            })
            .state('product_edit', {
                url: '/product_edit/:id',
                parent: 'base',
                templateUrl: '/app/components/products/productEditView.html',
                controller: 'productEditController'
            })
            .state('product_detail', {
                url: '/product_detail/:id',
                parent: 'base',
                templateUrl: '/app/components/products/productDetailView.html',
                controller: 'productDetailController'
            })
    }
})();