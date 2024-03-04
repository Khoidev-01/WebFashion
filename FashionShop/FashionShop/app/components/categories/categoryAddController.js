/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('categoryAddController', categoryAddController);

    categoryAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state']

    function categoryAddController($scope, apiService, notificationService, $state) {
        $scope.category = {};

        $scope.addCategory = () => {
            apiService.post('/api/Categories/add-category', $scope.category, (result) => {
                notificationService.displaySuccess('Tạo danh mục sản phẩm thành công');
                $state.go('category_list');
            }, (error) => {
                notificationService.displayError('Tạo danh mục sản phẩm thất bại');
            });
        };
    };

})(angular.module('FashionShopCategory'));