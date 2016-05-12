function ProjectSummaryCtrl($scope, $http, $localStorage, $modal, $stateParams, SweetAlert, userPhotoService, Constants) {

    $scope.PageSize = 20;

    $scope.projectId = $stateParams.id;
    $scope.AuditLogs = [];
    
    var ProjectDto = { Id: $scope.projectId}
    var PagingDto = { Id: $scope.projectId, Page: 0, Top: $scope.PageSize }

    $scope.loadProjectRecentActivity = function () {
        $scope.loading = true;
        
        $http.post(Constants.WebApi.Project.GetProjectRecentActivity, PagingDto).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.AuditLogs = response.data;
            $scope.loading = false;
            $scope.loadmorebutton = true;
            if (response.data.length < $scope.PageSize) {
                $scope.loadmorebutton = false;
            }
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            SweetAlert.swal({
                title: "Error!",
                text: "Load Summay Failed!",
                type: "warning"
            });
        });
    }
    $scope.getProject = function () {
        $http.post(Constants.WebApi.Project.GetProjectSummary,ProjectDto).then(function (response) {
            $scope.proj = response.data;
            $scope.LoadProjectAnalyseInformation($scope.projectId);
            $scope.loadProjectRecentActivity();
        },function(response){
            SweetAlert.swal({
                title: "Error!",
                text: "Load Project Summary Failed!",
                type: "warning"
            });
        });
    }

    $scope.onload = function () {
        $scope.getProject();
    }

    $scope.loadMore = function () {
        PagingDto.Page++;
        $scope.loading = true;
        $http.post(Constants.WebApi.Project.GetProjectRecentActivity, PagingDto).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.AuditLogs = $scope.AuditLogs.concat(response.data);
            $scope.loading = false;
            if (response.data.length < $scope.PageSize) {
                $scope.loadmorebutton = false;
            }
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            SweetAlert.swal({
                title: "Error!",
                text: "Load Summay Failed!",
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
            $rootScope.$broadcast(Constants.Events.ProjectListChanged, {});
        }, function () {
            //on cancel button press
        });
    }
    $scope.LoadProjectAnalyseInformation = function (projectId) {
        var transactionFilter = {
            ProjectId: projectId
        };

        $http.post(Constants.WebApi.Project.LoadProjectAnalyseInformation, transactionFilter).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            var analysingData = response.data;

            $scope.AnalyseData = analysingData;

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            SweetAlert.swal({
                title: "Error!",
                text: "Load Analyse Information Failed!",
                type: "warning"
            });
        });
    }

    $scope.$watch('AuditLogs', function (newVal, oldVal) {
        if (newVal != null) {
            for (var i = 0; i < newVal.length; i++) {
                var user = newVal[i].User;
                if (user != null && user.Id != null && (user.Photo == null || user.Photo.length < 8)) {
                    var photoResult = userPhotoService.getUserPhoto(user);
                    photoResult.then(function (data) {
                    }, function (error) { });
                }
            }
        }
    }, true);
}