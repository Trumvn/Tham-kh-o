function AccountUpgradeCtrl($scope, $http, $state, $location, SweetAlert, Constants) {

    $scope.products = {};

    $scope.loadUpgradeProducts = function () {
        $scope.loading = true;

        $http.post(Constants.WebApi.Client.GetUserUpgradeProducts, null).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.products = response.data;
            $scope.loading = false;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Save account failed!')
            console.log(response.data);
        });
    }

    $scope.onload = function () {
        $scope.loadUpgradeProducts();
    }

    $scope.placeOrder = function(productId, priceId)
    {
        console.log("ProductId:", productId, ", Price:", priceId);

        $scope.loading = true;

        var purchaseObject =
            {
                Id: priceId,
                ProductId: productId,
            }

        $http.post(Constants.WebApi.Billing.PlaceOrder, purchaseObject).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            var invoiceDto = response.data;

            $scope.loading = false;

            $scope.proceedToPayment(invoiceDto.Id);

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Save account failed!')
            console.log(response.data);
        });
    }

    $scope.proceedToPayment = function (invoiceId) {
        $state.go('app.account.pmethod', { 'id': invoiceId });
    }

}