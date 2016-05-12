function ProjectSettingCtrl($scope, $http, $modalInstance, ProjectId, userPhotoService, SweetAlert, Constants) {
    $scope.projectDto = {
        Id: ProjectId,
        ProjectTitle: '',
        ProjectDesc: '',
        Currency: 'VND',
        IsPrivate: true
    };
    $scope.invitedMemberEmail = '';
    $scope.Members = {};

    $scope.saveProject = function () {
        $http.post(Constants.WebApi.Project.UpdateProject, $scope.projectDto).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            toastr.options.closeButton = true;
            toastr.success('Save', 'Project saved');
            $scope.ok();
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            alert('Failed');
        });
    }

    $scope.ok = function () {
        $modalInstance.close();
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.onload = function () {
        if (!(angular.isUndefined(ProjectId) || ProjectId === null))
        {
            $scope.loadProject(ProjectId);
            $scope.loadProjectMembers(ProjectId);
        }
    };

    $scope.loadProject = function (projectId) {
        $http.get(Constants.WebApi.Project.GetProject, { params: { projectId: projectId } }).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.projectDto = response.data;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            alert('Failed');
        });
    }

    $scope.loadProjectMembers = function (projectId) {
        $http.get(Constants.WebApi.Project.GetProjectMembers, { params: { projectId: projectId } }).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Members = response.data;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            alert('Failed');
        });
    }

    $scope.inviteMember = function () {
        var inviteMemberDto = {
            ProjectId: ProjectId,
            Email: $scope.invitedMemberEmail
        };

        $http.post(Constants.WebApi.Project.InviteMember, inviteMemberDto).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Members = response.data;
            toastr.options.closeButton = true;
            toastr.success('Add Member', 'Add member saved');
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            alert('Failed');
        });
    }

    $scope.removeMember = function (projectMemberId) {
        var projectMemberDto = {
            Id: projectMemberId
        };
        $http.post(Constants.WebApi.Project.RemoveMember, projectMemberDto).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Members = response.data;
            toastr.options.closeButton = true;
            toastr.success('Remove Member', 'saved');
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            alert('Failed');
        });
    }

    $scope.confirmDeleteProject = function (project) {
        SweetAlert.swal({ title: "Delete this project?", text: "You will not be able to recover this!", type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!", closeOnConfirm: true },
            function (isConfirm) {
                if (isConfirm) {
                    $scope.deleteProject(project);
                }
            });
    }

    $scope.deleteProject = function (project) {
        $http.post(Constants.WebApi.Project.DeleteProject, project).then(function (response) {
            $scope.ok();
        }, function (response) {
            SweetAlert.swal({
                title: "Error!",
                text: "Delete project error!",
                type: "warning"
            });
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
}