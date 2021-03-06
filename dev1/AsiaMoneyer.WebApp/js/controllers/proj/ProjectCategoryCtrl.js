﻿function ProjectCategoryCtrl($scope, $http, $localStorage, $modal, $stateParams, SweetAlert, userPhotoService, Constants) {
    $scope.projectId = $stateParams.id;

    $scope.SelectedCategory = null;

    $scope.budgetLoading = false;
    $scope.Categories = [];
    $scope.ParentCategories=[];
    $scope.loadCategories = function () {
        $scope.loading = true;

        var category = {
            ProjectId: $scope.projectId
        }
        $http.post(Constants.WebApi.Project.GetProjectCategories, category).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.IncomeCategoryCount = 0;
            $scope.Categories = response.data;
            for (var i = 0; i < $scope.Categories.length; i++) {
                if ($scope.Categories[i].IsIncome==true) {
                    $scope.IncomeCategoryCount += 1;
                }
            };
            $scope.loading = false;
            }, function (response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.                   
                $scope.loading = false;
                SweetAlert.swal({
                    title: "Error!",
                    text: "Load Category Failed!",
                    type: "warning"
                });
            });
    }

    $scope.onload = function () {
        $scope.loadCategories();
    }

    $scope.addNewCategoryExpense = function()
    {
        var newCategory = {
            ProjectId: $scope.projectId,
            ParentId:null,
            CategoryTitle: '',
            CategoryDescription: null,
            HighlightColor: null,
            IsIncome: false,
            IsClosed:false,
            IsDelete: false,
        }
        $scope.Categories.push(newCategory);
    }

    $scope.addNewCategoryIncome = function () {
        var newCategory = {
            ProjectId: $scope.projectId,
            ParentId: null,
            CategoryTitle: '',
            CategoryDescription: null,
            HighlightColor: null,
            IsClosed: false,
            IsIncome: true,
            IsDelete: false,
        }
        $scope.Categories.push(newCategory);
        $scope.IncomeCategoryCount += 1;
    }

    $scope.loadParentcategories = function (categoryDto) {
        $http.post(Constants.WebApi.Project.GetParentCategories, categoryDto).then(function (response) {
            $scope.ParentCategories = response.data;

            for (var i = 0; i < $scope.ParentCategories.length; i++)
            {
                if (categoryDto.ParentId == $scope.ParentCategories[i].Id)
                {
                    categoryDto.Parent = $scope.ParentCategories[i];
                }
            }

            $scope.loading = false;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Save category failed!')
        });
    }
    $scope.checkParent = function (categoryDto) {
        $http.post(Constants.WebApi.Project.CheckParent, categoryDto).then(function (response) {
            $scope.isParent = response.data;
            if ($scope.isParent==true) {
                $scope.ParentCategories = [];
            } else {
                
            }
        }, function (response) {
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Check Parent failed!')
        });
    }
    $scope.selectCategory = function (categoryDto) {

        $http.post(Constants.WebApi.Project.CheckParent, categoryDto).then(function (response) {
            $scope.isParent = response.data;
            if ($scope.isParent == true) {
                $scope.ParentCategories = [];
            } else {
                $scope.loadParentcategories(categoryDto);
            }
        }, function (response) {
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Check Parent failed!')
        });


        
        $scope.SelectedCategory = categoryDto;
         
    }

    $scope.changeCategory = function (category) {
        $scope.loading = true;
        var parentcategory = { Id: $scope.SelectedCategory.Id,CategoryDescription:$scope.SelectedCategory.CategoryDescription,IsClosed:$scope.SelectedCategory.IsClosed,IsDeleted:$scope.SelectedCategory.IsDeleted,HighlightColor:$scope.SelectedCategory.HighlightColor, CategoryTitle: $scope.SelectedCategory.CategoryTitle, ParentId: category.Id };
        $http.post(Constants.WebApi.Project.SaveCategory, parentcategory).then(function (response) {
            
            $scope.loadCategories();
            $scope.SelectedCategory = response.data;
            $scope.checkParent(category)
            $scope.loading = false;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Save category failed!')
        });
        
    }

    $scope.removeCategory = function (category) {

        for (var i = 0; i < $scope.Categories.length; i++)
        {
            if ($scope.Categories[i].Id == category.ParentId)
            {
                var index = $scope.Categories[i].Childs.indexOf(category);
                $scope.Categories[i].Childs.splice(index, 1);
            }
        }

        category.ParentId = null;
        category.Parent = null;

        $scope.Categories.push(category);

        $scope.partialSave();
    }

    $scope.saveCategory = function()
    {
        $scope.loading = true;

        $http.post(Constants.WebApi.Project.SaveCategory, $scope.SelectedCategory).then(function (response) {
            $scope.SelectedCategory.Id = response.data.Id;
            $scope.loading = false;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.                   
            $scope.loading = false;
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Save category failed!')
        });
    }

    $scope.partialSave = function()
    {
        $scope.saveCategory();
    }

    $scope.remove = function () {

    }

    $scope.closeCategory = function (category, isClosed) {
        category.IsClosed = isClosed;
        $http.post(Constants.WebApi.Project.CloseCategory, category).then(function (response) {
            category.IsClosed = isClosed;
        }, function (response) {
            category.IsClosed = !isClosed;
            toastr.options.closeButton = true;
            toastr.error('Network Error', 'Save category failed!')
        });
    }

    $scope.confirmDelete = function (category) {
        SweetAlert.swal({ title: "Delete this category?", text: "You will not be able to recover this!", type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!", closeOnConfirm: true },
            function (isConfirm) {
                if (isConfirm) {
                    $scope.deleteCategory(category);
                }
            });
    }

    $scope.deleteCategory = function (category) {
        $http.post(Constants.WebApi.Project.DeleteCategory, category).then(function (response) {
            var index = $scope.Categories.indexOf(category);
            $scope.Categories.splice(index, 1);
            $scope.SelectedCategory = null;
        }, function (response) {
            SweetAlert.swal({
                title: "Error!",
                text: "Delete category error!",
                type: "warning"
            });
        });
    }

    $scope.setHighlightColor = function (category, color) {
        if (category != null) {
            category.HighlightColor = color;
            $scope.partialSave();
        }
    }

    $scope.createBudget = function(categoryId)
    {
        var modalInstance = $modal.open({
            templateUrl: 'views/modals/CategoryBudget.html',
            controller: CategoryBudgetCtrl,
            resolve: {
                categoryId: function () {
                    return categoryId;
                }
            }
        });

        modalInstance.result.then(function () {
            //on ok button press 
            //$scope.loadRecentProjects();
        }, function () {
            //on cancel button press
        });
    }

    $scope.Budgets = [];
    $scope.loadBudgets = function (categoryId) {
        $scope.budgetLoading = true;
        $http.get(Constants.WebApi.Project.GetCategoryBudgets, { params: { categoryId: categoryId } }).then(function (response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.Budgets = response.data;
            $scope.budgetLoading = false;
        }, function (response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
            $scope.budgetLoading = false;
            toastr.error('Network Error', 'Could not load budgets!')
        });
    }

    $scope.confirmRemoveBudget = function(budgetId)
    {
        SweetAlert.swal({ title: "Delete this budget?", text: "You will not be able to recover this!", type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!", closeOnConfirm: true },
            function () {
                var categoryId = $scope.SelectedCategory.Id;
                $scope.removeBudget(budgetId, categoryId);
            });
    }
    

    $scope.removeBudget = function(budgetId, categoryId)
    {
        var budgetDto = {
            Id: budgetId,
            CategoryId: categoryId
        };

        $http.post(Constants.WebApi.Project.DeleteBudget, budgetDto).then(function (response) {
            $scope.Budgets = response.data;
        }, function (response) {
            SweetAlert.swal({
                title: "Error!",
                text: "Remove budget error!",
                type: "warning"
            });
        });
    }

    $scope.$watch('SelectedCategory', function (newVal, oldVal) {        
        if ($scope.SelectedCategory != null && $scope.SelectedCategory.Id != null) {
            var categoryId = $scope.SelectedCategory.Id;
            $scope.loadBudgets(categoryId);
            $scope.loadAuditLogs(categoryId);
            $scope.loadUserComments(categoryId);

        }
        else {
            $scope.Budgets = [];
        }
    }, false);

    $scope.AuditLogs = [];
    $scope.CommentText = '';
    $scope.UserComments = [];

    $scope.loadAuditLogs = function (categoryId) {
        if (!(angular.isUndefined(categoryId) || categoryId === null)) {
            $http.get(Constants.WebApi.AuditLog.GetProjectCategoryAuditLogs, { params: { CategoryId: categoryId } }).then(function (response) {
                // this callback will be called asynchronously
                // when the response is available
                $scope.AuditLogs = response.data;
            }, function (response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                toastr.options.closeButton = true;
                toastr.error('Network Error', 'Could not load audit logs!')
            });
        }
    }

    $scope.loadUserComments = function (categoryId) {
        if (!(angular.isUndefined(categoryId) || categoryId === null)) {
            $http.get(Constants.WebApi.UserComment.GetProjectCategoryUserComments, { params: { CategoryId: categoryId } }).then(function (response) {
                // this callback will be called asynchronously
                // when the response is available
                $scope.UserComments = response.data;
            }, function (response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.                   
                toastr.options.closeButton = true;
                toastr.error('Network Error', 'Could not user comments!')
            });
        }
    }

    $scope.addComment = function () {
        var categoryId = $scope.SelectedCategory.Id;
        if (categoryId != null) {
            var comment = {
                ObjectId: categoryId,
                CommentText: $scope.CommentText
            }
            $http.post(Constants.WebApi.UserComment.CreateProjectCategoryUserComment, comment).then(function (response) {
                // this callback will be called asynchronously
                // when the response is available
                var savedComment = response.data;
                savedComment.User = {
                    Id: savedComment.UserId,
                    Photo: null
                };

                $scope.UserComments.push(savedComment);

                $scope.CommentText = '';
            }, function (response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.                   
                SweetAlert.swal({
                    title: "Error!",
                    text: "Add comment failed!",
                    type: "warning"
                });
            });
            $scope.loadUserComments();
        }
    }

    $scope.$watch('UserComments', function (newVal, oldVal) {
        if (newVal != null) {
            for (var i = 0; i < newVal.length; i++) {
                var user = newVal[i].User;
                if (user != null && user.Id != null && (user.Photo == null || user.Photo.length < 8)) {
                    var photoResult = userPhotoService.getUserPhoto(user);
                    photoResult.then(function (data) {
                    }, function (error) { });
                }
            }
        }
    }, true);
};