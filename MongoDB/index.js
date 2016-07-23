var express = require('express');
var app = express();
var parser = require("./parser.js");
var MongoClient = require('mongodb').MongoClient;
var assert = require ('assert');