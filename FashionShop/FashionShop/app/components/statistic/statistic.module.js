/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function () {
    angular.module('FashionShopStatistic', ['FashionShopCommon']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('statistic_revenue', {
                url: '/statistic_revenue',
                parent: 'base',
                templateUrl: '/app/components/statistic/revenueStatisticView.html',
                controller: 'revenueStatisticController'
            })
    }
})();