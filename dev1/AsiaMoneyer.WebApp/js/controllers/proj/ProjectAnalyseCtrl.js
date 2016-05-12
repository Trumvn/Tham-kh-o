function ProjectAnalyseCtrl($scope, $http, $localStorage, $modal, $stateParams, SweetAlert, userPhotoService, Constants) {
    $scope.projectId = $stateParams.id;

    $scope.CashFlowChartTypeReport = true;

    var today = new Date();

    $scope.CategoryFilter = {
        Account: {
            Id: null,
        },
        FilterDate: new Date(),
        FilterMonth: today.getMonth(),
        FilterYear: today.getFullYear(),
    };

    $scope.AccountFilter =
        {
            FilterDate: new Date(),
            FilterMonth: today.getMonth(),
            FilterYear: today.getFullYear(),
        };

    $scope.SelectedCategory = null;
    $scope.Categories = [];
    $scope.Accounts = [];

    $scope.AnalyseData = {};

    $scope.onload = function () {
        $scope.LoadProjectAnalyseInformation($scope.projectId);
        $scope.LoadTransactionSummaryByMonthYear($scope.projectId);
        $scope.loadProjectCategories($scope.projectId);
        $scope.loadProjectAccounts($scope.projectId);
        $scope.loadJarsAnalyseData($scope.projectId);
    }

    $scope.chartByAccountObject = {};
    $scope.chartByAccountObject.type = "PieChart";
    $scope.chartByAccountObject.options = {
    }

    $scope.chartByAccountObject.data = {
        "cols": [
            { id: "t", label: "Day", type: "string" },
            { id: "s", label: "Income", type: "number" }],
        "rows": []
    };

    $scope.chartByCategoryIncomeObject = {};
    $scope.chartByCategoryIncomeObject.type = "PieChart";
    $scope.chartByCategoryIncomeObject.options = {
        title: 'Income',        
    };

    $scope.chartByCategoryIncomeObject.data = {
        "cols": [
            { id: "t", label: "Day", type: "string" },
            { id: "s", label: "Balance", type: "number" }],
        "rows": []
    };

    $scope.chartByCategoryExpenseObject = {};
    $scope.chartByCategoryExpenseObject.type = "PieChart";
    $scope.chartByCategoryExpenseObject.options = {
        title: 'Expense',
    };

    $scope.chartByCategoryExpenseObject.data = {
        "cols": [
            { id: "t", label: "Day", type: "string" },
            { id: "s", label: "Balance", type: "number" }],
        "rows": []
    };

    $scope.chartObject = {};

    //$scope.chartObject.type = "LineChart";
    $scope.chartObject.type = "google.charts.Bar";
    $scope.chartObject.options = {
        title: 'Cashflow & Forecast',
        colors: ['#388E3C', '#FF9800', '#5D4037'],
        // Gives each series an axis that matches the vAxes number below.
        series: {
            0: { targetAxisIndex: 0 }
        },
        hAxis: {
            ticks: [],
            format: 'MM/yyy'
        }
    };

    $scope.chartObject.data = {
        "cols": [
            { id: "1", label: "", type: "string" },
            { id: "2", label: "Income", type: "number" },
            { id: "3", label: "Expense", type: "number" },
            { id: "4", label: "Balance", type: "number" }],
        "rows": []
    };

    $scope.chartCategoryBudget = {};

    //$scope.chartCategoryBudget.type = "LineChart";
    $scope.chartCategoryBudget.type = "google.charts.Bar";
    $scope.chartCategoryBudget.options = {
        title: 'Category Budget',
        colors: ['#607D8B', '#BDBDBD'],
        // Gives each series an axis that matches the vAxes number below.
        series: {
            0: { targetAxisIndex: 0 }
        },
        hAxis: {
            ticks: [],
            format: 'MM/yyy'
        }
    };

    $scope.chartCategoryBudget.data = {
        "cols": [
            { id: "1", label: "", type: "string" },
            { id: "2", label: "Balance", type: "number" },
            { id: "4", label: "Budget", type: "number" }],
        "rows": []
    };

    $scope.TotalMonthlyIncome = 0;
    $scope.SelectedMonthYearForJARSAnalyse = new Date();

    $scope.chartJarsAnalyseObject = {};
    $scope.chartJarsAnalyseObject.type = "google.charts.Bar";
    $scope.chartJarsAnalyseObject.options = {
        
    };

    $scope.chartJarsAnalyseObject.data = {
        "cols": [
            { id: "t", label: "Day", type: "string" },
            { id: "s", label: "Balance", type: "number" }],
        "rows": []
    };

    $scope.changeCashFlowChartType = function(isReportType)
    {
        $scope.CashFlowChartTypeReport = isReportType;
        $scope.LoadTransactionSummaryByMonthYear($scope.projectId);
    }

    $scope.loadProjectCategories = function(projectId)
    {
        $http.get(Constants.WebApi.Project.GetAvailableCategories, { params: { projectId: projectId } }).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Categories = response.data;

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

    $scope.loadProjectAccounts = function (projectId) {
        $http.get(Constants.WebApi.Project.GetAccounts, { params: { projectId: projectId } }).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Accounts = response.data.Accounts;
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

    $scope.LoadProjectAnalyseInformation = function (projectId) {
        var transactionFilter = {
            ProjectId: projectId
        };

        $http.post(Constants.WebApi.Project.LoadProjectAnalyseInformation, transactionFilter).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            var analysingData = response.data;

            $scope.AnalyseData = analysingData;

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: "Load Analyse Information Failed!",
                type: "warning"
            });
        });
    }

    $scope.LoadTransactionSummaryByMonthYear = function (projectId)
    {
        var transactionFilter = {
            ProjectId: projectId,
            FilterId: 1,
            IsIncome: true,
            IsUpcoming: true,
            IsUnclearOnly: ($scope.CashFlowChartTypeReport? false: true),
            Category: null,
            Account: null,
            FromDate: new Date(2016, 1),
            EndDate: new Date(2017, 0),
        };

        $http.post(Constants.WebApi.Project.LoadTransactionSummaryByMonthYear, transactionFilter).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            var transSums = response.data;
            $scope.chartObject.data.rows = [];
            //$scope.chartObject.options.hAxis.ticks = [];

            for(var i = 0;i < transSums.length; i ++)
            {
                var hAxisCol = transSums[i].Title;
                //$scope.chartObject.options.hAxis.ticks.push(hAxisCol);

                var val = [
                    { v: hAxisCol },
                    { v: transSums[i].Income },
                    { v: transSums[i].Expense },
                    { v: transSums[i].Balance },
                ];

                var c = {
                    c: val
                };

                $scope.chartObject.data.rows.push(c);
            }
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: "Load Summary Failed!",
                type: "warning"
            });
        });
    }

    $scope.LoadTransactionSummaryByCategory = function (projectId) {
        var fromDate = today;
        fromDate.setYear($scope.CategoryFilter.FilterYear);
        var endDate = fromDate;

        if ($scope.CategoryFilter.FilterMonth > 0)
        {
            fromDate.setFullYear($scope.CategoryFilter.FilterYear, $scope.CategoryFilter.FilterMonth, 1); // first day of month
            endDate = new Date();
            endDate.setFullYear(fromDate.getFullYear(), fromDate.getMonth() + 1, 0); // last day of month
        }
        

        var transactionFilter = {
            ProjectId: projectId,
            Account: {
                Id: $scope.CategoryFilter.Account.Id,
            },
            FromDate: fromDate,
            EndDate: endDate,
        };

        $http.post(Constants.WebApi.Project.LoadTransactionSummaryByCategory, transactionFilter).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            var transSums = response.data;
            $scope.chartByCategoryExpenseObject.data.rows = [];
            $scope.chartByCategoryIncomeObject.data.rows = [];

            for (var i = 0; i < transSums.length; i++) {
                var val = [
                    { v: transSums[i].Title },
                    { v: transSums[i].Balance },
                ];

                var c = {
                    c: val
                };

                if (transSums[i].Income != 0) {
                    $scope.chartByCategoryIncomeObject.data.rows.push(c);
                }
                else {
                    $scope.chartByCategoryExpenseObject.data.rows.push(c);
                }
            }
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: "Load Summary Failed!",
                type: "warning"
            });
        });
    };

    $scope.LoadTransactionSummaryByAccount = function (projectId) {
        var fromDate = today;
        fromDate.setYear($scope.AccountFilter.FilterYear);
        var endDate = fromDate;

        if ($scope.AccountFilter.FilterMonth > 0) {
            fromDate.setFullYear($scope.AccountFilter.FilterYear, $scope.AccountFilter.FilterMonth, 1); // first day of month
            endDate = new Date();
            endDate.setFullYear(fromDate.getFullYear(), fromDate.getMonth() + 1, 0); // last day of month
        }


        var transactionFilter = {
            ProjectId: projectId,
            FilterId: 1,
            IsIncome: true,
            IsUpcoming: true,
            IsUnclearOnly: false,
            Category: null,
            Account: null,
            FromDate: fromDate,
            EndDate: endDate,
        };

        $http.post(Constants.WebApi.Project.LoadTransactionSummaryByAccount, transactionFilter).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            var transSums = response.data;
            $scope.chartByAccountObject.data.rows = [];

            for (var i = 0; i < transSums.length; i++) {
                var val = [
                    { v: transSums[i].Title },
                    { v: transSums[i].Balance },
                ];

                var c = {
                    c: val
                };

                $scope.chartByAccountObject.data.rows.push(c);
            }
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: "Load Summary Failed!",
                type: "warning"
            });
        });
    };

    $scope.LoadTransactionCategorySummaryByMonthYear = function (projectId, categoryId) {
        var transactionFilter = {
            ProjectId: projectId,
            FilterId: 1,
            IsUpcoming: true,
            IsUnclearOnly: false,
            Category: {
                Id: categoryId
            },
            Account: null,
            FromDate: new Date(2016, 1),
            EndDate: new Date(2017, 0),
        };

        $http.post(Constants.WebApi.Project.LoadTransactionCategorySummaryByMonthYear, transactionFilter).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            var transSums = response.data;
            $scope.chartCategoryBudget.data.rows = [];

            for (var i = 0; i < transSums.length; i++) {
                var hAxisCol = transSums[i].Title;

                var val = [
                    { v: hAxisCol },
                    { v: transSums[i].Balance },
                    { v: transSums[i].Budget },
                ];

                var c = {
                    c: val
                };

                $scope.chartCategoryBudget.data.rows.push(c);
            }
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: "Load Summary Failed!",
                type: "warning"
            });
        });
    }

    $scope.loadJarsAnalyseData = function(projectId)
    {
        var transactionFilter = {
            ProjectId: projectId,
            IsIncome: true,
            FromDate: $scope.SelectedMonthYearForJARSAnalyse,
            EndDate: $scope.SelectedMonthYearForJARSAnalyse,
        };

        $http.post(Constants.WebApi.Project.LoadMonthTotalIncome, transactionFilter).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.TotalMonthlyIncome = response.data;
            $scope.loadJarsAnalyseChart($scope.TotalMonthlyIncome);

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: "Load Monthly Income For JARS Failed!",
                type: "warning"
            });
        });        
    }

    $scope.loadJarsAnalyseChart = function(totalIncome)
    {
        $scope.chartJarsAnalyseObject.data.rows = [];

        var transSums = [];

        var NEC = [
            { v: 'NEC' },
            { v: 0.55 }
        ];

        var LTSS = [
            { v: 'LTSS' },
            { v: 0.1 }
        ];

        var EDU = [
            { v: 'EDU' },
            { v: 0.1 }
        ];

        var FFA = [
            { v: 'FFA' },
            { v: 0.1 }
        ];

        var PLAY = [
            { v: 'PLAY' },
            { v: 0.1 }
        ];

        var GIVE = [
            { v: 'GIVE' },
            { v: 0.05 }
        ];

        transSums.push(NEC);
        transSums.push(LTSS);
        transSums.push(EDU);
        transSums.push(FFA);
        transSums.push(PLAY);
        transSums.push(GIVE);

        for (var i = 0; i < transSums.length; i++) {
            var amount = transSums[i][1].v * totalIncome;
            var val = [
                { v: transSums[i][0].v },
                { v: amount },
            ];

            var c = {
                c: val
            };

            $scope.chartJarsAnalyseObject.data.rows.push(c);
        }
    }

    $scope.setCategoryFilterAccount = function(accountId)
    {
        $scope.CategoryFilter.Account = {
            Id: accountId
        };
        $scope.LoadTransactionSummaryByCategory($scope.projectId);
    }

    $scope.setCategoryFilterDate = function (month, year) {
        $scope.CategoryFilter.FilterMonth = month;
        $scope.CategoryFilter.FilterYear = year;
        $scope.LoadTransactionSummaryByCategory($scope.projectId);
    }


    $scope.setAccountFilterDate = function (month, year) {
        $scope.AccountFilter.FilterMonth = month;
        $scope.AccountFilter.FilterYear = year;
        $scope.LoadTransactionSummaryByAccount($scope.projectId);
    }

    $scope.setJARSAnalyseDate = function(month, year)
    {
        $scope.loadJarsAnalyseData($scope.projectId);
    }

    $scope.$watch('CategoryFilter', function (newVal, oldVal) {
        if (newVal != null && newVal.FilterDate != null) {
            $scope.setCategoryFilterDate(newVal.FilterDate.getMonth(), newVal.FilterDate.getFullYear());
        }
    }, true);

    $scope.$watch('AccountFilter', function (newVal, oldVal) {
        if (newVal != null && newVal.FilterDate != null) {
            $scope.setAccountFilterDate(newVal.FilterDate.getMonth(), newVal.FilterDate.getFullYear());
        }
    }, true);

    $scope.$watch('SelectedMonthYearForJARSAnalyse', function (newVal, oldVal) {
        if (newVal != null) {
            $scope.setJARSAnalyseDate(newVal.getMonth(), newVal.getFullYear());
        }
    }, true);

    $scope.setSelectedCategory = function(cat)
    {
        $scope.SelectedCategory = cat;
        $scope.LoadTransactionCategorySummaryByMonthYear($scope.projectId, cat.Id);
    }
};

// color style https://www.google.com/design/spec/style/color.html#color-color-palette