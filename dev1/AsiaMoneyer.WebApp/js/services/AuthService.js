'use strict';
function authService($rootScope, $http, $q, $localStorage, Constants) {

    var serviceBase = Constants.AuthenticationService;
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: "",
        firstName: "",
        lastName: "",
        token: ""
    };

    var _saveRegistration = function (registration) {

        _logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });

    };

    var _isAuthenticated = function()
    {
        if($localStorage.authorizationData != null && $localStorage.authorizationData.isAuth)
        {
            return true;
        }
        return false;
    }

    var _login = function (loginData) {

        var data = "grant_type=password&username=" + loginData.userNameOrEmail + "&password=" + loginData.password;

        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

            _authentication = { token: response.access_token, userName: loginData.userNameOrEmail, firstName: response.firstName, lastName: response.lastName, isAuth: true };
            $localStorage.authorizationData = _authentication;

            $rootScope.$broadcast('authorized', {user: _authentication});

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _logOut = function () {

        //$localStorage.$remove('authorizationData');

        $localStorage.authorizationData = '';
        _authentication = {};

        $rootScope.$broadcast('unauthorized', { user: _authentication });
    };

    var _checkAuthenticated = function () {
        $http.get(Constants.WebApi.Accounts.IsAuthenticated).then(function (response) {
            return true;
        });
        return false;
    }

    var _fillAuthData = function () {

        var authData = $localStorage.authorizationData;
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
        }
    }

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.isAuthenticated = _isAuthenticated;
    authServiceFactory.checkAuthenticated = _checkAuthenticated;

    return authServiceFactory;
}