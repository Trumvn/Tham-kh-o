function ProjectListCtrl($scope, $rootScope, $http, $localStorage, $modal, $stateParams, SweetAlert, timeAgo, nowTime, userPhotoService, Commons, Constants) {
    
    $scope.TimeFrequency = Constants.TimeFrequency;

    $scope.loading = true;
    $scope.IsShowIncomeExpenseSummary = false;

    $scope.totalAmount = 0;
    $scope.totalIncome = 0;
    $scope.totalExpense = 0;

    $scope.Project = {
        Id: $stateParams.id,
        Title: '',
        Currency: null
    };

    $scope.NoneCategoryFilter = {
        Id: null,
        CategoryTitle: 'All'
    };

    $scope.NoneAccountFilter = {
        Id: null,
        AccountTitle: 'All'
    };

    $scope.FilterPrevious = {
        Id: 10,
        Name: 'Previous',
        Text: 'Previous',
        Title: 'Previous',
        FilterTypeId: 6,
        SelectedDate: new Date(),
        Order: 10
    };

    $scope.FilterNext = {
        Id: 11,
        Name: 'Next',
        Text: 'Next',
        Title: 'Next',
        FilterTypeId: 7,
        SelectedDate: new Date(),
        Order: 11
    };

    $scope.TransactionFilters = [
        {
            Id: 1,
            Name: 'Today',
            Text: 'Today',
            Title: 'Today',
            FilterTypeId: 1,
            SelectedDate: new Date(),
            Order: 1
        },
        {
            Id: 2,
            Name: '',
            Text: '',
            Title: '',
            FilterTypeId: 0,
            SelectedDate: new Date(),
            Order: 2
        },
        {
            Id: 3,
            Name: 'This Week',
            Text: 'This Week',
            Title: 'This Week',
            FilterTypeId: 2,
            SelectedDate: new Date(),
            Order: 3
        },
        {
            Id: 4,
            Name: 'This Month',
            Text: 'This Month',
            Title: 'This Month',
            FilterTypeId: 3,
            SelectedDate: new Date(),
            Order: 4
        },
        {
            Id: 5,
            Name: 'This Year',
            Text: 'This Year',
            Title: 'This Year',
            FilterTypeId: 4,
            SelectedDate: new Date(),
            Order: 5
        }
        ,
        {
            Id: 6,
            Name: '',
            Text: '',
            Title: '',
            FilterTypeId: 0,
            SelectedDate: new Date(),
            Order: 6
        },
            $scope.FilterPrevious
        ,
            $scope.FilterNext];

    $scope.TransactionFilter = {
        ProjectId: $stateParams.id,
        FilterType: $scope.TransactionFilters[0], // default by this week
        FilterTypeId: 0,
        Category: $scope.NoneCategoryFilter,
        Account: $scope.NoneAccountFilter,
        IsUpcoming: false,
        IsUnclearOnly: false,
    };

    $scope.Categories = [];
    $scope.Accounts = [];
    $scope.SelectedTransaction = null;
    $scope.PrimaryAccount = null;
    $scope.Transactions = [];
    $scope.Members = null;

    $scope.AuditLogs = [];
    $scope.CommentText = '';
    $scope.UserComments = [];
   
    $rootScope.$on(Constants.Events.ProjectHeaderUpdated, function (event, data) {
        $scope.Project.Currency = data.Project.Currency;
        $rootScope.CurrencyRoot = data.Project.Currency;
    });

    $scope.ShowIncomeExpenseSummary = function()
    {
        $scope.IsShowIncomeExpenseSummary = !$scope.IsShowIncomeExpenseSummary;
    }

    $scope.getProjectFilter = function (projectId) {
        var transactionFilter = {
            ProjectId: projectId
        };

        $http.post(Constants.WebApi.Project.GetProjectFilter, transactionFilter).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            var projectFilter = response.data;


            var date = new Date();
            if (projectFilter.FromDate != null)
            {
                date = new Date(projectFilter.FromDate);
                projectFilter.FromDate = date;
            }
            $scope.updateFilterNextPrevious(projectFilter.FilterTypeId, date);

            $scope.updateProjectFilter(projectFilter);

            $scope.searchTransactions($scope.TransactionFilter);

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.  
            $scope.searchTransactions($scope.TransactionFilter);
        });
    }

    $scope.updateProjectFilter = function (filter) {
        
        var filterType = $scope.TransactionFilters[0];
        if (filter.FilterTypeId != null && filter.FilterTypeId > 0) {
            filterType = $scope.getFilterType(filter.FilterTypeId);            
        }

        $scope.TransactionFilter.FilterType = filterType;
        $scope.TransactionFilter.FilterTypeId = filterType.FilterTypeId;
        $scope.TransactionFilter.FromDate = filter.FromDate;

        if (filter.Category != null) {
            $scope.TransactionFilter.Category =
                {
                    Id: filter.Category.Id,
                    CategoryTitle: null
                };

            var selectedCategory = $scope.getCategory(filter.Category.Id);
            if (selectedCategory != null)
                $scope.TransactionFilter.Category = selectedCategory;
        }
        if (filter.Account != null) {
            $scope.TransactionFilter.Account =
                {
                    Id: filter.Account.Id,
                    CategoryTitle: null
                };
            var selectedAccount = $scope.getAccount(filter.Account.Id);
            if (selectedAccount != null)
                $scope.TransactionFilter.Account = selectedAccount;
        }
    }

    $scope.getAccount = function(accountId)
    {
        var matchedAccount = null;
        if(accountId != null)
        {
            for (var i = 0; i < $scope.Accounts.length; i++) {
                var account = $scope.Accounts[i];
                if (account.Id == accountId) {
                    matchedAccount = account;
                    break;
                }
            }

        }

        return matchedAccount;
    }

    $scope.getCategory = function (categoryId) {
        var matchedCategory = null;
        if (categoryId != null) {
            for (var i = 0; i < $scope.Categories.length; i++) {
                var category = $scope.Categories[i];
                if (category.Id == categoryId) {
                    matchedCategory = category;
                    break;
                }
            }

        }

        return matchedCategory;
    }

    $scope.saveProjectFilter = function (transactionFilter) {

        $http.post(Constants.WebApi.Project.SaveProjectFilter, transactionFilter).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.  
        });
    }

    $scope.searchTransactions = function (transactionFilter) {

        transactionFilter.FilterTypeId = transactionFilter.FilterType.FilterTypeId;

        $http.post(Constants.WebApi.Project.SearchTransactions, transactionFilter).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Transactions = response.data;
            $scope.totalAmount = $scope.calcTotalAmount();

            $scope.saveProjectFilter(transactionFilter);
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: "Load Transaction Failed!",
                type: "warning"
            });
        });
    }

    $scope.calcTotalAmount = function () {        
        $scope.totalIncome = 0;
        $scope.totalExpense = 0;

        for (count = 0; count < $scope.Transactions.length; count++) {
            var amount = Number($scope.Transactions[count].Amount);
            if ($scope.Transactions[count].IsIncome) {                
                $scope.totalIncome += amount;
            }
            else {
                $scope.totalExpense += amount;
            }
        }
        var total = $scope.totalIncome - $scope.totalExpense;
        return total;
    };

    $scope.loadCategories = function () {
        $http.get(Constants.WebApi.Project.GetAvailableCategories, { params: { projectId: $scope.Project.Id } }).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Categories = response.data;

            if ($scope.TransactionFilter.Category.Id != null) {
                var selectedCategory = $scope.getCategory($scope.TransactionFilter.Category.Id);
                if (selectedCategory != null)
                    $scope.TransactionFilter.Category = selectedCategory;
            }

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: "Load Category Failed!",
                type: "warning"
            });
        });
    }

    $scope.loadAccounts = function () {
        $http.get(Constants.WebApi.Project.GetAccounts, { params: { projectId: $scope.Project.Id } }).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Accounts = response.data.Accounts;
            $scope.PrimaryAccount = $scope.getPrimaryAccount($scope.Accounts);

            if ($scope.TransactionFilter.Account.Id != null) {
                var selectedAccount = $scope.getAccount($scope.TransactionFilter.Account.Id);
                if (selectedAccount != null)
                    $scope.TransactionFilter.Account = selectedAccount;
            }

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: "Load Project Account Failed!",
                type: "warning"
            });
        });
    }

    $scope.onload = function ()
    {        
        $scope.loading = true;
        $scope.loadCategories();
        $scope.loadAccounts();
        $scope.getProjectFilter($scope.Project.Id);
        $rootScope.$broadcast(Constants.Events.ProjectChanged, $scope.Project.Id);
        $scope.loading = false;        

        $scope.loadProjectMembers($scope.Project.Id);
    }

    $scope.createNewTransaction = function () {
        var newTrans = {
            ProjectId: $scope.Project.Id,
            AccountId: null,
            Account: $scope.PrimaryAccount,
            CategoryId: null,
            Category: null,
            TransactionTitle: '',
            Amount: 0,
            TransactionDate: new Date(),
            IsIncome: true,
            RecurringTransaction: null,
            RecurringTransactionId: null,
            IsClear: true,
            IsDeleted: false,
            ClientId: null,
            Client: null
        }
        return newTrans;
    }

    $scope.getPrimaryAccount = function (accounts)
    {
        if(accounts != null)
        {
            for(var i = 0; i < accounts.length; i ++)
            {
                var account = accounts[i];
                if (account.IsPrimary) {
                    return account;
                }

            }
            /*angular.forEach(accounts, function(value, key) {
                if (value.IsPrimary) {
                    return value;
                }
            });*/
        }

        return null;
    }

    $scope.loadProjectMembers = function (projectId)
    {
        $http.get(Constants.WebApi.Project.GetProjectMembers, { params: { projectId: projectId } }).then(function (response) {
            $scope.Members = response.data;
        }, function (response) {
            alert('Failed');
        });
    }

    $scope.addNewExpense = function () {
        var newTrans = this.createNewTransaction();
        newTrans.IsIncome = false;
        $scope.Transactions.push(newTrans);
    }

    $scope.addNewIncome = function () {
        var newTrans = this.createNewTransaction();
        newTrans.IsIncome = true;
        $scope.Transactions.push(newTrans);
    }

    $scope.removeCategory = function(trans)
    {
        trans.Category = null;
        $scope.partialSave();
    }

    $scope.removeTransactionDate = function(trans)
    {
        trans.TransactionDate = null;
        $scope.partialSave();
    }

    $scope.selectTransaction = function (TransDto) {

        $scope.SelectedTransaction = TransDto;
        $scope.getCurrentTransactionId();
    }

    $scope.saveTrans = function () {
        $scope.loading = true;

        $http.post(Constants.WebApi.Project.SaveTransactions, $scope.Transactions).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Transactions = response.data;
            $scope.loading = false;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            SweetAlert.swal({
                title: "Error!",
                text: "Load Recent Project Failed!",
                type: "warning"
            });
        });

    }

    $scope.saveTransaction = function (transaction) 
    {
        if (transaction != null) {
            $http.post(Constants.WebApi.Project.SaveTransaction, transaction).then(function (response) {
                // this callback will be called asynchronously
                // when the response is available
                transaction.Id = response.data.Id;
            }, function (response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.                   
                SweetAlert.swal({
                    title: "Error!",
                    text: "Save Failed!",
                    type: "warning"
                });
            });
        }
    }

    $scope.partialSave = function ()
    {        
        $scope.saveTransaction($scope.SelectedTransaction);
    }

    $scope.confirmDeleteTransaction = function (deletingTransaction) {
        SweetAlert.swal({ title: "Delete this transaction?", text: "You will not be able to recover this!", type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!", closeOnConfirm: true },
            function (isConfirm) {
                if (isConfirm) {
                    $scope.deleteTransaction(deletingTransaction);
                }
            });
    }

    $scope.deleteTransaction = function (deletingTransaction)
    {

        $http.post(Constants.WebApi.Project.DeleteTransaction, deletingTransaction).then(function (response) {
            var index = $scope.Transactions.indexOf(deletingTransaction);
            $scope.Transactions.splice(index, 1);
            $scope.SelectedTransaction = null;

        }, function (response) {
            SweetAlert.swal({
                title: "Error!",
                text: "Save Failed!",
                type: "warning"
            });
        });

    }

    $scope.setRecurringTransaction = function(transaction, timeId)
    {
        if (transaction.RecurringTransaction == null) {
            if ($scope.TimeFrequency.Never != timeId) {
                // create new
                transaction.RecurringTransaction = {
                    Id: transaction.RecurringTransactionId,
                    ProjectId: transaction.ProjectId,
                    TimeFrequencyId: timeId
                }
                
                $scope.saveRecurringTransaction(transaction, transaction.RecurringTransaction);
            }
        }
        else {
            if (transaction.RecurringTransaction.TimeFrequencyId != timeId) {
                if ($scope.TimeFrequency.Never == timeId) {
                    $scope.removeRecurringTransaction(transaction, transaction.RecurringTransaction);
                    //transaction.RecurringTransaction = null;
                }
                else {
                    transaction.RecurringTransaction.TimeFrequencyId = timeId;
                    $scope.saveRecurringTransaction(transaction, transaction.RecurringTransaction);
                }
            }
        }
    }

    $scope.saveRecurringTransaction = function (transaction, recurringTransaction)
    {
        recurringTransaction.TransactionDate = transaction.TransactionDate;

        var recurringTransactionSubmitDto =
            {
                TransactionId: transaction.Id,
                RecurringTransaction: recurringTransaction
            };

        $http.post(Constants.WebApi.Project.SaveRecurringTransaction, recurringTransactionSubmitDto).then(function (response) {
            transaction.RecurringTransaction = response.data;
        }, function (response) {
            SweetAlert.swal({
                title: "Error!",
                text: "Save Recurring Transaction Failed!",
                type: "warning"
            });
        });
    }

    $scope.removeRecurringTransaction = function (transaction, recurringTransaction) {

        var recurringTransactionSubmitDto =
        {
            TransactionId: transaction.Id,
            RecurringTransaction: recurringTransaction
        };

        $http.post(Constants.WebApi.Project.RemoveRecurringTransaction, recurringTransactionSubmitDto).then(function (response) {
            transaction.RecurringTransaction = null;
            transaction.RecurringTransactionId = null;
        }, function (response) {
            SweetAlert.swal({
                title: "Error!",
                text: "Save Recurring Transaction Failed!",
                type: "warning"
            });
        });
    }

    $scope.getCurrentTransactionId = function()
    {
        var transactionId = null;
        if (!(angular.isUndefined($scope.SelectedTransaction) || $scope.SelectedTransaction === null)) {
            if (!(angular.isUndefined($scope.SelectedTransaction.Id) || $scope.SelectedTransaction.Id === null)) {
                transactionId = $scope.SelectedTransaction.Id;
            }
        }
        $localStorage.transactionId = transactionId;
        $localStorage.selectedTransaction = $scope.SelectedTransaction;
        return transactionId;
    }

    $scope.loadAuditLogs = function(transId)
    {
        if (!(angular.isUndefined(transId) || transId === null)) {
            $http.get(Constants.WebApi.AuditLog.GetTransactionAuditLogs, { params: { TransactionId: transId } }).then(function (response) {
                // this callback will be called asynchronously
                // when the response is available
                $scope.AuditLogs = response.data;
            }, function (response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.                   
                SweetAlert.swal({
                    title: "Error!",
                    text: "Load project activity failed!",
                    type: "warning"
                });
            });
        }
    }

    $scope.loadUserComments = function (transId) {
        if (!(angular.isUndefined(transId) || transId === null)) {
            $http.get(Constants.WebApi.UserComment.GetTransactionUserComments, { params: { TransactionId: transId } }).then(function (response) {
                // this callback will be called asynchronously
                // when the response is available
                $scope.UserComments = response.data;
            }, function (response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.                   
                SweetAlert.swal({
                    title: "Error!",
                    text: "Load Recent Project Failed!",
                    type: "warning"
                });
            });
        }
    }

    $scope.addComment = function()
    {
        var transactionId = this.getCurrentTransactionId();
        if(transactionId != null)
        {
            var comment = {
                ObjectId: transactionId,
                CommentText: $scope.CommentText
            }
            $http.post(Constants.WebApi.UserComment.CreateTransactionUserComment, comment).then(function (response) {
                // this callback will be called asynchronously
                // when the response is available
                var savedComment = response.data;
                savedComment.User = {
                    Id: savedComment.UserId,
                    Photo: null
                };
                $scope.UserComments.push(savedComment);

                $scope.CommentText = '';
            }, function (response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.                   
                SweetAlert.swal({
                    title: "Error!",
                    text: "Load Recent Project Failed!",
                    type: "warning"
                });
            });

        }
    }
    
    $scope.changeAccount = function(account)
    {
        if ($scope.SelectedTransaction != null) {
            $scope.SelectedTransaction.Account = account;
            $scope.partialSave();
        }
    }

    $scope.changeCategory = function (category) {
        if ($scope.SelectedTransaction != null) {
            $scope.SelectedTransaction.Category = category;
            $scope.partialSave();
        }
    }

    $scope.assignMember = function (member) {
        if ($scope.SelectedTransaction != null) {
            if (member != null) {
                $scope.SelectedTransaction.Client = member.Client;
            }
            else {
                $scope.SelectedTransaction.Client.User.Photo = null;
                $scope.SelectedTransaction.Client = null;
            }
            $scope.partialSave();
        }
    }

    $scope.setClear = function(transaction)
    {
        transaction.IsClear = !transaction.IsClear;
    }

    $scope.openProjectSetting = function (projId) {
        var modalInstance = $modal.open({
            templateUrl: 'views/modals/ProjectSetting.html',
            controller: ProjectSettingCtrl,
            resolve: {
                ProjectId: function () {
                    return projId;
                }
            }
        });

        modalInstance.result.then(function () {
            //on ok button press 
            $scope.loadProjectHeader();
            $rootScope.$broadcast('projectListChanged', { ProjectId: $scope.Project.Id });
        }, function () {
            //on cancel button press
        });
    }

    $scope.$watch('SelectedTransaction', function (newVal, oldVal) {
        if (newVal != null && newVal.Id != null) {
            var transId = newVal.Id;
            $scope.loadAuditLogs(transId);
            $scope.loadUserComments(transId);

            var client = newVal.Client;
            if (client == null || client.User == null || client.User.Photo == null) {
                if (client == null || client.User == null)
                {
                    newVal.Client = {
                        User: {
                            Id: null,
                            Photo: null
                        }
                    };
                }

                newVal.Client.User.Photo = '//:0';
            }

            console.debug('SelectedTransaction changed');
        }
    }, false);
    
    $scope.$watch(function ($scope) {
        return $scope.Transactions.
            map(function (obj) {
                return obj.Amount
            });
    }, function (newVal) {
        $scope.totalAmount = $scope.calcTotalAmount();
        console.debug('Changed amount: {0}', newVal);
    }, true);

    $scope.$watch('Members', function (newVal, oldVal) {
        if (newVal != null) {
            for (var i = 0; i < newVal.length; i++)
            {
                var client = newVal[i].Client;
                if (client.User.Id != null && (client.User.Photo == null || client.User.Photo.length < 8)) {
                    var photoResult = userPhotoService.getUserPhoto(client.User);
                    photoResult.then(function (data) {
                    }, function (error) { });
                }
            }
        }
    }, true);

    $scope.$watch('Transactions', function (newVal, oldVal) {
        if (newVal != null) {
            for (var i = 0; i < newVal.length; i++) {
                var client = newVal[i].Client;
                if (client != null && client.User != null && client.User.Id != null && (client.User.Photo == null || client.User.Photo.length < 8)) {
                    var photoResult = userPhotoService.getUserPhoto(client.User);
                    photoResult.then(function (data) {
                    }, function (error) { });
                }
            }
        }
    }, true);
    
    $scope.$watch('UserComments', function (newVal, oldVal) {
        if (newVal != null) {
            for (var i = 0; i < newVal.length; i++) {
                var user = newVal[i].User;
                if (user != null && user.Id != null && (user.Photo == null || user.Photo.length < 8)) {
                    var photoResult = userPhotoService.getUserPhoto(user);
                    photoResult.then(function (data) {
                    }, function (error) { });
                }
            }
        }
    }, true);

    $scope.getFilterType = function(FilterTypeId)
    {
        var filterType = $scope.TransactionFilters[0];

        for (var i = 0; i < $scope.TransactionFilters.length; i++) {
            var filter = $scope.TransactionFilters[i];
            if (filter.FilterTypeId == FilterTypeId) {
                filterType = filter;
                break;
            }
        }

        return filterType;
    }

    $scope.filter = function(filter)
    {
        //var filter = $scope.getFilterType(FilterTypeId);

        $scope.TransactionFilter.FilterType = filter;
        $scope.TransactionFilter.FilterTypeId = filter.FilterTypeId;
        $scope.TransactionFilter.FromDate = filter.SelectedDate;

        $scope.searchTransactions($scope.TransactionFilter);

        $scope.updateFilterNextPrevious(filter.FilterTypeId, filter.SelectedDate);
    }
    
    $scope.filterUpcoming = function(isUpcoming)
    {
        $scope.TransactionFilter.IsUpcoming = isUpcoming;
    }

    $scope.filterAccount = function (account) {
        $scope.TransactionFilter.Account = account;
        $scope.searchTransactions($scope.TransactionFilter);
    }

    $scope.filterCategory = function (category) {
        $scope.TransactionFilter.Category = category;
        $scope.searchTransactions($scope.TransactionFilter);
    }

    $scope.updateFilterNextPrevious = function(filterType, date)
    {
        //console.debug('Filter:', filterType, ', date:', date);

        switch (filterType) {
            case 1: //Today
                {
                    $scope.FilterPrevious.Text = 'Yesterday';
                    $scope.FilterPrevious.Title = 'Yesterday';
                    $scope.FilterPrevious.FilterTypeId = 5;
                    $scope.FilterPrevious.SelectedDate.setDate(date.getDate() - 1);

                    $scope.FilterNext.Text = 'Tomorrow';
                    $scope.FilterNext.Title = 'Tomorrow';
                    $scope.FilterNext.FilterTypeId = 5;
                    $scope.FilterNext.SelectedDate.setDate(date.getDate() + 1);
                }
                break;
            case 2: //This Week
                {
                    $scope.FilterPrevious.Text = 'Last Week';
                    $scope.FilterPrevious.Title = 'Last Week';
                    $scope.FilterPrevious.FilterTypeId = 6;
                    var lastMonday = Commons.GetPreviousMonday(date);
                    $scope.FilterPrevious.SelectedDate = lastMonday;

                    $scope.FilterNext.Text = 'Next Week';
                    $scope.FilterNext.Title = 'Next Week';
                    $scope.FilterNext.FilterTypeId = 6;
                    var nextMonday = Commons.GetNextMonday(date);
                    $scope.FilterNext.SelectedDate = nextMonday;
                }
                break;
            case 3: //This Month
                {
                    $scope.FilterPrevious.Text = 'Last Month';
                    $scope.FilterPrevious.Title = 'Last Month';
                    $scope.FilterPrevious.FilterTypeId = 7;
                    var preMonth = Commons.GetPreviousMonth(date);
                    $scope.FilterPrevious.SelectedDate = preMonth;

                    $scope.FilterNext.Text = 'Next Month';
                    $scope.FilterNext.Title = 'Next Month';
                    $scope.FilterNext.FilterTypeId = 7;
                    var nextMonth = Commons.GetNextMonth(date);
                    $scope.FilterNext.SelectedDate = nextMonth;
                }
                break;
            case 4: //This Year
                {
                    $scope.FilterPrevious.Text = 'Last Year';
                    $scope.FilterPrevious.Title = 'Last Year';
                    $scope.FilterPrevious.FilterTypeId = 8;
                    var preYear = Commons.GetPreviousYear(date);
                    $scope.FilterPrevious.SelectedDate = preYear;

                    $scope.FilterNext.Text = 'Next Year';
                    $scope.FilterNext.Title = 'Next Year';
                    $scope.FilterNext.FilterTypeId = 8;
                    var nextYear = Commons.GetNextYear(date);
                    $scope.FilterNext.SelectedDate = nextYear;
                }
                break;
            case 5: //Date
                {
                    $scope.FilterPrevious.Title = $scope.FilterPrevious.Text;
                    $scope.FilterPrevious.SelectedDate.setDate(date.getDate() - 1);
                    $scope.FilterPrevious.Text = Commons.DateToString($scope.FilterPrevious.SelectedDate);

                    $scope.FilterNext.Title = $scope.FilterNext.Text;
                    $scope.FilterNext.SelectedDate.setDate(date.getDate() + 1);
                    $scope.FilterNext.Text = Commons.DateToString($scope.FilterNext.SelectedDate);
                }
                break;
            case 6: //Week
                {
                    $scope.FilterPrevious.Title = $scope.FilterPrevious.Text;
                    var lastMonday = Commons.GetPreviousMonday(date);
                    $scope.FilterPrevious.SelectedDate = lastMonday;
                    $scope.FilterPrevious.Text = 'Week from ' + Commons.DateToString($scope.FilterPrevious.SelectedDate);

                    $scope.FilterNext.Title = $scope.FilterNext.Text;
                    var nextMonday = Commons.GetNextMonday(date);
                    $scope.FilterNext.SelectedDate = nextMonday;
                    $scope.FilterNext.Text = 'Week from ' + Commons.DateToString($scope.FilterNext.SelectedDate);
                }
                break;
            case 7: //Month
                {
                    $scope.FilterPrevious.Title = $scope.FilterPrevious.Text;
                    var preMonth = Commons.GetPreviousMonth(date);
                    $scope.FilterPrevious.SelectedDate = preMonth;
                    $scope.FilterPrevious.Text = Commons.DateToMonthString($scope.FilterPrevious.SelectedDate);

                    $scope.FilterNext.Title = $scope.FilterNext.Text;
                    var nextMonth = Commons.GetNextMonth(date);
                    $scope.FilterNext.SelectedDate = nextMonth;
                    $scope.FilterNext.Text = Commons.DateToMonthString($scope.FilterNext.SelectedDate);
                }
                break;
            case 8: //Year
                {
                    $scope.FilterPrevious.Title = $scope.FilterPrevious.Text;
                    var preYear = Commons.GetPreviousYear(date);
                    $scope.FilterPrevious.SelectedDate = preYear;
                    $scope.FilterPrevious.Text = Commons.DateToYearString($scope.FilterPrevious.SelectedDate);


                    $scope.FilterNext.Title = $scope.FilterNext.Text;
                    var nextYear = Commons.GetNextYear(date);
                    $scope.FilterNext.SelectedDate = nextYear;
                    $scope.FilterNext.Text = Commons.DateToYearString($scope.FilterNext.SelectedDate);

                }
                break;
        };
    }
};