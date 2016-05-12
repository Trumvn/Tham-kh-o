function MessagingTemplateViewCtrl($scope, $http, $location, $stateParams, SweetAlert, Constants) {
    $scope.MailTemplateContent = {
        Id: $stateParams.Id,
        AutoMessagingTemplateId: null,
        AutoMessagingTemplateName: null,
        Lang: 'en',
        MessagingSubject: '',
        MessagingFromName: '',
        MessagingFromEmailAddress: '',
        MessagingTo: '',
        MessagingCc: '',
        MessagingBcc: '',
        MessagingContent: '',
        Tags: '',
        IsPublish: false,
        FromDate: null,
        EndDate: null,
        CreatedDate: null
    }

    $scope.loadTemplateContent = function (contentId) {
        $http.get(Constants.WebApi.Messaging.GetMailTemplateContent, { params: { contentId: contentId } }).then(function (response) {

            $scope.MailTemplateContent = response.data;

        }, function (response) {
            SweetAlert.swal({
                title: "Error!",
                text: "Load Template Content Failed!",
                type: "warning"
            });
        });

    }

    $scope.onload = function () {
        if ($scope.MailTemplateContent.Id != null) {
            $scope.loadTemplateContent($scope.MailTemplateContent.Id);
        }
    }
}