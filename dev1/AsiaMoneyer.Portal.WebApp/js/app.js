/**
 * INSPINIA - Responsive Admin Theme
 *
 */
(function () {
    angular.module('inspinia', [
        'ui.router',                    // Routing
        'oc.lazyLoad',                  // ocLazyLoad
        'ui.bootstrap',                 // Ui Bootstrap
        'pascalprecht.translate',       // Angular Translate
        'ngIdle',                       // Idle timer
        'ngSanitize',                    // ngSanitize
        'ngMaterial',
        'googlechart',
        'oitozero.ngSweetAlert',
        'angularSpinners',
        'angular-timeago',
        'ngStorage',
        'datePicker',
        'ImageCropper',
        'ngAutoSave',
        'isoCurrency'        
    ])
})();

// Other libraries are loaded dynamically in the config.js file using the library ocLazyLoad