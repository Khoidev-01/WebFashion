/// <reference path="../../../assets/admin/libs/angular-1.8.2/angular.js" />

(function (app) {
    app.controller('contactListController', contactListController);

    contactListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox']

    function contactListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.listContacts = [];

        $scope.page = 0;
        $scope.pageSize = 6;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.num = 0;

        $scope.searchByPhoneNumber;

        $scope.getListContacts = getListContacts;
        function getListContacts(page) {
            page = page || 0;
            var config = {
                params: {
                    page: page,
                    pageSize: $scope.pageSize,
                    searchByPhoneNumber: $scope.searchByPhoneNumber,
                }
            };

            apiService.get('/api/Contacts/get-all-contact', config, (result) => {
                $scope.listContacts = result.data.list;

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
        $scope.getListContacts();

        $scope.confirmContact = function (id) {
            $ngBootbox.confirm('Xác nhận đã giải viết liên hệ có id = ' + id).then(() => {
                apiService.put('/api/Contacts/cofirm-contact/' + id, null, () => {
                    notificationService.displaySuccess('Xác nhận thành công');
                    $scope.getListContacts();
                }, () => {
                    notificationService.displayError('Xác nhận thất bại');
                })
            });
        };
    };


})(angular.module('FashionShopContact'));