function AccountYourAccountCtrl($scope, $http, $state, $stateParams, $location, SweetAlert, Constants) {

    $scope.CurrentSubscription = {};

    $scope.onload = function () {
        $scope.loadUserCurrentSubscription();
    }

    $scope.goUpgradeAccount = function()
    {
        $state.go('app.account.upgrade');
    }

    $scope.loadUserCurrentSubscription = function () {
        $scope.loading = true;

        $http.post(Constants.WebApi.Client.GetUserCurrentSubscription, null).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.CurrentSubscription = response.data;
            $scope.loading = false;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Save account failed!')
        });
    }

    $scope.goPayment = function (invoiceId) {
        $state.go('app.account.payment', { 'id': invoiceId });
    }
}