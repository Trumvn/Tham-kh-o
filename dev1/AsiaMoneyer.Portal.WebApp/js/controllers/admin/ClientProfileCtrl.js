function ClientProfileCtrl($scope, $http, $modalInstance, Constants, ProfileId) {

    $scope.UserProfile = {};

    $scope.loadUserProfile = function (profileId) {
        $http.get(Constants.WebApi.Client.GetUserProfile, {params: { profileId: profileId }}).then(function (response) {
            $scope.UserProfile = response.data;

        }, function (response) {
            alert('failed!');
        });
    }
    $scope.onload = function()
    {
        $scope.loadUserProfile(ProfileId);
    }

    $scope.save = function () {
        //$scope.saveProfile();
        $scope.ok();
    }

    $scope.saveProfile = function (userProfile) {

        $http.post(Constants.WebApi.Client.SaveUserProfileDetail, userProfile).then(function (response) {
            $scope.ok();
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            alert('save failed!');
        });
    }

    $scope.ok = function () {
        $modalInstance.close();        
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
};