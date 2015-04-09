<%@ Page Language="C#" AutoEventWireup="true" %>

<%
	string dir;
	if(Request.Form["dir"] == null || Request.Form["dir"].Length <= 0)
		dir = "/";
	else
		dir = Server.UrlDecode(Request.Form["dir"]);

    if (!dir.Contains(Server.MapPath("~")))
        dir = Server.MapPath("~") + dir;
        
   string folder = dir.Replace(Server.MapPath("~"), "");

	System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dir);
	Response.Write("<ul class=\"jqueryFileTree\" style=\"display: none;\">\n");
	foreach (System.IO.DirectoryInfo di_child in di.GetDirectories())
    {
        Response.Write("\t<li class=\"directory collapsed\"><input class=\"FTCB\" type=\"checkbox\" name='" + di_child.Name + "' onclick=\"checkFolderPermissions(this.id)\" id=\"" + folder + di_child.Name +
                    "\" rel=\"" + dir + "\"><a href=\"#\" name='" + di_child.Name + "' id=\"" + folder + di_child.Name + "\" rel=\"" + folder + di_child.Name + "/\">" + di_child.Name + "</a></li>\n");
    }  
	Response.Write("</ul>");   
 %>