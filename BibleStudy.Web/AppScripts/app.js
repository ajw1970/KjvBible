"use strict";

var app = angular.module("app", ['ngResource']);

app.factory("Chapters", function ($resource) {
    return $resource("/api/chapters/:id");
});

app.controller("BibleStudyCtrl", function ($scope, Chapters) {
    $scope.chapters = Chapters.query();
    $scope.next = function () {
        console.log("User clicked next");
        var chapter = $scope.chapters[0];
        Chapters.get({ id: { bookName: chapter.bookName, number: chapter.number } }, function (data) {
            $scope.chapters.push(data);
        });
    };
});
