function MessagingTemplateListCtrl($scope, $http, $location, $stateParams, SweetAlert, Constants) {

    $scope.TemplateId = $stateParams.Id;

    $scope.TemplateName = '';
    $scope.Total = 0;
    $scope.TemplateContents = {};

    $scope.loadTemplateContents = function (templateId) {
        $http.get(Constants.WebApi.Messaging.GetTemplateContentTitles, { params: { templateId: templateId } }).then(function (response) {

            $scope.TemplateContents = response.data.TemplateContentList;
            $scope.Total = response.data.Total;
            $scope.TemplateName = response.data.TemplateName;

        }, function (response) {
            SweetAlert.swal({
                title: "Error!",
                text: "Load Inbox Failed!",
                type: "warning"
            });
        });

    }

    $scope.onload = function () {
        $scope.loadTemplateContents($scope.TemplateId);
    }

}