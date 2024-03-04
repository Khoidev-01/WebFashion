/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function () {
    angular.module('FashionShopAdmin', ['FashionShopCommon']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('admin_list', {
                url: '/admin_list',
                parent: 'base',
                templateUrl: '/app/components/admin/adminListView.html',
                controller: 'adminListController'
            })
            .state('admin_add', {
                url: '/admin_add',
                parent: 'base',
                templateUrl: '/app/components/admin/adminAddView.html',
                controller: 'adminAddController'
            })
            .state('admin_edit', {
                url: '/admin_edit/:id',
                parent: 'base',
                templateUrl: '/app/components/admin/adminEditView.html',
                controller: 'adminEditController'
            })
    }
})();