/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter']

    function revenueStatisticController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.tableData = [];

        $scope.labels = [];
        $scope.series = ['Doanh thu', 'Lợi nhuận'];

        $scope.chartdata = [];

        $scope.getStatistic = getStatistic;
        function getStatistic() {
            var config = {
                params: {
                    fromDate: '01/01/2016',
                    toDate: '01/01/2025',
                }
            };

            apiService.get('/api/Statistics/get-revenueStatistics', config, (result) => {
                $scope.tableData = result.data;

                var labels = [];
                var chartData = [];
                var revenues = [];
                var benefits = [];
                $.each(result.data, function (i, item) {
                    labels.push($filter('date')(item.date, 'dd/MM/yyyy'));
                    revenues.push(item.revenues);
                    benefits.push(item.benefit);
                });
                chartData.push(revenues);
                chartData.push(benefits);

                $scope.chartdata = chartData;
                $scope.labels = labels;

            }, () => {
                notificationService.displayError('Lấy dữ liệu thất bại');
            });         
        }
        $scope.getStatistic();

        $scope.getCountOrder = getCountOrder;
        function getCountOrder() {
            apiService.get('/api/Orders/get-count-order', null, (result) => {
                $scope.countOrder = result.data;
            }, () => {
                notificationService.displayError('Lấy số lượng đơn hàng thất bại');
            });
        }
        $scope.getCountOrder();

        $scope.getCountProduct = getCountProduct;
        function getCountProduct() {
            apiService.get('/api/Products/get-count-product', null, (result) => {
                $scope.countProduct = result.data;
            }, () => {
                notificationService.displayError('Lấy số lượng sản phẩm thất bại');
            });
        }
        $scope.getCountProduct();

        $scope.getCountCustomer = getCountCustomer;
        function getCountCustomer() {
            apiService.get('/api/Users/get-count-customer', null, (result) => {
                $scope.countCustomer = result.data;
            }, () => {
                notificationService.displayError('Lấy số lượng khách hàng thất bại');
            });
        }
        $scope.getCountCustomer();

        $scope.getCountVoucher = getCountVoucher;
        function getCountVoucher() {
            apiService.get('/api/Vouchers/get-count-voucher', null, (result) => {
                $scope.countVoucher = result.data;
            }, () => {
                notificationService.displayError('Lấy số lượng giảm giá thất bại');
            });
        }
        $scope.getCountVoucher();

    };
})(angular.module('FashionShopStatistic'));
