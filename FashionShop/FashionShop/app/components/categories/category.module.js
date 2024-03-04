/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function () {
    angular.module('FashionShopCategory', ['FashionShopCommon']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('category_list', {
                url: '/category_list',
                parent: 'base',
                templateUrl: '/app/components/categories/categoryListView.html',
                controller: 'categoryListController'
            })
            .state('category_add', {
                url: '/category_add',
                parent: 'base',
                templateUrl: '/app/components/categories/categoryAddView.html',
                controller: 'categoryAddController'
            })
            .state('category_edit', {
                url: '/category_edit/:id',
                parent: 'base',
                templateUrl: '/app/components/categories/categoryEditView.html',
                controller: 'categoryEditController'
            })
    }
})();