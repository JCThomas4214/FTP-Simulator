<%@ Page Language="C#" AutoEventWireup="true" %>

<%
	//
	// jQuery File Tree ASP Connector
	//
	// Version 1.0
	//
	// Copyright (c)2008 Andrew Sweeny
	// asweeny@fit.edu
	// 24 March 2008
	//  
	string dir;
	if(Request.Form["dir"] == null || Request.Form["dir"].Length <= 0)
		dir = "/";
	else
		dir = Server.UrlDecode(Request.Form["dir"]);
	System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dir);
	Response.Write("<ul class=\"jqueryFileTree\" style=\"display: none;\">\n");
	foreach (System.IO.DirectoryInfo di_child in di.GetDirectories())
    {
        Response.Write("\t<li id=\"folder\" class=\"directory collapsed\"><a href=\"#\" rel=\"" + dir + di_child.Name + "/\">" + di_child.Name + "</a></li>\n");
    }
    //Response.Write("</ul>");
    //Response.Write("<ul id=\"file\" class=\"jqueryFileTree\" style=\"display: none;\">\n");

    
	foreach (System.IO.FileInfo fi in di.GetFiles())
	{
		string ext = ""; 
		if(fi.Extension.Length > 1)
			ext = fi.Extension.Substring(1).ToLower();
        string filePath = dir.Remove(0, Server.MapPath("~").Length);
        filePath = "/" + filePath.Remove(filePath.Length - 1) + "\\" + fi.Name;
        
        Response.Write("\t<li id=\"file\" class=\"file ext_" + ext + "\"><input class=\"FTCB\" type=\"checkbox\" onclick=\"checkB(this.id)\" id=\"" + dir + fi.Name + 
                    "\" rel=\"" + dir + fi.Name + "\"><a href=\"#\" rel=\"" + dir + fi.Name + "\">" + fi.Name +
                    "</a>&nbsp;&nbsp;<a href='#' ><img src='\\Images\\Play.png' Title='Play the file' id='" + filePath + "' style='width:15px;Height:15px' onclick='aud_play_pause(this.id);' ></a>" + 
                    "&nbsp;<img id=\"FTbutton\" src='\\Images\\Delete.png' Title='Delete file' style='width:15px;Height:15px'>&nbsp;"+
                    "<a href='#' >" +
                    "<img src='\\Images\\download.ico' Title='Download the file' style='width:15px;Height:15px' id='" + filePath + "' onclick ='downloadMP3(this.id);'/></a></li>" +
                        "\n");
	}
	Response.Write("</ul>");
    Response.Write("<audio id='MP3Player'> <source src='' type='audio/mp3'></audio>");
 %>