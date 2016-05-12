function ProjectTransferMoneyCtrl($scope, $http, $stateParams, $modalInstance, SweetAlert, Constants) {

    $scope.ProjectId = $stateParams.id;

    $scope.TransferMoneyDto = {
        ProjectId: $scope.ProjectId,
        AccountFrom: {Id: null, AccountTitle: null },
        AccountTo: {Id: null, AccountTitle: null },
        Amount: 0,
        Fee:0,
        Currency: 'd',
        TransferDate: new Date(),
        IsClear: true
    };

    $scope.Accounts = [];

    $scope.loadAvailableAccounts = function()
    {
        $http.get(Constants.WebApi.Project.GetAccounts, { params: { projectId: $scope.ProjectId } }).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Accounts = response.data.Accounts;

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: response.data,
                type: "warning"
            });
        });
    }

    $scope.transferMoney = function () {
        $http.post(Constants.WebApi.Project.TransferMoney, $scope.TransferMoneyDto).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.ok();

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: response.data,
                type: "warning"
            });
        });
    }

    $scope.onload = function()
    {
        $scope.loadAvailableAccounts();
    }

    $scope.ok = function () {
        $modalInstance.close();        
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}