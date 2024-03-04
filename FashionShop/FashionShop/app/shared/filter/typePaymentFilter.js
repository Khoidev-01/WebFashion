(function (app) {
    app.filter('typePaymentFilter', () => {
        return (typePayment) => {
            if (typePayment == 1) {
                return 'COD';
            }
            else if (typePayment == 2) {
                return 'Chuyển khoản';
            }
        }
    });

})(angular.module('FashionShopCommon'));