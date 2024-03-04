/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function () {
    angular.module('FashionShopRole', ['FashionShopCommon']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('role_list', {
                url: '/role_list',
                parent: 'base',
                templateUrl: '/app/components/roles/roleListView.html',
                controller: 'roleListController'
            })
            .state('role_add', {
                url: '/role_add',
                parent: 'base',
                templateUrl: '/app/components/roles/roleAddView.html',
                controller: 'roleAddController'
            })
            .state('role_edit', {
                url: '/role_edit/:id',
                parent: 'base',
                templateUrl: '/app/components/roles/roleEditView.html',
                controller: 'roleEditController'
            })
    }
})();