function DashboardCtrl($scope, $rootScope, $http, $modal, $stateParams, SweetAlert, authService, timeAgo, Constants) {

    $scope.ShowType = $stateParams.show;

    $scope.getRandomInt = function (min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    $rootScope.$on('logon', function (event, args) {
        $scope.onload();
    });

    $rootScope.$on(Constants.Events.ProjectNewProjectAdded, function (event, data) {
        $scope.onload();
    });

    $scope.Projects = [];

    $scope.loadProjects = function (show) {

        $scope.loading = true;

        var projectFilter =
            {
                FilterBy: 0
            };

        if (show == 'list')
        {
            projectFilter.FilterBy = 1;
        }
        else if (show == 'share') {
            projectFilter.FilterBy = 2;
        }
        else if (show == 'archived')
        {
            projectFilter.FilterBy = 3;
        }

        $http.post(Constants.WebApi.Project.GetProjects, projectFilter).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Projects = response.data.Projects;
            $scope.loading = false;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.   
            $scope.loading = false;
            SweetAlert.swal({
                title: "Error!",
                text: "Load Project Failed!",
                type: "warning"
            });
        });
    }

    $scope.onload = function ()
    {
        if ($scope.ShowType == null)
        {
            $scope.ShowType = 'list';
        }

        if (authService.isAuthenticated()) {
            $scope.loadProjects($scope.ShowType);
        }
    }

    $scope.setHighlightColor = function (project, color) {
        if (project != null) {
            var projectDto = {
                Id: project.Id,
                HighlightColor: color
            };

            $http.post(Constants.WebApi.Project.SetProjectHighlightColor, projectDto).then(function (response) {
                project.User.HighlightColor = color;
                $rootScope.$broadcast(Constants.Events.ProjectHighlightColorChanged, projectDto);
            }, function (response) {
                $scope.loading = false;
                SweetAlert.swal({
                    title: "Error!",
                    text: "Set project highlight color failed!",
                    type: "warning"
                });
            });

        }
    }    

    $scope.setFavorite = function (project, isFavorite) {
        var projectDto = {
            Id: project.Id,
            ProjectTitle: project.ProjectTitle,
            IsFavorite: isFavorite
        };

        $http.post(Constants.WebApi.Project.SetProjectFavorite, projectDto).then(function (response) {
            project.IsFavorite = isFavorite;
            project.User.IsFavorite = isFavorite;
            $rootScope.$broadcast(Constants.Events.ProjectFavoriteChanged, projectDto);
        }, function (response) {
            $scope.loading = false;
            SweetAlert.swal({
                title: "Error!",
                text: "Set project favorite failed!",
                type: "warning"
            });
        });
    }

    $scope.setArchived = function (project, isArchived) {
        
        var projectDto = {
            Id: project.Id,
            IsArchived: isArchived
        };

        $http.post(Constants.WebApi.Project.SetProjectArchived, projectDto).then(function (response) {
            project.IsArchived = isArchived;

            // remove
            var index = $scope.Projects.indexOf(project);
            $scope.Projects.splice(index, 1);

        }, function (response) {
            $scope.loading = false;
            SweetAlert.swal({
                title: "Error!",
                text: "Set project archived failed!",
                type: "warning"
            });
        });

    }

    $scope.openProperty = function (projId) {
        var modalInstance = $modal.open({
            templateUrl: 'views/modals/ProjectSetting.html',
            controller: ProjectSettingCtrl,
            resolve: {
                ProjectId: function () {
                    return projId;
                }
            }
        });

        modalInstance.result.then(function () {
            //on ok button press 
            $scope.onload();
            $rootScope.$broadcast(Constants.Events.ProjectListChanged, { });
        }, function () {
            //on cancel button press
        });
    }

    $scope.createNewProject = function()
    {
        $rootScope.$broadcast(Constants.Events.ProjectAddingNew, null);
    }
}