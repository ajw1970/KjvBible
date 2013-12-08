"use strict";

var app = angular.module("app", ['ngResource']);

app.factory("Chapters", function ($resource) {
    return $resource("/api/chapters/:id");
});

app.controller("BibleStudyCtrl", function ($scope, Chapters) {
    $scope.chapters = Chapters.query();
    $scope.showResults = false;
    $scope.next = function () {
        console.log("User clicked next");
        Chapters.get({ id: $scope.enteredPartNo }, function (data) {
            $scope.part = data;
            $scope.showResults = true;
        });
    };
});
