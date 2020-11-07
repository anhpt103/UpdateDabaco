// 'use strict';
define(['angular'], function (angular) {

    var app = angular.module('authModule', ['configModule']);

    app.service('userService', ['localStorageService', function (localStorageService) {
        var fac = {};
        fac.CurrentUser = null;
        fac.SetCurrentUser = function (user) {
            fac.CurrentUser = user;
            localStorageService.set('authorizationData', user);
        };
        fac.GetCurrentUser = function () {
            fac.CurrentUser = localStorageService.get('authorizationData');
            return fac.CurrentUser;
        };
        return fac;
    }]);

    app.service('accountService', ['configService', '$http', '$q', 'localStorageService', '$state', 'userService', function (configService, $http, $q, localStorageService, $state, userService) {
        var result = {
            login: function (user) {
                var defer = $q.defer();
                let error = {
                    status: 400,
                    data: {
                        error: true,
                        error_description: ''
                    }
                }

                if (!user.username) {
                    error.data.error_description = 'Tên đăng nhập không thể trống';
                    defer.resolve(error);
                } else if (!user.password) {
                    error.data.error_description = 'Mật khẩu không thể trống';
                    defer.resolve(error);
                }

                var obj = { 'username': user.username, 'password': user.password, 'grant_type': 'password' };
                Object.toparams = function ObjectsToParams(obj) {
                    var p = [];
                    for (var key in obj) {
                        p.push(key + '=' + encodeURIComponent(obj[key]));
                    }
                    return p.join('&');
                }

                $http({ method: 'post', url: configService.apiServiceBaseUri + "/token", data: Object.toparams(obj), headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(function (response) {
                    if (response && response.status === 200 && response.data) {
                        userService.SetCurrentUser(response.data);
                        if (response.data && response.data.level && response.data.level !== '2') {
                            $state.go('home');
                        } else {
                            $state.go('banLe');
                        }
                    }
                    defer.resolve(response);
                }, function (response) {
                    defer.resolve(response);
                });
                return defer.promise;
            },
            logout: function () {
                localStorageService.cookie.clearAll();
                $state.go('login');
            }
        };
        return result;
    }]);

    app.controller('loginCrtl', ['$scope', '$location', '$http', 'localStorageService', 'accountService', '$state', function ($scope, $location, $http, localStorageService, accountService, $state) {
        $scope.msg = '';
        $scope.user = { username: '', password: '', cookie: false, grant_type: 'password' };
        $scope.login = function () {
            accountService.login($scope.user).then(function (response) {
                console.log(response);
                if (response && response.data) {
                    if (response.data.error) {
                        if (response.data.error_description.indexOf('incorrect') != -1) $scope.msg = 'Tên đăng nhập hoặc Mật khẩu không chính xác';
                        else $scope.msg = response.data.error_description;
                        document.getElementById('username').focus();
                    }
                }
            });
        };
    }]);
    return app;
});

