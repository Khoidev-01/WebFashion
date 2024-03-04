(function (app) {
    app.service('apiService', apiService);

    apiService.$inject = ['$http', 'notificationService', 'authenticationService'];

    function apiService($http, notificationService, authenticationService) {
        return {
            get: get,
            post: post,
            put: put,
            delete: del
        }

        function get(url, params, success, failed) {
            authenticationService.setHeader();
            $http.get(url, params).then((result) => {
                success(result);
            }, (error) => {
                failed(error);
            });
        }

        function post(url, data, success, failed) {
            authenticationService.setHeader();
            $http.post(url, data).then((result) => {
                success(result);
            }, (error) => {
                if (error == '401') {
                    notificationService.displayError('authentication failure');
                }
                else if (failed != null) {
                    failed(error);
                }
            });
        }

        function put(url, data, success, failed) {
            authenticationService.setHeader();
            $http.put(url, data).then((result) => {
                success(result);
            }, (error) => {
                if (error == '401') {
                    notificationService.displayError('authentication failure');
                }
                else if (failed != null) {
                    failed(error);
                }
            });
        }

        function del(url, data, success, failed) {
            authenticationService.setHeader();
            $http.delete(url, data).then((result) => {
                success(result);
            }, (error) => {
                if (error == '401') {
                    notificationService.displayError('authentication failure');
                }
                else if (failed != null) {
                    failed(error);
                }
            });
        }
    }
})(angular.module('FashionShopCommon'));