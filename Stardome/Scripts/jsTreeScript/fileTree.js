		var fileList = new Array;
		var htmlList = new Array;
		var selectedUsers = new Array;
		var selectedFolders = new Array;
		var selectedFolderNames = new Array;

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
		    for (var i = 0; i < selectedFolders.length; i++) {
		        try { check(selectedFolders[i]); }
		        catch (e) { console.log(selectedFolders[i] + "is not in the current file tree") }
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
               if (document.getElementById(dir).checked == true) {                         // Folder Checked
              
                var index = selectedFolders.indexOf(dir);                               // Folder dosner exisit in the list
                if (index == -1)
                {
                    
                    // Removing the sub folders from list--may not remove all if the folder is not expanded
                    FolderName = document.getElementById(dir);
                    var subFolders = document.querySelectorAll('[id*=\'' + FolderName.name + '\']');
                    for (x = 0 ; x < subFolders.length ; x++) {
                        subFolderId = subFolders[x].getAttribute("id");
                        var subFolderIndex = selectedFolders.indexOf(subFolderId);
                        if (subFolderIndex > -1) {
                            selectedFolders.splice(subFolderIndex, 1);
                            selectedFolderNames.splice(subFolderIndex, 1)
                        }

                    }
                    selectedFolders.push(dir)       // Adding Selected folder to list
                    selectedFolderNames.push(FolderName.name)
                    document.getElementById('selectedFoldersList').innerHTML = selectedFolders.join("<br \>");
                }
                
            }
            else {                                                                  //Folder unchecked
               
                   var ParentFolder;
                   debugger
                do                                                                 // Uncheck all the parent folders recursively
                {
                    var index = selectedFolders.indexOf(dir);
                    // Remove the unchecked folders from list
                    if (index > -1)
                    {
                        FolderName = document.getElementById(dir);
                        var subFolders = document.querySelectorAll('[id*=\'' + FolderName.name +'\']');
                        for (x = 0 ; x < subFolders.length ; x++) {
                            subFolderId = subFolders[x].getAttribute("id");
                            var SubFolderCheckBox = document.getElementById(subFolderId);
                            if (SubFolderCheckBox.checked == true && selectedFolders.indexOf(subFolderId) == -1) {
                                selectedFolders.push(subFolderId)
                                selectedFolderNames.push(SubFolderCheckBox.name)
                            }
                        }
                        selectedFolders.splice(index, 1);
                        selectedFolderNames.splice(index, 1)

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
                    data: JSON.stringify({ UserId: UserId, SelectedFolders: selectedFolders, SelectedFolderNames: selectedFolderNames }),
                    dataType: 'json',
                    contentType: 'application/json',
                    error: function (xhr) {
                        //alert('Error: ' + xhr.statusText);
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

        function createFolder(Path) {           
            $("#dialog-confirm").html("<form><input type=\"text\" name=\"name\" id=\"txt2\" class=\"text ui-widget-content ui-corner-all\" /></form>");
            // Define the Dialog and its properties.
            $("#dialog-confirm").dialog({
                resizable: false,
                modal: true,
                title: "Name The Folder.",
                height: 167,
                width: 400,
                buttons: {
                    "Create": function () {
                        $(this).dialog('close');                        
                        var tmp = Path.substring(Path.indexOf("/"), Path.length);
                        while (tmp.search("/") != -1) {
                            tmp = tmp.replace("/", "\\");
                        }                        
                        callBackFolder($("#txt2").val(), tmp);                       
                    },
                    "Cancel": function () {
                        $(this).dialog('close');                       
                    }
                }
            });
        }

        function callBackFolder(Name, Path) {
            //var tmp = Path + "\\" + Name + "\\";
            $.ajax({
                url: "/Manage/CreateFolder/?Path=" + Path + "&Name=" + Name,
                type: "POST",
                data: {Path: Path, Name: Name },
                error: function (xhr) {
                   // alert('Error: ' + xhr.statusText);
                },
                success: function (result) {
                },
                async: true,
                processData: false
            });
            Tree(root, Role);
        }

        function deleteFolder(Name, Path) {
            $("#dialog-confirm").html("");
            // Define the Dialog and its properties.
            $("#dialog-confirm").dialog({
                resizable: false,
                modal: true,
                title: "Are you sure you want to delete this folder?",
                height: 167,
                width: 400,
                buttons: {
                    "Yes": function () {
                        $(this).dialog('close');
                        var tmp = Path.substring(Path.indexOf("/"), Path.length);
                        while (tmp.search("/") != -1) {
                            tmp = tmp.replace("/", "\\");
                        }                        
                        CBdelFolder(Name, tmp);                       
                    },
                    "No": function () {
                        $(this).dialog('close');
                    }
                }
            });
        }

        function CBdelFolder(Name, Path) {            
            $.ajax({
                url: "/Manage/DeleteFolder/?Path=" + Path + "&Name=" + Name,
                type: "POST",
                data: { Path: Path, Name: Name },
                error: function (xhr) {
                    alert('Error: ' + xhr.statusText);
                },
                success: function (result) {
                },
                async: true,
                processData: false
            });
            Tree(root, Role);
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
                height: 160,
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
                        //alert('Error: ' + xhr.statusText);
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
                    Tree($(this).attr('id'), Role);                   //reinitialization of the filetree based off what was clicked                                                                
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
            if (Role == 1) {
                $('.jqueryFileTree').contextMenu({
                    // define which elements trigger this menu
                    selector: "a",
                    // define the elements of the menu

                    items: {
                        "create folder": {
                            name: "Create Folder",
                            icon: "createFolder",
                            callback: function (key, opt) {
                                createFolder($(this).attr("rel"));
                            },
                            disabled: function (key, opt) {
                                if (($(this).parent().attr('id') == "file") || Role != 1) {
                                    return true;
                                }
                                else
                                    return false;
                            }
                        },
                        "delete folder": {
                            name: "Delete Folder",
                            icon: "deleteFolder",
                            callback: function (key, opt) {
                                deleteFolder($(this).attr("name"), $(this).attr("rel"));
                            },
                            disabled: function (key, opt) {
                                if (($(this).parent().attr('id') == "file") || Role != 1) {
                                    return true;
                                }
                                else
                                    return false;
                            }
                        },
                    }
                    // there's more, have a look at the demos and docs...
                });
            }
            
            
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
                    //deleteItem(items[1]); //deleteItem will delete an object in the list at position item[1]                   
                }
                else {
                    //insertItem(items[1], file); //insertItem will insert an object into a list with value 'file' and postiion item[1]                    
                }
            }, function (dir) {
                lastSelected = dir;
            });
            $('.jqueryPermissionsFileTree').contextMenu({
                // define which elements trigger this menu
                selector: "a",
                // define the elements of the menu

                items: {
                    "AddUsers": {
                        name: "Add Users to this folder",
                        icon: "AddUsers",
                        callback: function (key, opt) {
                            AddUsers($(this).attr('id'), $(this).attr('name'));
                        }                        
                    }


                }
                // there's more, have a look at the demos and docs...
            });


        }

        function AddUsers(folderId, folderName)
        {
            $("#AddUserstoFolder").dialog({
                open: function (event, ui) {
                    $.ajax({
                        url: "/Manage/GetUserPermissionsForFolder",
                        type: "POST",
                        data: JSON.stringify({ folderName: folderName }),
                        dataType: 'json',
                        contentType: 'application/json',
                        error: function (xhr) {
                        },
                        success: function (result) {
                            activeUsers = document.getElementsByTagName('input');
                            for (var i = 0; i < activeUsers.length; i++) {
                                if (activeUsers[i].getAttribute('type') == 'checkbox' && activeUsers[i].getAttribute('name')=='chkUserId')
                                    activeUsers[i].checked = false;
                            }
                            selectedUsers.length = 0;
                            selectedUsers = result.selectedUsers.slice();
                            for (var i = 0; i < selectedUsers.length; i++) {
                                document.getElementById(selectedUsers[i]).checked = true;
                            }
                        },
                        async: true,
                        processData: false
                    });

                },

                resizable: false,
                modal: true,
                title: "Select users to be added to this Folder",
                height: 300,
                width: 400,
                buttons: {
                    "Grant Permission": function () {
                        $(this).dialog('close');
                        grantPermissiontoFolder(folderId, folderName);
                    },
                    "Cancel": function () {
                        $(this).dialog('close');
                        
                    }
                }
                
            });
        }

        function addUsertoList(UserId)
        {
            if (document.getElementById(UserId).checked == true) {
                selectedUsers.push(UserId);
            }
            else {
                var index = selectedUsers.indexOf(UserId);                               // Folder dosner exisit in the list
                if (index > -1)
                    selectedUsers.splice(index, 1);
            }

            
        }

        function grantPermissiontoFolder(folderId, folderName)
        {
            $.ajax({
                url: "/Manage/GrantPermissionToFolder",
                type: "POST",
                data: JSON.stringify({ folderId: folderId, folderName: folderName, selectedUsers: selectedUsers }),
                dataType: 'json',
                contentType: 'application/json',
                error: function (xhr) {
                   // alert('Error: ' + xhr.statusText);
                },
                success: function (result) {
                },
                async: true,
                processData: false
            });

        }