(function (app) {
    app.filter('statusOrder', () => {
        return (status) => {
            if (status == 0) {
                return 'Đã hủy';
            }
            else if (status == 1) {
                return 'Chờ xác nhận';
            }
            else if (status == 2) {
                return 'Đang giao hàng';
            }
            else if (status == 3) {
                return 'Giao hàng thành công';
            }
        }
    });

})(angular.module('FashionShopCommon'));