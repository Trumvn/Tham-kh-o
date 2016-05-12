function AccountPaymentCtrl($scope, $http, $stateParams, $state, $location, SweetAlert, Constants) {

    $scope.InvoiceId = $stateParams.id;

    $scope.subscription = {};

    $scope.loadSubscriptionFromInvoice = function (invoiceId) {
        $scope.loading = true;

        var purchaseObject =
        {
            InvoiceId: invoiceId,
        }

        $http.post(Constants.WebApi.Billing.GetInvoiceProductPrices, purchaseObject).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.subscription = response.data;
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
        $scope.loadSubscriptionFromInvoice($scope.InvoiceId);
    }

    $scope.payInvoice = function (invoiceId, priceId) {
        console.log("InvoiceId:", invoiceId, ", Price:", priceId);

        $scope.loading = true;

        var purchaseObject =
            {
                PriceId: priceId,
                InvoiceId: invoiceId,
            }

        $http.post(Constants.WebApi.Billing.PayInvoice, purchaseObject).then(function (response) {
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