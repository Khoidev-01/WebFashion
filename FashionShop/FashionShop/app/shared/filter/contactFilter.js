(function (app) {
    app.filter('contactFilter', () => {
        return (status) => {
            if (status == true) {
                return 'Đã giải quyết';
            }
            else {
                return 'Chưa giải quyết';
            }
        }
    });

})(angular.module('FashionShopCommon'));