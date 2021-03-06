require.config({
    base: '/',
    paths: {
        'domReady': 'utils/require/2.3.6/domReady',
        'r': 'utils/require/2.3.6/r',
        'angular': 'lib/angular.min',
        'jquery': 'utils/kendo/2020.2.617/js/jquery.min',
        'jquery-ui': 'utils/jquery-ui/jquery-ui.min',
        'angularStorage': 'lib/angular-local-storage.min',
        'bootstrap': 'lib/bootstrap.min',
        'angular-animate': 'lib/angular-animate.min',
        'angular-resource': 'lib/angular-resource.min',
        'angular-filter': 'lib/angular-filter.min',
        'angular-sanitize': 'lib/angular-sanitize.min',
        'angular-cache': 'lib/angular-cache.min',
        'ocLazyLoad': 'lib/ocLazyLoad.require',
        'uiRouter': 'lib/angular-ui-router.min',
        'ui-bootstrap': 'lib/ui-bootstrap-tpls-1.3.3',
        'smartTable': 'utils/smart-table.min',
        'ngTable': 'utils/ng-table.min',
        'ngNotify': 'utils/ng-notify/ng-notify.min',
        'ui.tree': 'lib/angular-ui-tree.min',
        'dynamic-number': 'utils/dynamic-number.min',
        'kendo.all.min': 'utils/kendo/2020.2.617/js/kendo.all.min',
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
        'bootstrapper': 'bootstrapper',
        'moment': 'lib/moment.min',
        'moment-local': 'lib/moment-with-locales.min',
        'angularMoment': 'lib/angular-moment.min',
    },
    shim: {
        'angular': {
            deps: ['jquery', 'jquery-ui'],
            exports: 'angular'
        },
        'jquery': {
            exports: '$'
        },
        'jquery-ui': ['jquery'],
        'angularStorage': ['angular'],
        'bootstrap': ['jquery'],
        'ocLazyLoad': ['angular'],
        'uiRouter': ['angular'],
        'angular-animate': ['angular'],
        'angular-resource': ['angular'],
        'angular-filter': ['angular'],
        'angular-cache': ['angular'],
        'angular-sanitize': ['angular'],
        'ui-bootstrap': ['angular'],
        'smartTable': ['angular'],
        'ngTable': ['angular'],
        'ngNotify': ['angular'],
        'ui.tree': ['angular'],
        'dynamic-number': ['angular'],
        'kendo.all.min': ['angular'],
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
        'bootstrapper': {
            deps: [
                'angular',
                'app'
            ]
        },
    },
    waitSeconds: 0,
    deps: ['bootstrapper']
});