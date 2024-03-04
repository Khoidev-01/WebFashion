/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function () {
    angular.module('FashionShopContact', ['FashionShopCommon']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('contact_list', {
                url: '/contact_list',
                parent: 'base',
                templateUrl: '/app/components/contacts/contactListView.html',
                controller: 'contactListController'
            })
    }
})();