/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('postListController', postListController);

    postListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox']

    function postListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.listPosts = [];

        $scope.getListPosts = getListPosts;
        function getListPosts() {
            apiService.get('/api/Posts/get-all-post', null, (result) => {
                $scope.listPosts = result.data;

            }, () => {
                console.log("Không tải được danh sách bài viết");
            });
        }
        $scope.getListPosts();

        $scope.loadConent = function (content, id) {
            console.log("KQ: " + id + " " + content);
            document.querySelector('#content'+id).innerHTML = content;
        }

        $scope.deletePost = (id) => {
            $ngBootbox.confirm('Bạn có muốn xóa bài viết có id = ' + id).then(() => {
                apiService.delete('/api/Posts/delete-post-by-id/' + id, null, () => {
                    notificationService.displaySuccess('Xóa bài viết thành công');
                    $scope.getListPosts();
                }, () => {
                    notificationService.displayError('Xóa bài viết thất bại');
                })
            });
        };
    };


})(angular.module('FashionShopPost'));