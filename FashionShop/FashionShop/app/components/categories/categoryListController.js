/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('categoryListController', categoryListController);

    categoryListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox']

    function categoryListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.listCategories = [];

        $scope.getListCategories = getListCategories;
        function getListCategories() {
            apiService.get('/api/Categories/get-all-category', null, (result) => {
                $scope.listCategories = result.data;
            }, (error) => {
               console.log('Lấy danh sách danh mục sản phẩm thất bại');
            });
        };
        $scope.getListCategories();

        $scope.deleteCategory = function (id) {
            $ngBootbox.confirm('Bạn có muốn xóa danh mục sản phẩm có id = ' + id).then(() => {
                apiService.delete('/api/Categories/delete-category-by-id/' + id, null, () => {
                    notificationService.displaySuccess('Xóa danh mục sản phẩm thành công');
                    $scope.getListCategories();
                }, () => {
                    notificationService.displayError('Xóa danh mục sản phẩm thất bại');
                })
            });
        };
    };
})(angular.module('FashionShopCategory'));