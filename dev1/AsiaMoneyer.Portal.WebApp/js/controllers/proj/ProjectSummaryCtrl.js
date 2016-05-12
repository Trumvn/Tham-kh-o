function ProjectSummaryCtrl($scope, $http, $localStorage, $modal, $stateParams, SweetAlert, userPhotoService, Constants) {
    $scope.projectId = $stateParams.id;
    $scope.AuditLogs = [];
    $scope.page = 1;
    var ProjectDto = { Id: $scope.projectId}
    var PagingDto = { Id: $scope.projectId, Page: $scope.page,Top:50}
    $scope.loadRrojectSummary = function () {
        $scope.loading = true;
        
        $http.post(Constants.WebApi.Project.GetProjectSummary, PagingDto).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.AuditLogs = response.data;
            $scope.loading = false;
            $scope.loadmorebutton = true;
            if (response.data.length < 50) {
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
        $http.post(Constants.WebApi.Project.GetProjectForSummary,ProjectDto).then(function (response) {
            $scope.proj=response.data;
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
        $scope.loadRrojectSummary();
    }
    $scope.loadMore = function () {
        //$scope.page++;
        PagingDto.Page++;
        $scope.loading = true;
        $http.post(Constants.WebApi.Project.GetProjectSummary, PagingDto).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.AuditLogs = $scope.AuditLogs.concat(response.data);
            $scope.loading = false;
            if (response.data.length < 50) {
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
}