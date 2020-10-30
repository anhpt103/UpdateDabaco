require.config({
    base: '/',
    paths: {
        'jquery': 'utils/kendo/2020.2.617/js/jquery.min',
        'jquery-ui': 'utils/jquery-ui/jquery-ui.min',
        'bootstrap': 'lib/bootstrap.min',
        'angular': 'lib/angular.min',
        'angular-animate': 'lib/angular-animate.min',
        'angular-resource': 'lib/angular-resource.min',
        'angular-filter': 'lib/angular-filter.min',
        'angular-sanitize': 'lib/angular-sanitize.min',
        'angular-cache': 'lib/angular-cache.min',
        'ocLazyLoad': 'lib/ocLazyLoad.require',
        'uiRouter': 'lib/angular-ui-router.min',
        'angularStorage': 'lib/angular-local-storage.min',
        'ui-bootstrap': 'lib/ui-bootstrap-tpls-1.3.3',
        'smartTable': 'utils/smart-table.min',
        'ngTable': 'utils/ng-table.min',
        'ngNotify': 'utils/ng-notify/ng-notify.min',
        'ui.tree': 'lib/angular-ui-tree.min',
        'dynamic-number': 'utils/dynamic-number.min',
        'kendo': 'utils/kendo/2020.2.617/js/kendo.all.min',
        'telerikReportViewer': 'utils/telerik/js/telerikReportViewer-14.0.20.115.min',
        'toaster': 'utils/toaster/toaster.min',
        'ui-grid': 'utils/ui-grid/ui-grid.min',
        'angular-confirm': 'utils/angular-confirm/angular-confirm.min',
        'fileUpload': 'lib/angular-file-upload.min',
        'ng-file-upload': 'utils/ng-file-upload-all.min',
        'ng-tags-input': 'utils/ng-tags-input/ng-tags-input.min',
        'ckeditor': 'utils/ckeditor/ckeditor',
        'ng-ckeditor': 'utils/ckeditor/ng-ckeditor',
        'ngMaterial': 'utils/meterial/angular-material/angular-material.min',
        'ngAria': 'utils/meterial/angular-aria/angular-aria.min',
        'angular-md5': 'utils/angular-md5',
        'adapt-strap': 'utils/adapt-strap/adapt-strap',
        'chart-js': 'utils/angular-chart/Chart.min',
        'angular-chart': 'utils/angular-chart/angular-chart.min',
    },
    shim: {
        'jquery': {
            exports: '$'
        },
        'jquery-ui': ['jquery'],
        'bootstrap': ['jquery'],
        'angular': {
            deps: ['jquery', 'bootstrap'],
            exports: 'angular'
        },
        'ocLazyLoad': ['angular'],
        'uiRouter': ['angular'],
        'angular-animate': ['angular'],
        'angular-resource': ['angular'],
        'angular-filter': ['angular'],
        'angular-cache': ['angular'],
        'angular-sanitize': ['angular'],
        'angularStorage': ['angular'],
        'ui-bootstrap': ['angular'],
        'smartTable': ['angular'],
        'ngTable': ['angular'],
        'ngNotify': ['angular'],
        'ui.tree': ['angular'],
        'dynamic-number': ['angular'],
        'kendo': ['jquery', 'angular'],
        'telerikReportViewer': ['jquery', 'angular'],
        'toaster': ['angular'],
        'ui-grid': ['angular'],
        'angular-confirm': ['angular'],
        'fileUpload': ['angular'],
        'ng-file-upload': ['angular'],
        'ng-tags-input': ['angular'],
        'ckeditor': ['angular'],
        'ng-ckeditor': ['angular'],
        'ngMaterial': ['angular'],
        'ngAria': ['angular'],
        'angular-md5': ['angular'],
        'adapt-strap': ['angular'],
        'chart-js': ['angular'],
        'angular-chart': ['chart-js'],
    },
    waitSeconds: 0,
    urlArgs: 'bust=' + new Date().getTime()
});

// Start the main app logic.
require(['app'], function (app) {
    angular.bootstrap(document.body, ['myApp']);
});
