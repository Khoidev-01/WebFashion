(function (app) {
    app.filter('lockFilter', () => {
        return (status) => {
            if (status == true) {
                return 'Có';
            }
            else {
                return 'Không';
            }
        }
    });

})(angular.module('FashionShopCommon'));
