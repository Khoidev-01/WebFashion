/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function () {
    angular.module('FashionShopMember', ['FashionShopCommon']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('member_list', {
                url: '/member_list',
                parent: 'base',
                templateUrl: '/app/components/members/memberListView.html',
                controller: 'memberListController'
            })
            .state('member_add', {
                url: '/member_add',
                parent: 'base',
                templateUrl: '/app/components/members/memberAddView.html',
                controller: 'memberAddController'
            })
            .state('member_edit', {
                url: '/member_edit/:id',
                parent: 'base',
                templateUrl: '/app/components/members/memberEditView.html',
                controller: 'memberEditController'
            })
    }
})();