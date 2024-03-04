(function (app) {
    app.filter('statusFilter', () => {
        return (status) => {
            if (status == true) {
                return 'Hiển thị';
            }
            else {
                return 'Ẩn';
            }
        }
    });

})(angular.module('FashionShopCommon'));