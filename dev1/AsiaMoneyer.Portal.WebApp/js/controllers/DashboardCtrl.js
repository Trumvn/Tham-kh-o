function DashboardCtrl($scope, $rootScope, $http, $modal, SweetAlert, authService, timeAgo, Constants) {

    $scope.getRandomInt = function (min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    $rootScope.$on('logon', function (event, args) {
        $scope.onload();
    });

    $scope.Projects = [];

    $scope.Budget = {
        Date: new Date(),
        UnclearItems: $scope.getRandomInt(0, 60),
        Amount: $scope.getRandomInt(0, 500000)
    };

    $scope.loadProjects = function () {
        $scope.loading = true;

        $http.get(Constants.WebApi.Project.GetProjects).then(function (response) {
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
                text: "Load Recent Project Failed!",
                type: "warning"
            });
        });
    }

    $scope.onload = function ()
    {
        if (authService.isAuthenticated()) {
            
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

    $scope.setFavorite = function (projId) {
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
}