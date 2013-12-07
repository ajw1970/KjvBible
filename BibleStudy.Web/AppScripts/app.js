"use strict";

var app = angular.module("app", ['ngResource']);

app.factory("Chapters", function ($resource) {
    return $resource("/api/scriptures/:id");
});

app.controller("BibleStudyCtrl", function ($scope, Chapters) {
    $scope.chapters = [{
        name: "Genesis 1", 
        verses: [{
            num: 1, 
            text: "In the beginning God created the heaven and the earth."
        },
        {
            num: 2, 
            text: "And the earth was without form, and void; and darkness was upon the face of the deep. And the Spirit of God moved upon the face of the waters."
        }]},
        {
            name: "John 1",
            verses: [{
                num: 1,
                text: "In the beginning was the Word, and the Word was with God, and the Word was God."
            },
            {
                num: 2,
                text: "The same was in the beginning with God."
            }]
        }];
    $scope.showResults = false;
    $scope.lookup = function () {
        console.log("User entered: " + $scope.enteredPartNo);
        $scope.enteredPartNo = $scope.enteredPartNo.replace('*', '-');
        Chapters.get({ partNo: $scope.enteredPartNo }, function (data) {
            $scope.part = data;
            $scope.showResults = true;
        });
    };
});
