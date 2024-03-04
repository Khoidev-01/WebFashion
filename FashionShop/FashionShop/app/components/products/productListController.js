/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('productListController', productListController);

    productListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox']

    function productListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.listProducts = [];

        $scope.page = 0;
        $scope.pageSize = 6;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.num = 0;

        $scope.searchByCategory;
        $scope.searchByName;

        $scope.listCategories = [];
        $scope.getListCategory = () => {
            apiService.get('/api/Categories/get-all-category', null, (result) => {
                $scope.listCategories = result.data;
            }, () => {
                console.log('Tải danh mục sản phẩm thất bại');
            });
        };
        $scope.getListCategory();

        $scope.getListProducts = getListProducts;
        function getListProducts(page) {
            page = page || 0;
            var config = {
                params: {
                    page: page,
                    pageSize: $scope.pageSize,
                    searchByCategory: $scope.searchByCategory,
                    searchByName: $scope.searchByName
                }
            };

            apiService.get('/api/Products/get-list-products', config, (result) => {
                $scope.listProducts = result.data.list;

                $scope.page = result.data.page;
                $scope.num = result.data.count;
                $scope.pagesCount = result.data.pagesCount;
                $scope.totalCount = result.data.totalCount;

                if ($scope.num == 0) {
                    $scope.showTo = 0;
                }
                else {
                    $scope.showTo = ($scope.page * $scope.pageSize + 1);
                }
                $scope.showFrom = ($scope.page * $scope.pageSize) + $scope.num;

                if ($scope.showFrom % $scope.pageSize == 1) {
                    $scope.showEnd = true;
                }
                else {
                    $scope.showEnd = false;
                }

            }, (error) => {
                console.log(error);
            });
        };
        $scope.getListProducts();

        $scope.deleteProduct = function(id) {
            $ngBootbox.confirm('Bạn có muốn xóa sản phẩm có id = ' + id).then(() => {
                apiService.delete('/api/Products/delete-product/' + id, null, () => {
                    notificationService.displaySuccess('Xóa sản phẩm thành công');
                    $scope.getListProducts();
                }, () => {
                    notificationService.displayError('Xóa sản phẩm thất bại');
                })
            });
        };
        
    };
})(angular.module('FashionShopProduct'));