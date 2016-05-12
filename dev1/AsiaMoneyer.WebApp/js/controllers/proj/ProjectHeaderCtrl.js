function ProjectHeaderCtrl($scope, $rootScope, $http, $localStorage, $modal, $state, $stateParams, SweetAlert, timeAgo, nowTime, userPhotoService, Constants) {
    $rootScope.CurrentProject = {
        Id: $stateParams.id,
        Title: '',
        Currency: null,
    };
       
    $scope.Members = [];
    $scope.EmptyMembers = [1, 2, 3, 4, 5];

    $scope.loadProjectHeader = function (projectId) {
        $http.get(Constants.WebApi.Project.GetProjectHeader, { params: { projectId: projectId } }).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $rootScope.CurrentProject = response.data;
            $rootScope.$broadcast(Constants.Events.ProjectHeaderUpdated, { Project: $rootScope.CurrentProject });
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: "Load Project Header Failed!",
                type: "warning"
            });
        });
    }

    $scope.onload = function ()
    {
        $scope.loadProjectHeader($rootScope.CurrentProject.Id);
        $scope.loadProjectMembers($rootScope.CurrentProject.Id);
    }

    $rootScope.$on(Constants.Events.ProjectChanged, function (event, data) {
        $scope.CurrentProject.Id = data;
        $scope.loadProjectHeader($rootScope.CurrentProject.Id);
        $scope.loadProjectMembers($rootScope.CurrentProject.Id);
    });

    $scope.go2list = function(projectId)
    {
        $state.go('app.project.list', { 'id': projectId });
    }

    $scope.go2Category = function (projectId) {
        $state.go('app.project.category', { 'id': projectId });
    }

    $scope.go2Account = function (projectId) {
        $state.go('app.project.account', { 'id': projectId });
    }

    $scope.go2Analyse = function (projectId) {
        $state.go('app.project.analyse', { 'id': projectId });
    }

    $scope.go2Summary = function (projectId) {
        $state.go('app.project.summary', { 'id': projectId });
    }

    $scope.loadProjectMembers = function (projectId) {
        $http.get(Constants.WebApi.Project.GetProjectMembers, { params: { projectId: projectId } }).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Members = response.data;

            $scope.EmptyMembers = [];
            for(var i = 0; i < 5 - $scope.Members.length; i ++)
            {
                $scope.EmptyMembers.push(i);
            }
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            alert('Failed');
        });
    }

    $scope.$watch('Members', function (newVal, oldVal) {
        if (newVal != null) {
            for (var i = 0; i < newVal.length; i++) {
                var client = newVal[i].Client;
                if (client.User.Id != null && (client.User.Photo == null || client.User.Photo.length < 8)) {
                    var photoResult = userPhotoService.getUserPhoto(client.User);
                    photoResult.then(function (data) {
                    }, function (error) { });
                }
            }
        }
    }, true);
};