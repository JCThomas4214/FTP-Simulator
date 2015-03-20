		var fileList = new Array;
        var htmlList = new Array;         

        function check(box) {            
            document.getElementById(box).checked = true;            
        }

        function uncheck(box) {            
            document.getElementById(box).checked = false;           
        }
        function checkB(file) {
            console.log(file);
            var items = new Array;
            items = checkVal(file);
            if (items[0] == true) {
                deleteItem(items[1]);
            }
            else {
                insertItem(items[1], file);
            }
        }
        function checkVal(file) {
            var found = false;           
            var items = new Array;
            for (var i = 0; i < htmlList.length; i++) {
                if (htmlList[i] == '<li ondblclick=\"deleteItem(this.id);uncheck(\''+file+'\');\" id=\"' + i + '\" name=\"' + file + '\">' + file + '</li>') {
                    found = true;                    
                    break;
                }                
            }
            items[0] = found;
            items[1] = i;
            console.log(items);
            return items;
        }
        function insertItem(number, file) {                     
            htmlList.push('<li ondblclick=\"deleteItem(this.id);uncheck(\''+file+'\');\" id=\"' + number + '\" name=\"' + file + '\">' + file + '</li>');
            fileList.push(file);
            console.log(fileList);
            document.getElementById('selectedFileList').innerHTML = htmlList.join("");
        }
        function deleteItem(number) {
            var tmp = number;            
            htmlList.splice(number, 1);
            fileList.splice(number, 1);
            for (var i = number; i < htmlList.length; i++) {                      
                tmp++;                    
                htmlList[i] = htmlList[i].replace('id=\"' + tmp + '\"', 'id=\"' + i + '\"')
            }
            console.log(fileList);
            document.getElementById('selectedFileList').innerHTML = htmlList.join("");
        }
		 $(document).ready(function () {
        $('#MainTree').fileTree({
            root: 'C:/Users/',
            script: '../Scripts/jqueryFileTree.aspx',            
            multiFolder: false,
            folderEvent: 'dblclick'
        }, function (file) {            
            var items = new Array; //array that contrains check outputs
            items = checkVal(file);    //check will return, true or false and position if the file is already in the list
            if (items[0] == true) {               
                deleteItem(items[1]); //deleteItem will delete an object in the list at position item[1]
                uncheck(file);
            }
            else {                
                insertItem(items[1], file); //insertItem will insert an object into a list with value 'file' and postiion item[1]
                check(file);
            }                 
        }, function (dir) {
            console.log(dir);            
        });
        $('.jqueryFileTree').contextMenu({
            // define which elements trigger this menu
            selector: "li a",           
            // define the elements of the menu
            items: {
                "download": {
                    name: "Add/Remove file",
                    icon: "edit",
                    callback: function (key, opt) {
                        var items = new Array;
                        items = checkVal($(this).attr('rel'));
                        if (items[0] == true) {
                            deleteItem(items[1]);
                            uncheck($(this).attr('rel'));
                        }
                        else {
                            insertItem(items[1], $(this).attr('rel'));
                            check($(this).attr('rel'));
                        }
                    }
                },
                "upload": {
                    name: "Upload to",
                    icon: "add",
                    callback: function (key, opt) {                        
                        var m = "clicked: " + key + " on " + $(this).attr('rel');
                        window.console && console.log(m) || alert(m);                        
                    }
                },
                "sep1": "---------",
                "quit": {
                    name: "Quit",
                    icon: "quit",
                    callback: function (key, opt) {
                        console.log(key);
                    }
                    
                }
            }
            // there's more, have a look at the demos and docs...
        });
    });