(function (app) {
    'use strict';
    app.service('loginService', ['$http', '$q', 'authenticationService', 'authData',
        function ($http, $q, authenticationService, authData) {
            var userInfo;
            var deferred;

            this.login = function (email, password) {
                deferred = $q.defer();

                var loginModel = {
                    Email: email,
                    Password: password
                };

                $http.post('/api/Users/login', loginModel, {
                    headers: { 'Content-Type': 'application/json' }
                }).then(function successCallback(response) {
                    userInfo = {
                        accessToken: response.data,
                        email: email
                    };
                    authenticationService.setTokenInfo(userInfo);
                    authData.authenticationData.IsAuthenticated = true;
                    authData.authenticationData.email = email;
                    deferred.resolve(null);
                }, function errorCallback(err) {
                    authData.authenticationData.IsAuthenticated = false;
                    authData.authenticationData.email = "";
                    deferred.resolve(err);
                });

                return deferred.promise;
            }

            this.logOut = function () {
                authenticationService.removeToken();
                authData.authenticationData.IsAuthenticated = false;
                authData.authenticationData.email = "";
            }
        }]);
})(angular.module('FashionShopCommon'));