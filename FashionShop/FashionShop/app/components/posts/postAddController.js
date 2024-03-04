/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('postAddController', postAddController);

    postAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state']

    function postAddController($scope, apiService, notificationService, $state) {
        $scope.post = {
            status: true
        };

        $scope.chooseImage = () => {
            window.fileSelected = function (data) {
                $scope.$apply(() => {
                    $scope.post.image = data.path;
                })
            };
            window.open('/file-manager-elfinder', "Select", "menubar:0;");
        };

        $scope.addPost = () => {
            apiService.post('/api/Posts/add-post', $scope.post, (result) => {
                notificationService.displaySuccess('Tạo bài viết thành công');
                $state.go('post_list');

            }, (error) => {
                notificationService.displayError('Tạo bài viết thất bại');
            });
        };
    };

})(angular.module('FashionShopPost'));