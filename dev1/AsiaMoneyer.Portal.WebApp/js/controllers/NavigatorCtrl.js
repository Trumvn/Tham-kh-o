function NavigatorCtrl($scope, $rootScope, $state, $http, $modal, $localStorage, spinnerService, authService, Constants) {
    $scope.createNewProject = function () {
        var modalInstance = $modal.open({
            templateUrl: 'views/modals/newProject.html',
            controller: ProjectCtrl
        });

        modalInstance.result.then(function () {
            //on ok button press 
            $scope.loadRecentProjects();
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

    $scope.RecentProjects = [];

    $scope.loadRecentProjects = function () {
        $scope.loading = true;

        $http.get(Constants.WebApi.Application.GetNavigator).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.RecentProjects = response.data.Projects;
            $scope.loading = false;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Could not load recent projects!')
        });        
    }

    $scope.onload = function ()
    {
        if (authService.isAuthenticated()) {
            $scope.loadRecentProjects();
        }
    }

    $scope.go2list = function (projectId) {
        $state.go('app.project.list', { 'id': projectId });
    }
}