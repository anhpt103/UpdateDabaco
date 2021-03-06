﻿/*  
* Người tạo : Phạm Tuấn Anh
* View: BTS.SP.MART/views/htdm/merchandiseType
* Vm sevices: BTS.API.SERVICE -> MD ->MdMerchandiseTypeVm.cs
* Sevices: BTS.API.SERVICE -> MD -> MdMerchandiseTypeService.cs
* Entity: BTS.API.ENTITY -> Md - > MdMerchandiseType.cs
* Menu: Danh mục-> Danh mục loại vật tư
*/
define(['ui-bootstrap'], function () {
    'use strict';
    var app = angular.module('merchandiseTypeModule', ['ui.bootstrap']);
    app.factory('merchandiseTypeService', ['$http', 'configService', function ($http, configService) {
        var serviceUrl = configService.rootUrlWebApi + '/Md/MerchandiseType';
        var selectedData = [];
        var result = {
            postQuery: function (data) {
                return $http.post(serviceUrl + '/PostQuery', data);
            },
            post: function (data) {
                return $http.post(serviceUrl + '/Post', data);
            },
            postSelectData: function (data) {
                return $http.post(serviceUrl + '/PostSelectData', data);
            },
            getNewInstance: function () {
                return $http.get(serviceUrl + '/GetNewInstance');
            },
            getSelectAll: function () {
                return $http.get(serviceUrl + '/GetSelectAll');
            },
            update: function (params) {
                return $http.put(serviceUrl + '/' + params.id, params);
            },
            deleteItem: function (params) {
                return $http.delete(serviceUrl + '/' + params.id, params);
            },
            filterTypeMerchandiseCodes: function (maLoai) {
                return $http.post(serviceUrl + '/FilterTypeMerchandiseCodes/' + maLoai);
            },
            //service set ; get data
            getSelectData: function () {
                return selectedData;
            },
            setSelectData: function (array) {
                selectedData = array;
            },
            //end service
            getAll_MerchandiseType: function () {
                return $http.get(serviceUrl + '/GetAll_MerchandiseType');
            },
            getAll_MerchandiseTypeRoot: function () {
                return $http.get(serviceUrl + '/GetAll_MerchandiseTypeRoot');
            },
            clearSelectedData: function () {
                selectedData = [];
            }
        }
        return result;
    }]);
    /* controller list */
    app.controller('merchandiseTypeController', ['$scope', '$location', '$http', 'configService', 'merchandiseTypeService', 'tempDataService', '$filter', '$uibModal', '$log', 'ngNotify', 'securityService', 'toaster',
        function ($scope, $location, $http, configService, service, tempDataService, $filter, $uibModal, $log, ngNotify, securityService, toaster) {
            $scope.config = angular.copy(configService);
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.tempData = tempDataService.tempData;
            $scope.isEditable = true;
            $scope.disabledSave = false;
            //load dữ liệu
            function filterData() {
                $scope.isLoading = true;
                if ($scope.accessList.view) {
                    var postdata = { paged: $scope.paged, filtered: $scope.filtered };
                    service.postQuery(postdata).then(function (successRes) {
                        console.log('successRes', successRes);
                        if (successRes && successRes.status === 200 && successRes.data && successRes.data.status) {
                            $scope.isLoading = false;
                            $scope.data = successRes.data.data.data;
                            angular.extend($scope.paged, successRes.data.data);
                        }
                    }, function (errorRes) {
                        console.log(errorRes);
                    });
                }
            };
            //end
            //check quyền truy cập
            function loadAccessList() {
                securityService.getAccessList('merchandiseType').then(function (successRes) {
                    console.log(successRes);
                    if (successRes && successRes.status == 200) {
                        $scope.accessList = successRes.data;
                        if (!$scope.accessList.view) {
                            toaster.pop('error', "Lỗi:", "Không có quyền truy cập !");
                        } else {
                            filterData();
                        }
                    } else {
                        toaster.pop('error', "Lỗi:", "Không có quyền truy cập !");
                    }
                }, function (errorRes) {
                    console.log(errorRes);
                    toaster.pop('error', "Lỗi:", "Không có quyền truy cập !");
                    $scope.accessList = null;
                });
            }
            loadAccessList();
            //end

            $scope.setPage = function (pageNo) {
                $scope.paged.currentPage = pageNo;
                filterData();
            };
            $scope.sortType = 'mamerchandiseType';
            $scope.sortReverse = false;
            $scope.doSearch = function () {
                $scope.paged.currentPage = 1;
                filterData();
            };
            $scope.pageChanged = function () {
                filterData();
            };
            $scope.goHome = function () {
                window.location.href = "#!/home";
            };
            $scope.refresh = function () {
                $scope.setPage($scope.paged.currentPage);
            };
            $scope.title = function () { return 'Danh sách loại Vật tư, hàng hóa' };

            /* Function add New Item */
            $scope.create = function () {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    size: 'md',
                    templateUrl: configService.buildUrl('htdm/MerchandiseType', 'add'),
                    controller: 'merchandiseTypeCreateController',
                    resolve: {}
                });
                modalInstance.result.then(function (updatedData) {
                    $scope.refresh();
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };

            /* Function Edit Item */
            $scope.update = function (target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    size: 'md',
                    templateUrl: configService.buildUrl('htdm/MerchandiseType', 'update'),
                    controller: 'merchandiseTypeEditController',
                    resolve: {
                        targetData: function () {
                            return target;
                        }
                    }
                });
                modalInstance.result.then(function (updatedData) {
                    $scope.refresh();
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };

            /* Function Details Item */
            $scope.details = function (target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    templateUrl: configService.buildUrl('htdm/MerchandiseType', 'details'),
                    controller: 'merchandiseTypeDetailsController',
                    size: 'md',
                    resolve: {
                        targetData: function () {
                            return target;
                        }
                    }
                });
                modalInstance.result.then(function (updatedData) {
                    $scope.refresh();
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };

            /* Function Delete Item */
            $scope.deleteItem = function (event, target) {
                var modalInstance = $uibModal.open({
                    backdrop: 'static',
                    templateUrl: configService.buildUrl('htdm/MerchandiseType', 'delete'),
                    controller: 'merchandiseTypeDeleteController',
                    resolve: {
                        targetData: function () {
                            return target;
                        }
                    }
                });
                modalInstance.result.then(function (updatedData) {
                    $scope.refresh();
                }, function () {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };
        }]);

    /* controller addNew */
    app.controller('merchandiseTypeCreateController', ['$scope', '$uibModalInstance', '$location', '$http', 'configService', 'merchandiseTypeService', 'tempDataService', '$filter', '$uibModal', '$log', 'ngNotify',
        function ($scope, $uibModalInstance, $location, $http, configService, service, tempDataService, $filter, $uibModal, $log, ngNotify) {
            $scope.robot = angular.copy(service.robot);
            $scope.config = angular.copy(configService);
            $scope.target = {};
            $scope.isGenCode = true;
            $scope.disableGenCode = true;
            $scope.tempData = tempDataService.tempData;
            service.getNewInstance().then(function (resNewIn) {
                if (resNewIn && resNewIn.status == 200 && resNewIn.data) {
                    $scope.target = resNewIn.data;
                    $scope.target.maLoaiVatTu = resNewIn.data.maLoaiVatTu;
                    $scope.target.trangThai = 10;
                }
            });
            $scope.unAuToGenCode = function (event) {
                if (event.target.checked) {
                    $scope.disableGenCode = false;
                    $scope.isGenCode = false;
                }
                else {
                    $scope.disableGenCode = true;
                    $scope.isGenCode = true;
                }
            };
            $scope.isLoading = false;
            $scope.title = function () { return 'Thêm loại Vật tư, Hàng hóa'; };
            $scope.save = function () {
                $scope.target.isGenCode = $scope.isGenCode;
                service.post($scope.target).then(function (successRes) {
                    if (successRes && successRes.status === 201 && successRes.data.status) {
                        ngNotify.set(successRes.data.message, { type: 'success' });
                        $uibModalInstance.close($scope.target);
                    } else {
                        console.log('addNew successRes', successRes);
                        ngNotify.set(successRes.data.message, { duration: 3000, type: 'error' });
                    }
                },
                    function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    /* controller Edit */
    app.controller('merchandiseTypeEditController', ['$scope', '$uibModalInstance', '$location', '$http', 'configService', 'merchandiseTypeService', 'tempDataService', '$filter', '$uibModal', '$log', 'targetData', 'ngNotify',
        function ($scope, $uibModalInstance, $location, $http, configService, service, tempDataService, $filter, $uibModal, $log, targetData, ngNotify) {
            $scope.config = angular.copy(configService);
            $scope.targetData = angular.copy(targetData);
            $scope.tempData = tempDataService.tempData;
            $scope.target = targetData;
            $scope.isLoading = false;
            $scope.title = function () { return 'Cập nhập loại Vật tư, Hàng hóa'; };
            function filterData() {
                service.getSelectAll().then(function (response) {
                    $scope.merchandiseTypeSort = response;
                });
            }
            filterData();
            $scope.save = function () {
                service.update($scope.target).then(function (successRes) {
                    if (successRes && successRes.status === 200 && successRes.data.status) {
                        ngNotify.set(successRes.data.message, { type: 'success' });
                        $uibModalInstance.close($scope.target);
                    } else {
                        console.log('update successRes', successRes);
                        ngNotify.set(successRes.data.message, { duration: 3000, type: 'error' });
                    }
                },
                    function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);

    /* controller Details */
    app.controller('merchandiseTypeDetailsController', ['$scope', '$uibModalInstance', '$location', '$http', 'configService', 'merchandiseTypeService', 'tempDataService', '$filter', '$uibModal', '$log', 'targetData', 'ngNotify',
        function ($scope, $uibModalInstance, $location, $http, configService, service, tempDataService, $filter, $uibModal, $log, targetData, ngNotify) {
            $scope.config = angular.copy(configService);
            $scope.targetData = angular.copy(targetData);
            $scope.tempData = tempDataService.tempData;
            $scope.target = targetData;
            $scope.title = function () { return 'Thông tin loại Vật tư, Hàng hóa'; };
            function filterData() {
                service.getSelectAll().then(function (response) {
                    $scope.merchandiseTypeSort = response;
                });
            }
            filterData();
            $scope.cancel = function () {
                $uibModalInstance.close();
            };

        }]);
    /* controller delete */
    app.controller('merchandiseTypeDeleteController', ['$scope', '$uibModalInstance', '$location', '$http', 'configService', 'merchandiseTypeService', 'tempDataService', '$filter', '$uibModal', '$log', 'targetData', 'ngNotify',
        function ($scope, $uibModalInstance, $location, $http, configService, service, tempDataService, $filter, $uibModal, $log, targetData, ngNotify) {
            $scope.config = angular.copy(configService);
            $scope.targetData = angular.copy(targetData);
            $scope.target = targetData;
            $scope.isLoading = false;
            $scope.title = function () { return 'Xoá thành phần'; };
            $scope.save = function () {
                service.deleteItem($scope.target).then(function (successRes) {
                    if (successRes && successRes.status === 200) {
                        ngNotify.set(successRes.data.message, { type: 'success' });
                        $uibModalInstance.close($scope.target);
                    } else {
                        ngNotify.set(successRes.data.message, { duration: 3000, type: 'error' });
                    }
                },
                    function (errorRes) {
                        console.log('errorRes', errorRes);
                    });
            };
            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    /* controller delete */
    app.controller('merchandiseTypeSelectDataController', ['$scope', '$uibModalInstance', '$location', '$http', 'configService', 'merchandiseTypeService', 'tempDataService', '$filter', '$uibModal', '$log', 'ngNotify', 'filterObject', 'serviceSelectData',
        function ($scope, $uibModalInstance, $location, $http, configService, service, tempDataService, $filter, $uibModal, $log, ngNotify, filterObject, serviceSelectData) {
            $scope.config = angular.copy(configService);;
            $scope.paged = angular.copy(configService.pageDefault);
            $scope.filtered = angular.copy(configService.filterDefault);
            $scope.filtered = angular.extend($scope.filtered, filterObject);
            angular.extend($scope.filtered, filterObject);
            $scope.all = false;

            $scope.sortType = 'maLoaiVatTu'; // set the default sort type
            $scope.sortReverse = false;  // set the default sort order

            function filterData() {
                $scope.listSelectedData = serviceSelectData.getSelectData();
                $scope.isLoading = true;
                var postdata = { paged: $scope.paged, filtered: $scope.filtered };
                service.postSelectData(postdata).then(function (response) {
                    $scope.isLoading = false;
                    if (response.status) {
                        $scope.data = response.data.data.data;
                        $scope.all = configService.filterDataForSelectData($scope.data, $scope.listSelectedData, $scope.all)
                        angular.extend($scope.paged, response.data.data);
                    }
                });
            };

            filterData();
            $scope.doSearch = function () {
                $scope.paged.currentPage = 1;
                filterData();
            };
            $scope.pageChanged = function () {
                filterData();
            };
            $scope.refresh = function () {
                $scope.setPage($scope.paged.currentPage);
            };
            $scope.title = function () {
                return 'Loại hàng';
            };
            $scope.setPage = function (pageNo) {
                $scope.paged.currentPage = pageNo;
                filterData();
            };

            $scope.selecteItem = function (item) {
                $uibModalInstance.close(item);
            }

            $scope.doCheck = function (item) {
                $scope.all = configService.doCheckDataForSelectData(item, $scope.data, $scope.all);
            }

            $scope.save = function () {
                let result = $filter('filter')($scope.data, { selected: true }, true);
                service.setSelectData(result);
                $uibModalInstance.close(result);
            };

            $scope.cancel = function () {
                $uibModalInstance.close();
            };
        }]);
    return app;
});