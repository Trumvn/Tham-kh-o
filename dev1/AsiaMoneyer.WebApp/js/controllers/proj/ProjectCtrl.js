function ProjectCtrl($scope, $http, $modalInstance, SweetAlert, Constants) {
    $scope.projectDto = {
        ProjectTitle: '',
        ProjectDesc: '',
        Currency: 'VND',
        FinanceYearStartMonth: 1,
        IsPrivate: true
    };

    $scope.createProject = function()
    {
        $http.post(Constants.WebApi.Project.CreateProject, $scope.projectDto).then(function (response) {
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

    $scope.ok = function () {
        $modalInstance.close();        
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}