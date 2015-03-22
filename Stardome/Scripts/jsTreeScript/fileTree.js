		var fileList = new Array;
		var htmlList = new Array;
		var root = "C:/Users/";
		var t = new String;

		function call(number) {
		    for (var i = 0; i < fileList.length; i++) {		        
		        try{check(fileList[i]);}
                catch(e){console.log(fileList[i] + "is not in the current file tree")}
		    }
		}

		function check(box) {
		    //console.log(box);
            document.getElementById(box).checked = true;            
        }

        function uncheck(box) {
            //console.log(box);
            document.getElementById(box).checked = false;           
        }

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
            window.open("/Services/Application/FileDownloadPage.aspx?FilePath=" + filePath); //TODO: Change the location for Filedownloadpage.aspx
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
            htmlList.push('<li  id=\"' + number + '\" name=\"' + file + '\">' + file + '<img src="\\Images\\Delete.png" id=\"' + number + '\" onclick=\"deleteItem(this.id)\"></li>');
            fileList.push(file);
            check(fileList[number]);
            console.log(fileList);
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
            console.log(fileList);
            document.getElementById('selectedFileList').innerHTML = htmlList.join("");
        }

        function dropD() {
            console.log(subList[0][1]);
            htmlDDlist.push('<div class="dropdown"><button class=\"btn btn-default dropdown-toggle\" type=\"button\" id=\"dropdownMenu1\" data-toggle=\"dropdown\" aria-expanded=\"true\">Dropdown<span class=\"caret\"></span></button>');
            htmlDDlist.push('<ul class=\"dropdown-menu\" role=\"menu\" aria-labelledby=\"dropdownMenu1\">');
            for (var i = 0; i < subList.length; i++) {
                htmlDDlist.push('<li role="presentation"><a role="menuitem" tabindex="-1" id=\"' + subList[i][0] + '\" href="#">' + subList[i][1] + '</a></li>');
            }
            htmlDDlist.push('</div>');
            document.getElementById('dropD').innerHTML = htmlDDlist.join("");

            $(function () {
                $(".dropdown-menu li a").click(function () {
                    $(".btn:first-child").text($(this).text());
                    $(".btn:first-child").val($(this).text());
                    Tree($(this).attr('id'));

                    setTimeout(function () { call(); }, 100);
                });
            });
        }

        function Tree(root) {
            $('#MainTree').fileTree({
                root: root,
                script: '../Scripts/jqueryFileTree.aspx',
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
                console.log(dir);
            });
            $('.jqueryFileTree').contextMenu({
                // define which elements trigger this menu
                selector: "a.file",
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
                            window.console && console.log(m) || alert(m);
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

        $(document).ready(function () {
             dropD();
		     Tree(root);    
         });