<%@ Page Language="C#" AutoEventWireup="true" %>

<%
	string dir;
	if(Request.Form["dir"] == null || Request.Form["dir"].Length <= 0)
		dir = "/";
	else
		dir = Server.UrlDecode(Request.Form["dir"]);
	System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dir);
	Response.Write("<ul class=\"jqueryFileTree\" style=\"display: none;\">\n");
	foreach (System.IO.DirectoryInfo di_child in di.GetDirectories())
    {
        Response.Write("\t<li id=\"folder\" class=\"directory collapsed\"><input class=\"FTCB\" type=\"checkbox\" name='" + di_child.Name + "' onclick=\"checkFolderPermissions(this.id)\" id=\"" + dir + di_child.Name +
                    "\" rel=\"" + dir +  "\"><a href=\"#\" rel=\"" + dir + di_child.Name + "/\">" + di_child.Name + "</a></li>\n");
    }  

	Response.Write("</ul>");
   
 %>