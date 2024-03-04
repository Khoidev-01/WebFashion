/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('productEditController', productEditController);

    productEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams']

    function productEditController($scope, apiService, notificationService, $state, $stateParams) {
        $scope.product = {};

        $scope.chooseImage = function () {
            window.fileSelected = function (data) {
                $scope.$apply(() => {
                    $scope.product.image = data.path;
                })
            };
            window.open('/file-manager-elfinder', "Select", "menubar:0;");
        };

        $scope.listImages = [];
        $scope.chooseListImage = function () {
            window.fileSelected = function (data) {
                $scope.$apply(() => {
                    if ($scope.listImages.includes(data.path) == false) {
                        $scope.listImages.push(data.path);
                    }
                    else {
                        notificationService.displayWarning('Hình ảnh này đã tồn tại');
                    }
                })
            };
            window.open('/file-manager-elfinder', "Select", "menubar:0;");
        }

        $scope.removeImage = function (img) {
            $scope.temp = $scope.listImages.filter(item => item !== img);
            $scope.listImages = $scope.temp
        }

        $scope.listAllCategory = [];
        $scope.getCategory = function () {
            apiService.get('/api/Categories/get-all-category', null, (result) => {
                $scope.listAllCategory = result.data;
            }, () => {
                alert('Get all list categories failed');
            });
        };
        $scope.getCategory();

        $scope.loadProductById = function () {
            apiService.get('/api/Products/get-product-by-id/' + $stateParams.id, null, (result) => {
                $scope.product = result.data;
                $scope.listImages = JSON.parse(result.data.listImages);
            }, (error) => {
                notificationService.displayError('Không tải sản phẩm được');
            });
        };
        $scope.loadProductById();

        $scope.updatePoduct = function () {
            $scope.product.listImages = JSON.stringify($scope.listImages);
            $scope.product.updatedBy = 'Quang Hào';
            console.log($scope.product);
            apiService.put('/api/Products/update-product/' + $stateParams.id, $scope.product, (result) => {
                notificationService.displaySuccess('Cập nhập sản phẩm thành công');
                $state.go('product_list')
            }, (error) => {
                notificationService.displayError('Cập nhập sản phẩm không thành công');
            });
        };

    };


})(angular.module('FashionShopProduct'));