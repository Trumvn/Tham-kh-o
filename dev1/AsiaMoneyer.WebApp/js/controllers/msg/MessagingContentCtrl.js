function MessagingContentCtrl($scope, $http, $location, SweetAlert, Constants) {

    $scope.Templates = {};

    $scope.sendTestEmail = function () {

        $http.post(Constants.WebApi.Messaging.SendTestEmail, null).then(function (response) {
            $scope.loading = false;
            SweetAlert.swal({
                title: "Send Test Email",
                text: "Completed!",
                type: "success"
            });
        }, function (response) {
            $scope.loading = false;
            SweetAlert.swal({
                title: "Error!",
                text: "Register new user Failed!",
                type: "warning"
            });
        });

    }

    $scope.getMessagingContent = function () {

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

    $scope.onload = function ()
    {
        $scope.getMessagingContent();
    }
}