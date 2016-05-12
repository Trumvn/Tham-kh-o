function AccountRegisterCtrl($scope, $http, $location, SweetAlert, authService, Constants) {
    $scope.CreateUserBindingModel =
    {
        Email: '',
        Username: '',
        FirstName: '',
        LastName: '',
        RoleName: '',
        Password: '',
        ConfirmPassword: '',
        IsAcceptTerm:''
    }
    $scope.createNewUser = function () {
        $scope.loading = true;              

        $scope.CreateUserBindingModel.Username = $scope.CreateUserBindingModel.Email;
        $scope.CreateUserBindingModel.ConfirmPassword = $scope.CreateUserBindingModel.Password;

        $http.post(Constants.WebApi.Accounts.CreateUser, $scope.CreateUserBindingModel).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available

            this.loginData = {
                userNameOrEmail: $scope.CreateUserBindingModel.Username,
                password: $scope.CreateUserBindingModel.Password
            }

            authService.login(loginData).then(function (response) {
                $scope.CreateUserBindingModel = {};
                $scope.loading = false;
                $location.path("/app/dashboard");
            },
             function (err) {
                 $scope.loading = false;
                 SweetAlert.swal({
                     title: "Login!",
                     text: "Authentication Proceed!",
                     type: "info"
                 });
             });

        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            SweetAlert.swal({
                title: "Register new user Failed!",
                text: response.data.Message,
                type: "warning"
            });
        });

    }

};


function AccountLoginCtrl($scope, $http, SweetAlert, $location, $stateParams, authService) {

    $scope.isLogout = $stateParams.logout != null;

    if ($scope.isLogout)
    {
        authService.logOut();
    }

    $scope.LoginUserBindingModel =
     {
         userNameOrEmail: '',
         password: ''
     }    

    $scope.login = function () {
        authService.logOut();
        this.loginData = {
            userNameOrEmail: $scope.LoginUserBindingModel.userNameOrEmail,
            password: $scope.LoginUserBindingModel.password
        }
        $scope.authenticate(this.loginData);
    };

    $scope.authenticate = function (loginData) {
        $scope.loading = true;
        authService.login(loginData).then(function (response) {
            $scope.loading = false;
            $location.path("/app/dashboard");

        },
         function (err) {
             $scope.loading = false;
             SweetAlert.swal({
                 title: "Login Failed!",
                 text: err.data,
                 type: "info"
             });
         });
    };
};

function AccountChangePasswordCtrl($scope, $http, SweetAlert) {
    $scope.ChangePasswordBindingModel =
     {
         OldPassword: '',
         NewPassword: '',
         ConfirmPassword: ''
     }

    $scope.changePassword = function () {

        $scope.loading = true;

        $http.post(Constants.WebApi.Accounts.ChangePassword, $scope.ChangePasswordBindingModel).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available

            $scope.ChangePasswordBindingModel =
                {
                    OldPassword: '',
                    NewPassword: '',
                    ConfirmPassword: ''
                }

            $scope.loading = false;
            SweetAlert.swal({
                title: "Change Password!",
                text: "Your password has been changed successfully!",
                type: "info"
            });
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            SweetAlert.swal({
                title: "Change Password!",
                text: "Failed! Something is wrong!",
                type: "info"
            });
        });
    }    
};

function AccountForgotPasswordCtrl($scope, $http, SweetAlert) {
    $scope.ChangePasswordBindingModel =
    {
        Email: ''
    }

    $scope.forgotPassword = function () {

        $scope.loading = true;

        $http.post(Constants.WebApi.Accounts.ForgotPassword, $scope.ForgotPasswordBindingModel).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available

            $scope.ForgotPasswordBindingModel =
                {
                    Email: ''
                }

            $scope.loading = false;
            SweetAlert.swal({
                title: "Change Password!",
                text: "An email has been sent to you!",
                type: "info"
            });
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            SweetAlert.swal({
                title: "Change Password!",
                text: "Your email has not been existed!",
                type: "info"
            });
        });
    }

    
};

function AccountResetPasswordCtrl($scope, $http, SweetAlert, $location) {
    

    var searchObject = $location.search();
    var code = searchObject.code;

    $scope.ResetPasswordBindingModel =
     {
         Email: '',
         Password: '',
         ConfirmPassword: '',
         Code: code
     }

    $scope.resetPassword = function () {

        $scope.loading = true;

        $http.post(Constants.WebApi.Accounts.ResetPasswordConfirmation, $scope.ResetPasswordBindingModel).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available

            $scope.ResetPasswordBindingModel =
             {
                 Email: '',
                 Password: '',
                 ConfirmPassword: '',
                 Code: ''
             }

            $scope.loading = false;
            alert("Changed");
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            SweetAlert.swal({
                title: "Reset Password!",
                text: "Failed! Something is wrong!",
                type: "info"
            });
        });
    }    
    
};