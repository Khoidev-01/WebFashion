/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('productDetailController', productDetailController);

    productDetailController.$inject = ['$scope', 'apiService', 'notificationService', '$stateParams']

    function productDetailController($scope, apiService, notificationService, $stateParams) {
        $scope.product = [];
        $scope.listImages = [];

        $scope.loadProductById = function () {
            apiService.get('/api/Products/get-product-by-id/' + $stateParams.id, null, (result) => {
                $scope.product = result.data;
                $scope.listImages = JSON.parse(result.data.listImages);

                document.querySelector('#product-desc').innerHTML = $scope.product.describe;
            }, (error) => {
                notificationService.displayError('Không tải sản phẩm được');
            });
        };
        $scope.loadProductById();

        $scope.choose = function () {
            $('.product-image-thumb').on('click', function () {
                var $image_element = $(this).find('img')
                $('.product-image').prop('src', $image_element.attr('src'))
                $('.product-image-thumb.active').removeClass('active')
                $(this).addClass('active')
            })
        };
    };


})(angular.module('FashionShopProduct'));