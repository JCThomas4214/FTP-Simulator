		var fileList = new Array;
		var htmlList = new Array;
		var selectedUsers = new Array;
		var selectedFolders = new Array;

		var root = "C:/Users/";
		var t = new String;
		var lastSelected = new String;		

		function initializer(Role) {
		    if (Role != 1) {        //1 = Admin, 2 = Producer, 3 = User
		        lastSelected = subList[0][0];
		        dropD();
		        Tree(subList[0][0],Role);
		    }
		    else {
		        lastSelected = root;
		        Tree(root, Role);
		    }
		}

		function initializer_Permissions(Role) {
		        lastSelected = root;
		        Tree_Permissions(root);
		}
		
		function call(number) {
		    for (var i = 0; i < fileList.length; i++) {		        
		        try{check(fileList[i]);}
                catch(e){console.log(fileList[i] + "is not in the current file tree")}
		    }
		}

		function check(box) {		    
            document.getElementById(box).checked = true;            
        }

        function uncheck(box) {            
            document.getElementById(box).checked = false;           
        }

        function checkFolderPermissions(dir)
        {
            debugger
            if (document.getElementById(dir).checked == true) {                         // Folder Checked
              
                var index = selectedFolders.indexOf(dir);                               // Folder dosner exisit in the list
                if (index == -1)
                {
                    
                    // Removing the sub folders from list--may not remove all if the folder is not expanded
                    FolderName = document.getElementById(dir);
                    var subFolders = document.querySelectorAll('[id*=' + FolderName.name + ']');
                    for (x = 0 ; x < subFolders.length ; x++) {
                        subFolderId = subFolders[x].getAttribute("id");
                        var subFolderIndex = selectedFolders.indexOf(subFolderId);
                        if(subFolderIndex > -1)
                            selectedFolders.splice(subFolderIndex, 1);

                    }
                    selectedFolders.push(dir)       // Adding Selected folder to list
                    document.getElementById('selectedFoldersList').innerHTML = selectedFolders.join("");
                }
                
            }
            else {                                                                  //Folder unchecked
               
                var ParentFolder;
                do                                                                 // Uncheck all the parent folders recursively
                {
                    var index = selectedFolders.indexOf(dir);
                    // Remove the unchecked folders from list
                    if (index > -1)
                    {
                        FolderName = document.getElementById(dir);
                        var subFolders = document.querySelectorAll('[id*=' + FolderName.name +']');
                        for (x = 0 ; x < subFolders.length ; x++) {
                            subFolderId = subFolders[x].getAttribute("id");
                            var SubFolderCheckBox = document.getElementById(subFolderId);
                            if (SubFolderCheckBox.checked == true && selectedFolders.indexOf(subFolderId) == -1) {
                                selectedFolders.push(subFolderId)
                            }
                        }
                        selectedFolders.splice(index,1);

                    }
                    
                    // uncheck the parent folder
                    var checkBox = document.getElementById(dir);
                    folderName = checkBox.name;
                    ParentFolder = checkBox.id.replace("/" + folderName, "");
                    var ParentCheckBox = document.getElementById(ParentFolder);
                    ParentCheckBox.checked = false;
                    dir = ParentFolder;
                }
                while (ParentFolder.toString().endsWith("Stardome") == false)       // Repeat untill it reaches Stardome folder

                }
            
        }

        function updatePermissions()
        {
            UserId = $('#ddlUsers').val();
            if (UserId >0)
            {
                $.ajax({
                    url: "/Manage/UpdateFolderPermissions",
                    type: "POST",
                    data: JSON.stringify({ UserId: UserId, SelectedFolders: selectedFolders }),
                    dataType: 'json',
                    contentType: 'application/json',
                    error: function (xhr) {
                        alert('Error: ' + xhr.statusText);
                    },
                    success: function (result) {
                    },
                    async: true,
                    processData: false
                });
            }

        }

        String.prototype.endsWith = function(suffix) {
            return this.indexOf(suffix, this.length - suffix.length) !== -1;
        };
        function checkB(file) {           
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
                if (fileList[i] == file) {
                    found = true;                    
                    break;
                }                
            }
            items[0] = found;
            items[1] = i;            
            return items;
        }

        function downloadMP3(filePath) {
            window.open("/Services/Application/FileDownloadPage.aspx?Mode=1&FilePath=" + filePath); //TODO: Change the location for Filedownloadpage.aspx
        }

        function aud_play_pause(file) {
            var myAudio = document.getElementById('MP3Player');
            if (myAudio.paused) {
                myAudio.src = file;
                myAudio.load();
                myAudio.play();
                document.getElementById(file).src = "/Images/Pause.png";
            } else {
                myAudio.pause();
                document.getElementById(file).src = "/Images/play.png";
            }
        }

        function downloadAsZip()
        {
            if (fileList.length>0) {
                $('<form method="post" action="/Services/Application/FileDownloadPage.aspx?Mode=2" ><input type="hidden" name="selectedFiles" value="' + fileList + '">' +
                    '</form>').submit();
            }
        }

        function deleteFile(file)
        {

            $("#dialog-confirm").html("This will delete the file permanently. Are you sure?");
            // Define the Dialog and its properties.
            $("#dialog-confirm").dialog({
                resizable: false,
                modal: true,
                title: "Are you sure?",
                height: 150,
                width: 400,
                buttons: {
                    "Yes": function () {
                        $(this).dialog('close');
                        callback(true, file);
                    },
                    "No": function () {
                        $(this).dialog('close');
                        callback(false, file);
                    }
                }
            });
        }

        function callback(value, filePath) {
            if (value) {
                $.ajax({
                    url: "/Manage/DeleteFile/?filePath=" + filePath,
                    type: "POST",
                    data: { filePath: filePath },
                    error: function (xhr) {
                        alert('Error: ' + xhr.statusText);
                    },
                    success: function (result) {
                    },
                    async: true,
                    processData: false
                });


            } else {
                //Clicked No; do Nothing
            }
        }

        function insertItem(number, file) {
            var split0 = file.split("\\");           
            var split1 = split0[split0.length - 1].split("\/");           
            htmlList.push('<li  id=\"' + number + '\" name=\"' + file + '\">' + split1[split1.length-1] + '<img src="\\Images\\Delete.png" id=\"' + number + '\" onclick=\"deleteItem(this.id)\"></li>');
            fileList.push(file);
            try{check(fileList[number]);}
            catch(e){}
            document.getElementById('selectedFileList').innerHTML = htmlList.join("");

        }

        function deleteItem(number) {
            var tmp = number;            
            htmlList.splice(number, 1);

            try { uncheck(fileList[number]); }
            catch (e) { }

            fileList.splice(number, 1);
            for (var i = number; i < htmlList.length; i++) {                      
                tmp++;                    
                htmlList[i] = htmlList[i].replace('id=\"' + tmp + '\"', 'id=\"' + i + '\"')
                htmlList[i] = htmlList[i].replace('id=\"' + tmp + '\"', 'id=\"' + i + '\"')
            }            
            document.getElementById('selectedFileList').innerHTML = htmlList.join("");
        }

        function dropD() {           
            htmlDDlist.push('<div class="dropdown"><button class=\"btn btn-default dropdown-toggle\" type=\"button\" id=\"dropdownMenu1\" data-toggle=\"dropdown\" aria-expanded=\"true\">'+subList[0][1]+'<span class=\"caret\"></span></button>');
            htmlDDlist.push('<ul class=\"dropdown-menu\" role=\"menu\" aria-labelledby=\"dropdownMenu1\">');
            for (var i = 0; i < subList.length; i++) {
                htmlDDlist.push('<li role="presentation"><a role="menuitem" tabindex="-1" id=\"' + subList[i][0] + '\" href="#">' + subList[i][1] + '</a></li>');
            }
            htmlDDlist.push('</div>');
            document.getElementById('dropD').innerHTML = htmlDDlist.join("");

            $(function () {
                $(".dropdown-menu li a").click(function () {    //this is the on click function for the dropdown
                    $(".btn:first-child").text($(this).text());
                    $(".btn:first-child").val($(this).text());
                    Tree($(this).attr('id'));                   //reinitialization of the filetree based off what was clicked                                                                
                    setTimeout(function () { call(); }, 100);   //call() keeps checkboxes consistant with what's in the download list. Using 100ms delay to wait for the DOM
                    lastSelected = $(this).attr('id');
                    //TODO: when a new folder is selected change the upload path to selected root "$(this).attr('id')"
                });
            });
        }

        function Tree(root, Role) {
            $('#MainTree').fileTree({
                root: root,
                role: Role,
                script: '../Scripts/jqueryFileTree.aspx?Role='+Role,
                multiFolder: false,
                folderEvent: 'dblclick'
            }, function (file) {                
                var items = new Array; //array that contrains check outputs
                items = checkVal(file);    //check will return, true or false and position if the file is already in the list
                if (items[0] == true) {
                    deleteItem(items[1]); //deleteItem will delete an object in the list at position item[1]                   
                }
                else {                    
                    insertItem(items[1], file); //insertItem will insert an object into a list with value 'file' and postiion item[1]                    
                }
            }, function (dir) {
                lastSelected = dir;
            });
            $('.jqueryFileTree').contextMenu({
                // define which elements trigger this menu
                selector: "a",
                // define the elements of the menu

                items: {
                    download: {
                        name: "Download",
                        icon: "copy",
                        callback: function (key, opt) {

                        },
                        disabled: function (key, opt) {
                            if ($(this).parent().attr('id') == "folder") {
                                return true;
                            }
                            else
                                return false;
                        }
                    },
                    "download list": {
                        name: "Add/Remove file",
                        icon: "edit",
                        callback: function (key, opt) {
                            var items = new Array;
                            items = checkVal($(this).attr('rel'));
                            if (items[0] == true) {
                                deleteItem(items[1]);
                                //uncheck($(this).attr('rel'));
                            }
                            else {
                                insertItem(items[1], $(this).attr('rel'));
                                //check($(this).attr('rel'));
                            }
                        },
                        disabled: function (key, opt) {
                            if ($(this).parent().attr('id') == "folder") {
                                return true;
                            }
                            else
                                return false;
                        }
                    },
                    "upload": {
                        name: "Upload to",
                        icon: "add",
                        callback: function (key, opt) {                            
                            var m = "clicked: " + key + " on " + $(this).attr('rel');
                            //window.console && console.log(m) || alert(m);
                            lastSelected = m;
                        },
                        disabled: function (key, opt) {
                            if ($(this).parent().attr('id') == "file") {
                                return true;
                            }
                            else
                                return false;
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
            
            
            
        }

        
        function Tree_Permissions(root) {
            $('#MainTree').fileTree({
                root: root,
                script: '../Scripts/jqueryPermissionsFileTree.aspx',                
                multiFolder: false,
                folderEvent: 'dblclick'
            }, function (file) {
                var items = new Array; //array that contrains check outputs
                items = checkVal(file);    //check will return, true or false and position if the file is already in the list
                if (items[0] == true) {
                    deleteItem(items[1]); //deleteItem will delete an object in the list at position item[1]                   
                }
                else {
                    insertItem(items[1], file); //insertItem will insert an object into a list with value 'file' and postiion item[1]                    
                }
            }, function (dir) {
                lastSelected = dir;
            });
           


        }