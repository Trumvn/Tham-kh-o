function HeaderCtrl($scope, $rootScope, $http, $modal, $localStorage, SweetAlert, spinnerService, authService, Constants) {

    $scope.UserProfile = {
        Id: null,
        FirstName: '',
        LastName: '',
        Gender: true,
        Photo: 'img/default_avatar.png',
        PhotoSmall: null,
    };

    $scope.openUserProfile = function () {
        var modalInstance = $modal.open({
            templateUrl: 'views/modals/UserProfile.html',
            controller: UserProfileCtrl,
            resolve: {
                UserProfileId: function () {
                    return null;
                }
            }
        });

        modalInstance.result.then(function () {
            //on ok button press 
            $scope.getUserProfile();
        }, function () {
            //on cancel button press
        });
    }

    $rootScope.$on('authorized', function (event, args) {
        $scope.onload();
    });

    $scope.getUserProfile = function () {
        $http.post(Constants.WebApi.Accounts.GetCurrentUserProfile, null).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.UserProfile = response.data;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: "Load User Profile Failed!",
                type: "warning"
            });
        });
    }

    $scope.onload = function () {
        if (authService.isAuthenticated()) {
            $scope.getUserProfile();
        }        
    }
}