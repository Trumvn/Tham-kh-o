function LoadingCtrl($scope, $http, $location, SweetAlert, Constants) {

    $scope.verifyAuthentication = function () {
        $scope.loading = true;

        $http.get(Constants.WebApi.Accounts.IsAuthenticated).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.loading = false;
            $location.path('/app/dashboard');
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
        });
    }
}