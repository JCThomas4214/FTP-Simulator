<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%

        Response.ContentType = "audio/mp3";
        string FilePath = (Request.QueryString["filePath"]);
        string[] temp = FilePath.Split('\\');
        Response.AddHeader("Content-Disposition", "attachment;filename=" + temp[temp.GetUpperBound(0)]);
        Response.WriteFile(FilePath);
        Response.End();
  
%>