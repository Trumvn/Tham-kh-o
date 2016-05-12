function MessagingMailInboxCtrl($scope, $http, $location, SweetAlert, Constants) {

    $scope.Total = 0;
    $scope.Messages = {};

    $scope.loadInbox = function () {
        $http.get(Constants.WebApi.Messaging.GetMessages).then(function (response) {

            $scope.Messages = response.data.Messages;
            $scope.Total = response.data.Total;

        }, function (response) {
            SweetAlert.swal({
                title: "Error!",
                text: "Load Inbox Failed!",
                type: "warning"
            });
        });

    }

    $scope.onload = function () {
        $scope.loadInbox();
    }

}