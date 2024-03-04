/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function () {
    angular.module('FashionShopPost', ['FashionShopCommon']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('post_list', {
                url: '/post_list',
                parent: 'base',
                templateUrl: '/app/components/posts/postListView.html',
                controller: 'postListController'
            })
            .state('post_add', {
                url: '/post_add',
                parent: 'base',
                templateUrl: '/app/components/posts/postAddView.html',
                controller: 'postAddController'
            })
            .state('post_edit', {
                url: '/post_edit/:id',
                parent: 'base',
                templateUrl: '/app/components/posts/postEditView.html',
                controller: 'postEditController'
            })
    }
})();