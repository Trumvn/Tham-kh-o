/**
 * INSPINIA - Responsive Admin Theme
 *
 * Inspinia theme use AngularUI Router to manage routing and views
 * Each view are defined as state.
 * Initial there are written state for all view in theme.
 *
 */
function config($stateProvider, $urlRouterProvider, $httpProvider, $localStorageProvider, $ocLazyLoadProvider, IdleProvider, KeepaliveProvider) {

    // Configure Idle settings
    IdleProvider.idle(5); // in seconds
    IdleProvider.timeout(120); // in seconds

    $httpProvider.interceptors.push('authInterceptorService');

    $urlRouterProvider.otherwise("/welcome");

    $ocLazyLoadProvider.config({
        // Set to true if you want to see what and when is dynamically loaded
        debug: false
    });

    $stateProvider
        .state('loading', {
            url: "/loading",
            templateUrl: "views/loading.html",
            controller: LoadingCtrl
        })
        .state('app.dashboard', {
            url: "/dashboard?show",
            templateUrl: "views/dashboard.html",
            controller: DashboardCtrl,
            data: { pageTitle: 'My Dashboard' }
        })        

        .state('app', {
            abstract: true,
            url: "/app",
            templateUrl: "views/common/content.html",
        })

        .state('app.faq', {
            url: "/faq",
            templateUrl: "views/faq.html",
            data: { pageTitle: 'FAQ' },
            controller:FaqCtrl,
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            name: 'localytics.directives',
                            files: ['css/plugins/chosen/chosen.css','js/plugins/chosen/chosen.jquery.js','js/plugins/chosen/chosen.js']
                        },
                        {
                            files: ['js/controllers/FaqQuestionCtrl.js']
                        }
                    ]);
                }
            }
        })

        .state('app.faqadmin', {
            url: "/faqadmin",
            templateUrl: "views/faqAdmin.html",
            data: { pageTitle: 'FAQ' },
            controller: FaqAdminCtrl,
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            name: 'localytics.directives',
                            files: ['css/plugins/chosen/chosen.css', 'js/plugins/chosen/chosen.jquery.js', 'js/plugins/chosen/chosen.js']
                        },
                        {
                            name: 'summernote',
                            files: ['css/plugins/summernote/summernote.css','css/plugins/summernote/summernote-bs3.css','js/plugins/summernote/summernote.min.js','js/plugins/summernote/angular-summernote.min.js']
                        },
                        {
                            files: ['js/controllers/FaqAdminQuestionCtrl.js']
                        }
                    ]);
                }
            }
        })

        .state('app.project', {
            url: "/project?id",
            templateUrl: "views/proj/content.html",
            controller: function ($scope, $state) {
                var parentState = 'app.project',
                defaultChildState = '.list';
                // If the parent state has been transitioned to, redirect to the default child.         
                if ($state.current.name.substr(-parentState.length) === parentState) {
                    $state.go(defaultChildState);
                }
            }
        })

        .state('app.project.list', {
            url: "/list",
            templateUrl: "views/proj/list.html",
            controller: ProjectListCtrl,
            data: { pageTitle: 'Project' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                    {
                        name: 'datePicker',
                        files: ['css/plugins/datapicker/angular-datapicker.css', 'js/plugins/datapicker/angular-datepicker.js']

                    },
                    {
                        files: ['css/plugins/iCheck/custom.css', 'js/plugins/iCheck/icheck.min.js']
                    }
                    ]);
                }
            }
        })

        .state('app.project.category', {
            url: "/category",
            templateUrl: "views/proj/category.html",
            controller: ProjectCategoryCtrl,
            data: { pageTitle: 'Project Category' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                    {
                        name: 'datePicker',
                        files: ['css/plugins/datapicker/angular-datapicker.css', 'js/plugins/datapicker/angular-datepicker.js']

                    },
                    {
                        files: ['css/plugins/iCheck/custom.css', 'js/plugins/iCheck/icheck.min.js']
                    }
                    ]);
                }
            }
        })

        .state('app.project.account', {
            url: "/account",
            templateUrl: "views/proj/account.html",
            controller: ProjectAccountCtrl,
            data: { pageTitle: 'Project Account' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                    {
                        name: 'datePicker',
                        files: ['css/plugins/datapicker/angular-datapicker.css', 'js/plugins/datapicker/angular-datepicker.js']

                    },
                    {
                        files: ['css/plugins/iCheck/custom.css', 'js/plugins/iCheck/icheck.min.js']
                    }
                    ]);
                }
            }
        })

        .state('app.project.analyse', {
            url: "/analyse",
            templateUrl: "views/proj/analyse.html",
            controller: ProjectAnalyseCtrl,
            data: { pageTitle: 'Project Analyse' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ui.checkbox',
                            files: ['js/bootstrap/angular-bootstrap-checkbox.js']
                        }
                    ]);
                }
            }
        })
        .state('app.project.summary', {
            url: "/summary",
            controller:ProjectSummaryCtrl,
            templateUrl: "views/proj/summary.html",
            data: { pageTitle: 'Project Summary' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'angles',
                            files: ['js/plugins/chartJs/angles.js', 'js/plugins/chartJs/Chart.min.js']
                        },
                        {
                            name: 'angular-peity',
                            files: ['js/plugins/peity/jquery.peity.min.js', 'js/plugins/peity/angular-peity.js']
                        },
                        {
                            name: 'ui.checkbox',
                            files: ['js/bootstrap/angular-bootstrap-checkbox.js']
                        }
                    ]);
                }
            }
        })
        .state('app.account', {
            abstract: true,
            url: "/account",
            templateUrl: "views/account/content.html",
            controller: AccountContentCtrl,
        })
        .state('app.account.youraccount', {
            url: "/youraccount",
            templateUrl: "views/account/your_account.html",
            controller: AccountYourAccountCtrl,
            data: { pageTitle: 'Your Account' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {                            
                        }
                    ]);
                }
            }
        })
        .state('app.account.upgrade', {
            url: "/upgrade",
            templateUrl: "views/account/upgrade_account.html",
            controller: AccountUpgradeCtrl,
            data: { pageTitle: 'Upgrade Account' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                        }
                    ]);
                }
            }
        })
        .state('app.account.payment', {
            url: "/payment?id",
            templateUrl: "views/account/payment.html",
            controller: AccountPaymentCtrl,
            data: { pageTitle: 'Payment' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                        }
                    ]);
                }
            }
        })
        .state('app.account.pmethod', {
            url: "/pmethod?id",
            templateUrl: "views/account/payment_method.html",
            controller: AccountPaymentMethodCtrl,
            data: { pageTitle: 'Payment Method' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                        }
                    ]);
                }
            }
        })
        .state('app.msg', {
            abstract: true,
            url: "/msg",
            templateUrl: "views/msg/content.html",
            controller: MessagingContentCtrl,
        })
        .state('app.msg.mail_inbox', {
            url: "/inbox",
            templateUrl: "views/msg/mail_inbox.html",
            controller: MessagingMailInboxCtrl,
            data: { pageTitle: 'Mail Inbox' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            files: ['css/plugins/iCheck/custom.css', 'js/plugins/iCheck/icheck.min.js']
                        }
                    ]);
                }
            }
        })
        .state('app.msg.mail_view', {
            url: "/view?Id",
            templateUrl: "views/msg/mail_view.html",
            controller: MessagingMailViewCtrl,
            data: { pageTitle: 'Mail detail' }
        })
        .state('app.msg.mail_compose', {
            url: "/compose",
            templateUrl: "views/msg/mail_compose.html",
            controller: MessagingMailComposeCtrl,
            data: { pageTitle: 'Mail compose' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            files: ['css/plugins/summernote/summernote.css', 'css/plugins/summernote/summernote-bs3.css', 'js/plugins/summernote/summernote.min.js']
                        },
                        {
                            name: 'summernote',
                            files: ['css/plugins/summernote/summernote.css', 'css/plugins/summernote/summernote-bs3.css', 'js/plugins/summernote/summernote.min.js', 'js/plugins/summernote/angular-summernote.min.js']
                        }
                    ]);
                }
            }
        })
        .state('app.msg.tlist', {
            url: "/tlist?Id",
            templateUrl: "views/msg/template_list.html",
            controller: MessagingTemplateListCtrl,
            data: { pageTitle: 'Mail Inbox' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            files: ['css/plugins/iCheck/custom.css', 'js/plugins/iCheck/icheck.min.js']
                        }
                    ]);
                }
            }
        })
        .state('app.msg.tview', {
            url: "/tview?Id",
            templateUrl: "views/msg/template_view.html",
            controller: MessagingTemplateViewCtrl,
            data: { pageTitle: 'Mail detail' }
        })
        .state('app.msg.tedit', {
            url: "/tedit?id&tid",
            templateUrl: "views/msg/template_edit.html",
            controller: MessagingTemplateEditCtrl,
            data: { pageTitle: 'Mail compose' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            files: ['css/plugins/summernote/summernote.css', 'css/plugins/summernote/summernote-bs3.css', 'js/plugins/summernote/summernote.min.js']
                        },
                        {
                            name: 'summernote',
                            files: ['css/plugins/summernote/summernote.css', 'css/plugins/summernote/summernote-bs3.css', 'js/plugins/summernote/summernote.min.js', 'js/plugins/summernote/angular-summernote.min.js']
                        }
                    ]);
                }
            }
        })
        .state('pages.empy_page', {
            url: "/empy_page",
            templateUrl: "views/empty_page.html",
            data: { pageTitle: 'Empty page' }
        })
        .state('login', {
            url: "/login?logout",
            templateUrl: "views/login.html",
            controller: AccountLoginCtrl,
            data: { pageTitle: 'Login', specialClass: 'gray-bg' }
        })
        .state('register', {
            url: "/register",
            templateUrl: "views/register.html",
            controller: AccountRegisterCtrl,
            data: { pageTitle: 'Register', specialClass: 'gray-bg' }
        })
        .state('lockscreen', {
            url: "/lockscreen",
            templateUrl: "views/lockscreen.html",
            data: { pageTitle: 'Lockscreen', specialClass: 'gray-bg' }
        })
        .state('forgot_password', {
            url: "/forgot_password",
            templateUrl: "views/forgot_password.html",
            controller: AccountForgotPasswordCtrl,
            data: { pageTitle: 'Forgot password', specialClass: 'gray-bg' }
        })
        .state('reset_password', {
            url: "/reset_password",
            templateUrl: "views/reset_password.html",
            controller: AccountResetPasswordCtrl,
            data: { pageTitle: 'Reset password', specialClass: 'gray-bg' }
        })
        .state('errorOne', {
            url: "/errorOne",
            templateUrl: "views/errorOne.html",
            data: { pageTitle: '404', specialClass: 'gray-bg' }
        })
        .state('errorTwo', {
            url: "/errorTwo",
            templateUrl: "views/errorTwo.html",
            data: { pageTitle: '500', specialClass: 'gray-bg' }
        })

        .state('landing', {
            url: "/landing",
            templateUrl: "views/landing.html",
            data: { pageTitle: 'Landing page', specialClass: 'landing-page' }
        })

        .state('welcome', {
            url: "/welcome",
            templateUrl: "views/welcome.html",
            data: { pageTitle: 'Welcome', specialClass: 'landing-page' }
        })

        .state('change_password', {
            url: "/change_password",
            templateUrl: "views/change_password.html",
            controller: AccountChangePasswordCtrl,
            data: { pageTitle: 'Change password', specialClass: 'gray-bg' }
        });
}
angular
    .module('inspinia')
    .constant('Constants', Constants)
    .constant('Commons', Commons)
    .factory('authService', authService)
    .factory('authInterceptorService', authInterceptorService)
    .factory('userPhotoService', userPhotoService)
    .config(config)
    .run(function($rootScope, $state) {
        $rootScope.$state = $state;
    });
