function AccountContentCtrl($scope, $http, $location, SweetAlert, Constants) {

    $scope.loadAccount = function () {

        $http.get(Constants.WebApi.Messaging.GetMessagingContent).then(function (response) {

            $scope.Templates = response.data.MessagingTemplates;

        }, function (response) {
            SweetAlert.swal({
                title: "Error!",
                text: "Load Messaging Content Failed!",
                type: "warning"
            });
        });

    }

    $scope.onload = function () {
        //$scope.loadAccount();
    }
}