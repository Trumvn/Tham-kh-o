function AdminClientsCtrl($scope, $http, $modal, $stateParams, Constants, SweetAlert) {

    $scope.Profiles = [];

    $scope.loadUserProfiles = function () {
        $http.get(Constants.WebApi.Client.GetUserProfiles, {}).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Profiles = response.data.UserProfiles;

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

    $scope.onload = function () {
        $scope.loadUserProfiles();
    }    

    $scope.open = function(profile)
    {
        var modalInstance = $modal.open({
            templateUrl: 'views/admin/client_profile.html',
            controller: ClientProfileCtrl,
            resolve: {
                ProfileId: function () {
                    return profile.Id;
                }
            }
        });

        modalInstance.result.then(function () {
            //on ok button press             
        }, function () {
            //on cancel button press
        });
    }
};