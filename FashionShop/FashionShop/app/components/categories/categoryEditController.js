/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('categoryEditController', categoryEditController);
    
    categoryEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams'];

    function categoryEditController($scope, apiService, notificationService, $state, $stateParams) {
        $scope.category = {};

        $scope.loadCategoryById = () => {
            apiService.get('api/Categories/get-category-by-id/' + $stateParams.id, null, (result) => {
                $scope.category = result.data;
            }, (error) => {
                notificationService.displayError('Không tải danh mục sản phẩm được');
            });
        }
        $scope.loadCategoryById();

        $scope.editCategory = () => {
            apiService.put('/api/Categories/update-category-by-id/' + $stateParams.id, $scope.category, (result) => {
                notificationService.displaySuccess('Chỉnh sửa danh muc sản phẩm thành công');
                $state.go('category_list');
            }, (error) => {
                notificationService.displayError('Chỉnh sửa danh muc sản phẩm thất bại');
            });
        }
    }


})(angular.module('FashionShopCategory'));