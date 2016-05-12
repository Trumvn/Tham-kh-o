/**
 * INSPINIA - Responsive Admin Theme
 *
 * Main controller.js file
 * Define controllers with data used in Inspinia theme
 *
 *
 * Functions (controllers)
 *  - MainCtrl
 *  - translateCtrl
 *
 */

/**
 * MainCtrl - controller
 * Contains severals global data used in diferent view
 *
 */

function MainCtrl($scope, $rootScope, $http, $modal, $localStorage, $location, authService) {

    $rootScope.UserPhotos = [];

    $scope.isAuthenticated = function () {
        if (!authService.isAuthenticated()) {
            $location.path("/login");
        }
    }
};


/**
 * translateCtrl - Controller for translate
 */
function translateCtrl($translate, $scope) {
    $scope.changeLanguage = function (langKey) {
        $translate.use(langKey);
    };
}


/**
 *
 * Pass all functions into module
 */
angular
    .module('inspinia')
    .controller('LoadingCtrl', LoadingCtrl)
    .controller('HeaderCtrl', HeaderCtrl)
    .controller('NavigatorCtrl', NavigatorCtrl)
    .controller('MainCtrl', MainCtrl)
    .controller('translateCtrl', translateCtrl)
    .controller('ProjectCtrl', ProjectCtrl)
    .controller('ProjectHeaderCtrl', ProjectHeaderCtrl)
    .controller('ProjectListCtrl', ProjectListCtrl)
    .controller('ProjectCategoryCtrl', ProjectCategoryCtrl)
    .controller('CategoryBudgetCtrl', CategoryBudgetCtrl)
    .controller('ProjectAccountCtrl', ProjectAccountCtrl)
    .controller('ProjectAnalyseCtrl', ProjectAnalyseCtrl)
    .controller('ProjectSummaryCtrl', ProjectSummaryCtrl)
    .controller('FaqCtrl', FaqCtrl)
    .controller('FaqAdminCtrl', FaqAdminCtrl)
    .controller('DashboardCtrl', DashboardCtrl)
    .controller('AccountRegisterCtrl', AccountRegisterCtrl)
    .controller('AccountLoginCtrl', AccountLoginCtrl)
    .controller('AccountForgotPasswordCtrl', AccountForgotPasswordCtrl)
    .controller('AccountChangePasswordCtrl', AccountChangePasswordCtrl)
    .controller('AccountResetPasswordCtrl', AccountResetPasswordCtrl)
    .controller('MessagingContentCtrl', MessagingContentCtrl)
    .controller('MessagingMailComposeCtrl', MessagingMailComposeCtrl)
    .controller('MessagingMailInboxCtrl', MessagingMailInboxCtrl)
    .controller('MessagingMailViewCtrl', MessagingMailViewCtrl)
    .controller('MessagingTemplateEditCtrl', MessagingTemplateEditCtrl)
    .controller('MessagingTemplateListCtrl', MessagingTemplateListCtrl)
    .controller('MessagingTemplateViewCtrl', MessagingTemplateViewCtrl)
    .controller('ClientProfileCtrl', ClientProfileCtrl)
    .controller('AdminContentCtrl', AdminClientsCtrl)
    .controller('AdminClientsCtrl', AdminClientsCtrl)
    .controller('ProjectViewCtrl', ProjectViewCtrl)
    .controller('AdminProjectsCtrl', AdminProjectsCtrl);


