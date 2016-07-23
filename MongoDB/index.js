var jsonfile = require('jsonfile');
const path = require('path');
var file = path.join(__dirname, 'raw_data', 'h9.json')

console.log(file);

jsonfile.readFile(file, function(err, obj) {
    
    for(var i = 0; i< obj.length; ++i){
        console.log("type:" + obj[i].type);
    }
     
})


