function ProjectAccountCtrl($rootScope,$scope, $http, $modal, $stateParams, $localStorage, SweetAlert, userPhotoService, Constants) {
    $scope.projectId = $stateParams.id;

    $scope.SelectedAccount = null;
    $scope.Accounts = [];
    $scope.Project = {
        Id: $stateParams.id,
        Title: '',
        Currency: $rootScope.CurrencyRoot
    };

    $scope.loadAccounts = function () {
        $scope.loading = true;

        $http.get(Constants.WebApi.Project.GetAccounts, { params: { projectId: $scope.projectId } }).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Accounts = response.data.Accounts;

            $scope.loadAccountsDetail();
            $scope.loading = false;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            SweetAlert.swal({
                title: "Error!",
                text: "Load Account Failed!",
                type: "warning"
            });
        });

    }

    $scope.loadAccountsDetail = function () {
        $scope.loading = true;

        $http.get(Constants.WebApi.Project.GetAccountCurrentBalance, { params: { projectId: $scope.projectId } }).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Accounts = response.data.Accounts;
            $scope.loading = false;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            SweetAlert.swal({
                title: "Error!",
                text: "Load Account Failed!",
                type: "warning"
            });
        });

    }

    $scope.onload = function () {
        $scope.loadAccounts();        
    }

    $scope.addNewAccount = function () {
        var newAccount = {
            ProjectId: $scope.projectId,
            AccountTitle: '',
            AccountDescription: '',
            CurrentBalance: 0,
            HighlightColor: null,
            OpenDate: null,
            IsClosed: false,
            IsDelete: false,
            IsPrimary: false,
            Stage: 1 // add new
        }
        $scope.Accounts.push(newAccount);
    }

    $scope.selectAccount = function (AccountDto) {

        $scope.SelectedAccount = AccountDto;
        
        if (AccountDto.Stage != 1) {
            AccountDto.Stage = 2; // modified
        }
    }

    $scope.saveAccount = function (account) {
        $scope.loading = true;

        $http.post(Constants.WebApi.Project.SaveAccount, account).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            account.Id = response.data.Id;
            $scope.loading = false;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Save account failed!')
        });
    }

    $scope.partialSave = function ()
    {
        $scope.saveAccount($scope.SelectedAccount);
    }

    $scope.closeAccount = function (account, isClosed) {
        account.IsClosed = isClosed;
        $http.post(Constants.WebApi.Project.CloseAccount, account).then(function (response) {
            account.IsClosed = isClosed;
        }, function (response) {
            account.IsClosed = !isClosed;
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Save account failed!')
        });
    }

    $scope.setPrimary = function (account) {
        $http.post(Constants.WebApi.Project.SetAccountPrimary, account).then(function (response) {
            angular.forEach($scope.Accounts, function (value, key) {
                value.IsPrimary = false;
            });
            account.IsPrimary = true;
        }, function (response) {
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Save account failed!')
        });
    }

    $scope.setHighlightColor = function(account, color)
    {
        if (account != null)
        {
            account.HighlightColor = color;
            $scope.partialSave();
        }
    }

    $scope.confirmDelete = function (account) {
        SweetAlert.swal({ title: "Delete this account?", text: "You will not be able to recover this!", type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!", closeOnConfirm: true },
            function (isConfirm) {
                if (isConfirm) {
                    $scope.deleteAccount(account);
                }
            });
    }

    $scope.deleteAccount = function (account) {
        $http.post(Constants.WebApi.Project.DeleteAccount, account).then(function (response) {
            var index = $scope.Accounts.indexOf(account);
            $scope.Accounts.splice(index, 1);
            $scope.SelectedAccount = null;
        }, function (response) {
            SweetAlert.swal({
                title: "Error!",
                text: "Delete account error!",
                type: "warning"
            });
        });
    }

    $scope.AuditLogs = [];
    $scope.CommentText = '';
    $scope.UserComments = [];

    $scope.loadAuditLogs = function (projectAccountId) {
        if (!(angular.isUndefined(projectAccountId) || projectAccountId === null)) {
            $http.get(Constants.WebApi.AuditLog.GetProjectAccountAuditLogs, { params: { AccountId: projectAccountId } }).then(function (response) {
                // this callback will be called asynchronously
                // when the response is available
                $scope.AuditLogs = response.data;
            }, function (response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                toastr.options.closeButton = true;
                toastr.error('Network Error', 'Could not load audit logs!');
            });
        }
    }

    $scope.loadUserComments = function (accountId) {
        if (!(angular.isUndefined(accountId) || accountId === null)) {
            $http.get(Constants.WebApi.UserComment.GetProjectAccountUserComments, { params: { AccountId: accountId } }).then(function (response) {
                // this callback will be called asynchronously
                // when the response is available
                $scope.UserComments = response.data;

                /*angular.forEach($scope.UserComments, function (value, key) {
                    if (value.User != null && value.User.Id != null)
                        value.User.Photo = $scope.getUserPhoto(value.User.Id);
                });*/
            }, function (response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.                   
                toastr.options.closeButton = true;
                toastr.error('Network Error', 'Could not user comments!')
            });
        }
    }

    $scope.addComment = function () {
        var accountId = $scope.SelectedAccount.Id;
        //var transactionId = this.getCurrentTransactionId();
        if (accountId != null) {
            var comment = {
                ObjectId: accountId,
                CommentText: $scope.CommentText
            }
            $http.post(Constants.WebApi.UserComment.CreateProjectAccountUserComment, comment).then(function (response) {
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
                    text: "Add comment failed!",
                    type: "warning"
                });
            });
            $scope.loadUserComments();
        }
    }

    $scope.$watch('SelectedAccount', function (newVal, oldVal) {
        if (newVal != null && newVal.Id != null) {
            var projectAccountId = newVal.Id;
            $scope.loadAuditLogs(projectAccountId);
            $scope.loadUserComments(projectAccountId);
        }

    }, false);

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
};