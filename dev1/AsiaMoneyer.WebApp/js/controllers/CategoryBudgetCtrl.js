function CategoryBudgetCtrl($scope, $http, $modalInstance, categoryId, Constants) {
    
    $scope.CategoryBudget = {
        Id: null,
        CategoryId: categoryId,
        StartDate: null,
        EndDate: null,
        BudgetAmount: null,
        TimeFrequencyId: 3,
        IsActive: true
    };

    $scope.onload = function () {
    }

    $scope.saveBudget = function () {
        $http.post(Constants.WebApi.Project.SaveCategoryBudget, $scope.CategoryBudget).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
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
}