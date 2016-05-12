function UserProfileCtrl($scope, $http, $modalInstance, SweetAlert, Constants) {
    $scope.UserProfileDto = {
        Id: null,
        EmailAddress: '',
        FirstName: '',
        LastName: '',
        Gender: true,
        Photo: null,
    };

    $scope.processingCount = 0;
    $scope.isPhotoChanged = false;

    $scope.loadUserProfile = function () {
        $http.post(Constants.WebApi.Accounts.GetCurrentUserProfile, null).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.UserProfileDto = response.data;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
            SweetAlert.swal({
                title: "Error!",
                text: "Load User Profile Failed!",
                type: "warning"
            });
            console.log(response.data);
        });
    }

    $scope.saveUserProfile = function () {
        if ($scope.UserProfileDto.Id != null) {
            $scope.processingCount++;
            $http.post(Constants.WebApi.Accounts.UpdateCurrentUserProfile, $scope.UserProfileDto).then(function (response) {
                $scope.processingCount--;
                if ($scope.processingCount == 0) {
                    $scope.ok();
                }
            }, function (response) {
                $scope.processingCount--;
                SweetAlert.swal({
                    title: "Error!",
                    text: "Save User Profile Failed!",
                    type: "warning"
                });

                console.log(response.data);
            });
        }
    }

    $scope.changeUserProfilePhoto = function () {
        if ($scope.UserProfileDto.Id != null) {
            $scope.processingCount++;
            $http.post(Constants.WebApi.Accounts.ChangeCurrentUserPhoto, $scope.UserProfileDto).then(function (response) {
                $scope.processingCount--;
                if ($scope.processingCount == 0) {
                    $scope.ok();
                }
            }, function (response) {
                $scope.processingCount--;
                SweetAlert.swal({
                    title: "Error!",
                    text: "Change Avatar Failed!",
                    type: "warning"
                });

                console.log(response.data);
            });
        }
    }

    $scope.fileChanged = function (e) {
        var files = e.target.files;

        var fileReader = new FileReader();
        fileReader.readAsDataURL(files[0]);

        fileReader.onload = function (e) {
            $scope.imgSrc = this.result;
            $scope.$apply();

            $scope.isPhotoChanged = true;
        };

    }

    $scope.clear = function () {
        $scope.imageCropStep = 1;
        delete $scope.imgSrc;
        delete $scope.result;
        delete $scope.resultBlob;
    };

    $scope.ok = function () {
        $modalInstance.close();
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.onload = function () {
        $scope.loadUserProfile();
    }

    $scope.saveProfile = function () {
        if ($scope.isPhotoChanged) {
            $scope.changeUserProfilePhoto();
        }
        $scope.saveUserProfile();
    }

}