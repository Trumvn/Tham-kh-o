function AccountPaymentMethodCtrl($scope, $http, $stateParams, $location, SweetAlert, Constants) {

    $scope.InvoiceId = $stateParams.id;
    $scope.PaymentMethods = {};

    $scope.loadPaymentMethods = function () {
        $scope.loading = true;

        $http.post(Constants.WebApi.Billing.GetPaymentMethods, null).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.PaymentMethods = response.data;
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
        $scope.loadPaymentMethods();
    }

    $scope.purchase = function (paymentMethodId) {
        console.log("Invoice:", $scope.InvoiceId, ", Payment Method:", paymentMethodId);

        $scope.loading = true;

        var paymentObject =
            {
                InvoiceId: $scope.InvoiceId,
                PaymentMethodId: paymentMethodId,
            }

        $http.post(Constants.WebApi.Billing.Purchase, paymentObject).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            var invoiceDto = response.data;

            $scope.loading = false;

            $scope.placeOrderSuccess(invoiceDto.Id);

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Save account failed!')
            console.log(response.data);
        });
    }

    // Test Only
    $scope.placeOrderSuccess = function (invoiceId) {

        var purchaseObject =
            {
                InvoiceId: invoiceId,
            }

        $http.post(Constants.WebApi.Billing.PaymentSuccess, purchaseObject).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            toastr.options.closeButton = true;

            toastr.success('Place Order Successfully', '');

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Place Order failed!')
            console.log(response.data);
        });
    }
}