function AdminProjectsCtrl($scope, $http, $modal, $stateParams, SweetAlert, Constants) {

    $scope.Projects = null;

    $scope.SearchFilter = {
        Title: ''
    };

    $scope.loadProjects = function (filter) {
        $http.post(Constants.WebApi.AdProject.SearchProjects, filter).then(function (response) {
            $scope.Projects = response.data.Projects;

        }, function (response) {
            SweetAlert.swal({
                title: "Error!",
                text: response.data,
                type: "warning"
            });
        });
    }

    $scope.onload = function()
    {
        $scope.loadProjects($scope.SearchFilter);
    }

    $scope.open = function (project) {
        var modalInstance = $modal.open({
            templateUrl: 'views/admin/project_view.html',
            controller: ProjectViewCtrl,
            resolve: {
                ProjectId: function () {
                    return project.Id;
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