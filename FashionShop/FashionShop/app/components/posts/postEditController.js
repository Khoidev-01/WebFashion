/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('postEditController', postEditController);

    postEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams']

    function postEditController($scope, apiService, notificationService, $state, $stateParams) {
        $scope.post = {};
        var res = {};
        $scope.chooseImage = () => {
            window.fileSelected = function (data) {
                $scope.$apply(() => {
                    $scope.post.image = data.path;
                })
            };
            window.open('/file-manager-elfinder', "Select", "menubar:0;");
        };

        $scope.loadPostById = () => {
            apiService.get('/api/Posts/get-post-by-id/' + $stateParams.id, null, (result) => {
                $scope.post = result.data;

                res = $scope.post;

            }, (error) => {
                notificationService.displayError('Không tải bài viết được');
            });
        };
        $scope.loadPostById();

        $scope.updatePost = () => {
            apiService.put('/api/Posts/update-post-by-id/' + $stateParams.id, $scope.post, (result) => {
                notificationService.displaySuccess('Chỉnh sửa bài viết thành công');
                $state.go('post_list')
            }, (error) => {
                notificationService.displayError('Chỉnh sửa bài viết thất bại');
            });
        };
    };


})(angular.module('FashionShopPost'));