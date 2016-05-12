'use strict';

var Constants = (function () {
    var ViewUrl = {
        Dashboard: '/app/dashboard',
        login: 'login'
    };

    // local
    var rootApiUrl = '/portalapi/api';
    var authenticationService = '/portalapi/oauth/';

    // dev
    //var rootApiUrl = 'http://portal.aegona.com/api';
    //var authenticationService = 'http://portal.aegona.com/oauth/';

    var WebApi = {
        Application: {
            GetNavigator: rootApiUrl + '/application/getNavigator',
        },
        Accounts: {
            IsAuthenticated: rootApiUrl + '/accounts/isAuthenticated',
            CreateUser: rootApiUrl + '/accounts/createUser',
            ChangePassword: rootApiUrl + '/accounts/ChangePassword',
            GetCurrentUserProfile: rootApiUrl + '/accounts/GetCurrentUserProfile',
            UpdateCurrentUserProfile: rootApiUrl + '/accounts/UpdateCurrentUserProfile',
            ForgotPassword: rootApiUrl + '/accounts/ForgotPassword',
            ResetPasswordConfirmation: rootApiUrl + '/accounts/ResetPasswordConfirmation',
            ChangeCurrentUserPhoto: rootApiUrl + '/accounts/changeCurrentUserPhoto',
        },
        Messaging: {
            SendTestEmail: rootApiUrl + '/autoMessaging/sendTestEmail',
            GetMessagingContent: rootApiUrl + '/autoMessaging/getMessagingContent',
            GetMessages: rootApiUrl + '/autoMessaging/getMessages',
            GetTemplateContentTitles: rootApiUrl + '/autoMessaging/GetTemplateContentTitles',
            GetMailMessage: rootApiUrl + '/autoMessaging/getMailMessage',
            GetMailTemplateContent: rootApiUrl + '/autoMessaging/getMailTemplateContent',
            SaveMailTemplateContent: rootApiUrl + '/autoMessaging/saveMailTemplateContent',
        },
        Project: {
            CreateProject: rootApiUrl + '/projects/createProject',
            SearchProjects: rootApiUrl + '/projects/searchProjects',
            GetProjectMembers: rootApiUrl + '/projects/getProjectMembers',
            SearchTransactions: rootApiUrl + '/projects/searchTransactions',
            GetTransactions: rootApiUrl + '/projects/getTransactions',
            LoadProjectAnalyseInformation: rootApiUrl + '/projects/loadProjectAnalyseInformation',
            LoadTransactionSummaryByMonthYear: rootApiUrl + '/projects/loadTransactionSummaryByMonthYear',
            LoadTransactionSummaryByCategory: rootApiUrl + '/projects/loadTransactionSummaryByCategory',
            LoadTransactionSummaryByAccount: rootApiUrl + '/projects/loadTransactionSummaryByAccount',
            LoadTransactionCategorySummaryByMonthYear: rootApiUrl + '/projects/loadTransactionCategorySummaryByMonthYear',
            LoadMonthTotalIncome: rootApiUrl + '/projects/loadMonthTotalIncome',
            SaveCategoryBudget: rootApiUrl + '/projects/saveCategoryBudget',
            GetProjects: rootApiUrl + '/projects/getProjects',
            GetProjectSummary: rootApiUrl + '/projects/getProjectSummary',
            GetAccounts: rootApiUrl + '/projects/getAccounts',
            SaveAccount: rootApiUrl + '/projects/saveAccount',
            CloseAccount: rootApiUrl + '/projects/closeAccount',
            SetAccountPrimary: rootApiUrl + '/projects/setAccountPrimary',
            GetCategories: rootApiUrl + '/projects/getCategories',
            SaveCategory: rootApiUrl + '/projects/saveCategory',
            CloseCategory: rootApiUrl + '/projects/closeCategory',
            DeleteCategory: rootApiUrl + '/projects/deleteCategory',
            GetCategoryBudgets: rootApiUrl + '/projects/getCategoryBudgets',
            DeleteBudget: rootApiUrl + '/projects/deleteBudget',
            GetProjectHeader: rootApiUrl + '/projects/getProjectHeader',
            UpdateProject: rootApiUrl + '/projects/updateProject',
            GetProject: rootApiUrl + '/projects/getProject',
            InviteMember: rootApiUrl + '/projects/inviteMember',
            RemoveMember: rootApiUrl + '/projects/removeMember',
            GetAvailableCategories: rootApiUrl + '/projects/getAvailableCategories',
            SaveTransactions: rootApiUrl + '/projects/saveTransactions',
            SaveTransaction: rootApiUrl + '/projects/saveTransaction',
            DeleteTransaction: rootApiUrl + '/projects/deleteTransaction',
            SaveRecurringTransaction: rootApiUrl + '/projects/saveRecurringTransaction',
            RemoveRecurringTransaction: rootApiUrl + '/projects/removeRecurringTransaction',
            SetProjectHighlightColor: rootApiUrl + '/projects/setProjectHighlightColor',
            GetAccountCurrentBalance: rootApiUrl + '/projects/getAccountCurrentBalance',
            DeleteAccount: rootApiUrl + '/projects/deleteAccount',
            DeleteProject: rootApiUrl + '/projects/deleteProject',
            GetProjectFilter: rootApiUrl + '/projects/getProjectFilter',
            SaveProjectFilter: rootApiUrl + '/projects/saveProjectFilter',
            GetProjectForSummary: rootApiUrl + '/projects/GetProjectForSummary'
        },
        Client: {
            GetUserProfile: rootApiUrl + '/clients/getUserProfile',
            SaveUserProfileDetail: rootApiUrl + '/clients/saveUserProfileDetail',
            GetUserProfiles: rootApiUrl + '/clients/getUserProfiles',
            GetUserPhoto: rootApiUrl + '/clients/getUserPhoto',
        },
        Faq: {
            GetFaqs: rootApiUrl + '/faq/getFaqs',
            DeletFaq: rootApiUrl + '/faq/deletFag',
            GetFaq: rootApiUrl + '/faq/getFaq',
            CreateQuestion: rootApiUrl + '/faq/createQuestion',
            EditQuestion: rootApiUrl + '/faq/editQuestion',
        },
        AuditLog: {
            GetProjectAccountAuditLogs: rootApiUrl + '/auditlogs/getProjectAccountAuditLogs',
            GetProjectCategoryAuditLogs: rootApiUrl + '/auditlogs/getProjectCategoryAuditLogs',
            GetTransactionAuditLogs: rootApiUrl + '/auditlogs/getTransactionAuditLogs',
        },
        UserComment: {
            GetProjectAccountUserComments: rootApiUrl + '/usercomments/getProjectAccountUserComments',
            CreateProjectAccountUserComment: rootApiUrl + '/usercomments/createProjectAccountUserComment',
            GetProjectCategoryUserComments: rootApiUrl + '/usercomments/getProjectCategoryUserComments',
            CreateProjectCategoryUserComment: rootApiUrl + '/usercomments/createProjectCategoryUserComment',
            GetTransactionUserComments: rootApiUrl + '/usercomments/getTransactionUserComments',
            CreateTransactionUserComment: rootApiUrl + '/usercomments/createTransactionUserComment',
        },
        AdProject: {
            SearchProjects: rootApiUrl + '/adproject/searchProjects',
        },
    };

    var Events = {
        ProjectChanged: 'ProjectChanged',
        ProjectHeaderUpdated: 'ProjectHeaderUpdated',
        ProjectListChanged: 'ProjectListChanged'
    };

    var TimeFrequency = {
        Never: 1,
        Daily: 2,
        Weekly: 3,
        Monthly: 4,
        Yearly: 5,
        Periodically: 6,
    };

    return {
        RootApi: rootApiUrl,
        AuthenticationService: authenticationService,
        ViewUrl: ViewUrl,
        WebApi: WebApi,
        Events: Events,
        TimeFrequency: TimeFrequency
    };
})();