/*  
* Người tạo : HuyNQ
* Menu: Báo cáo-> Báo cáo xuất bán lẻ
*/
define(['ui-bootstrap',], function () {
    'use strict';
    var app = angular.module('xuatBanLeModule', ['ui.bootstrap']);
    /* controller ShowReportController */
    app.controller('xuatBanLeReportController', ['$scope', '$uibModalInstance', '$location', '$http', 'configService', 'obj',
        function ($scope, $uibModalInstance, $location, $http, configService, obj) {
            $scope.para = angular.copy(obj);
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
            $scope.report = {
                name: "BTS.SP.API.Reports.XUATBANLE.XBANLE_TONGHOP,BTS.SP.API",
                title: $scope.para.TENBAOCAO,
                params: $scope.para
            };
        }]);

    app.controller('xuatBanLeDetailsReportController', ['$scope', '$uibModalInstance', 'obj',
        function ($scope, $uibModalInstance, obj) {
            $scope.para = angular.copy(obj);
            if ($scope.para && !$scope.para.P_BOHANG) $scope.para.P_BOHANG = null;
            if ($scope.para && !$scope.para.P_UNITUSER) $scope.para.P_UNITUSER = null;
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
            $scope.report = {
                name: "BTS.SP.API.Reports.XUATBANLE.XBANLE_CHITIET,BTS.SP.API",
                title: $scope.para.TENBAOCAO,
                params: $scope.para
            };
        }]);
    return app;
});