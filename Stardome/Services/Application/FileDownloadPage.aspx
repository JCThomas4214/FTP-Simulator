<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%
    
    if (Convert.ToInt32(Request.QueryString["Mode"])==1)
    {
        Response.ContentType = "audio/mp3";
        string FilePath = (Request.QueryString["filePath"]);
        string[] temp = FilePath.Split('\\');
        Response.AddHeader("Content-Disposition", "attachment;filename=" + temp[temp.GetUpperBound(0)]);
        Response.WriteFile(FilePath);
        Response.End();
    }
    else if (Convert.ToInt32(Request.QueryString["Mode"]) == 2)
    {
       string[] selectedFiles = Request["selectedFiles"].Split(',');
       string zipFileName = "StardomeContents.zip";
       Response.ContentType = "application/octet-stream";
        
       Response.AddHeader("content-disposition", "attachment; fileName=" + zipFileName);
       using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
       {
           foreach (string filePath in selectedFiles.ToList())
           {
               zip.AddFile(filePath);
           }
           zip.Save(Response.OutputStream);
           Response.End();
          

       }
    }
 
%>