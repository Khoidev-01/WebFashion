/// <reference path="../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.filter('formatCurrencyVND', () => {
        return (price) => {
            return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',') + '₫'; 
        }
    });

})(angular.module('FashionShopCommon'));
