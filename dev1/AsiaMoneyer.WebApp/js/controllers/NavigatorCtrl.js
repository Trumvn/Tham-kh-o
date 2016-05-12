function NavigatorCtrl($scope, $rootScope, $state, $http, $modal, $localStorage, spinnerService, authService, Constants) {
    $scope.createNewProject = function () {
        var modalInstance = $modal.open({
            templateUrl: 'views/modals/newProject.html',
            controller: ProjectCtrl
        });

        modalInstance.result.then(function () {
            //on ok button press 
            $scope.loadRecentProjects();
            $rootScope.$broadcast(Constants.Events.ProjectNewProjectAdded, null);
        }, function () {
            //on cancel button press
        });
    }

    $rootScope.$on('logon', function (event, args) {
        $scope.onload();
    });

    $rootScope.$on('logoff', function () {
        //$stateProvider.state.go('login');
    });

    $rootScope.$on(Constants.Events.ProjectListChanged, function (event, args) {
        $scope.loadRecentProjects();
    });

    $rootScope.$on(Constants.Events.ProjectHighlightColorChanged, function (event, data) {
        $scope.updateProjectHighlightColor(data);
    });

    $rootScope.$on(Constants.Events.ProjectFavoriteChanged, function (event, data) {
        $scope.updateProjectFavoriteList(data);
    });

    $rootScope.$on(Constants.Events.ProjectAddingNew, function (event, data) {
        $scope.createNewProject();
    });
    
    $scope.RecentProjects = [];
    $scope.FavoriteProjects = [];

    $scope.loadRecentProjects = function () {
        $scope.loading = true;

        $http.get(Constants.WebApi.Application.GetNavigator).then(function (response) {
            $scope.RecentProjects = response.data.Projects;
            $scope.loading = false;
        }, function (response) {
            $scope.loading = false;
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Could not load recent projects!')
        });        
    }

    $scope.loadFavotiteProjects = function () {

        $http.post(Constants.WebApi.Project.GetFavoriteProjectHeaders, null).then(function (response) {
            $scope.FavoriteProjects = response.data;
        }, function (response) {
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Could not load favotite projects!')
        });
    }

    $scope.onload = function ()
    {
        if (authService.isAuthenticated()) {
            $scope.loadRecentProjects();
            $scope.loadFavotiteProjects();
        }
    }

    $scope.go2list = function (projectId) {
        $state.go('app.project.list', { 'id': projectId });
    }

    $scope.updateProjectHighlightColor = function(projectDto)
    {
        for(var i = 0; i < $scope.RecentProjects.length; i ++)
        {
            var project = $scope.RecentProjects[i];
            if(project.Id == projectDto.Id)
            {
                project.User.HighlightColor = projectDto.HighlightColor;
                break;
            }
        }
    }

    $scope.updateProjectFavoriteList = function(projectDto)
    {
        if (projectDto.IsFavorite) {
            $scope.FavoriteProjects.push(projectDto);
        }
        else {
            for (var i = 0; i < $scope.FavoriteProjects.length; i++) {
                var project = $scope.FavoriteProjects[i];

                if (project.Id == projectDto.Id) {
                    // remove
                    var index = $scope.FavoriteProjects.indexOf(project);
                    $scope.FavoriteProjects.splice(index, 1);

                    break;
                }
            }
        }
    }
}