using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GroupingDocumentsWeb.clases;
using System.IO;
using System.Net;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GroupingDocumentsWeb.clases
{
    public class Functions
    {
        DateTime Today = DateTime.Now;    

        protected internal String CreateFolderUpload(string route)
        {
            try
            {                             
                //string UploadPath = ("~/uploads"); //+ SToday;              
                //System.Globalization.CultureInfo cur = new
                //             System.Globalization.CultureInfo("en-US");
                //string SaveDate = Today.ToString("yyyyMMddHHmmss", cur);
                string FinalSave = String.Format("{0}", route);
                if (ScanFilePath(FinalSave))
                {
                    return FinalSave;
                }else { return ""; }

                ////System.Globalization.CultureInfo cur = new
                ////            System.Globalization.CultureInfo("en-US");
                ////string SaveDate = Today.ToString("yyyyMMddHHmmss", cur);
                ////string FinalSave = String.Format("{0}\\{1}\\{2}", UploadPath, SaveDate, filename);
                ////if (ScanFilePath(FinalSave))
                ////{
                ////    return FinalSave;
                ////}
                ////else { return ""; }

            }
            catch(Exception )
            {
                return "";
            }      
        }


        protected internal Boolean ScanFilePath(String path)
        {            
            try
            {                
                //String PathString = HttpContext.Current.Server.MapPath(rootPathUpload) + @"\" + path;
                String PathString = HttpContext.Current.Server.MapPath(path);
                if (Directory.Exists(PathString))
                {                    
                    return true;
                }
                else
                {                   
                    DirectoryInfo di = Directory.CreateDirectory(PathString);                   
                    return true;
                    // System.IO.Directory.CreateDirectory(path);                  
                }
            }
            catch (Exception)
            {
                return false;
            }
        }//termnino ScanFilePath

        protected internal String CreateFolderToSaveDocs(string filename)
        {
            try
            {               
                String SavedDocsPath = ("~/SavedDocs");
                String SaveDocxPath = String.Empty;
                //String FinalSave = String.Empty;
                System.Globalization.CultureInfo cur = new
                             System.Globalization.CultureInfo("en-US");
                string SaveDate = Today.ToString("yyyyMMddHHmmss", cur);
                //string FinalSave = String.Format("{0}\\{1}\\{2}", SavedDocsPath, SaveDate, filename);
                string FinalFolder = String.Format(@"{0}\{1}", SavedDocsPath, SaveDate);
                if (ScanFilePath(FinalFolder))
                {
                    //FinalSave = String.Format("{0}\\{1}", FinalFolder, filename);                    
                    return SaveDate;
                }
                else { return ""; }

            }
            catch (Exception)
            {
                return "";
            }
        }

        protected internal String AssignFolder()
        {
            try
            {
                String MainPath = ("~/Main");
                String SaveDocxPath = String.Empty;
                //String FinalSave = String.Empty;
                System.Globalization.CultureInfo cur = new
                             System.Globalization.CultureInfo("en-US");
                string DateFolder = Today.ToString("yyyyMMddHHmmss", cur);
                //string FinalSave = String.Format("{0}\\{1}\\{2}", SavedDocsPath, SaveDate, filename);
                string FinalFolder = String.Format("{0}\\{1}", MainPath, DateFolder);
                if (ScanFilePath(FinalFolder))
                {
                    //FinalSave = String.Format("{0}\\{1}", FinalFolder, filename);                    
                    return DateFolder;
                }
                else { return ""; }

            }
            catch (Exception)
            {
                return "";
            }
        }

        protected internal String GenerateName()
        {
            String Name = String.Empty;
            try
            {
                String Token = GenerateToken(8);
                System.Globalization.CultureInfo cur = new
                             System.Globalization.CultureInfo("en-US");
                String StringDate = Today.ToString("yyyyMMddHHmmss", cur);
                Name = String.Format("{0}\\{0}",Token,StringDate);

                return Name;
            }
            catch (Exception)
            {
                return "";
            }
        }


        public string GenerateToken(Byte length)
        {
            var bytes = new byte[length];
            var rnd = new Random();
            rnd.NextBytes(bytes);
            return Convert.ToBase64String(bytes).Replace("=", "").Replace("+", "").Replace("/", "");
        }


        //////////public Boolean SaveSoftwareData(List<List<String>> Software)
        //////////{
        //////////    DataAccess oDataAccess = new DataAccess();
        //////////    SqlCommand cmd = new SqlCommand();
        //////////    int SoftCount = Software.Count;
        //////////    int Softsubitm = Software[0].Count;
        //////////    int i = 0;

        //////////    try
        //////////    {
        //////////        SaveLogEvent(0, "Running", System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());///log

        //////////        using (SqlConnection Connex = new SqlConnection(oDataAccess.ConexString()))
        //////////        {
        //////////            if (SoftCount == 14) //19 datos trae el arreglo.
        //////////            {
        //////////                Connex.Open();

        //////////                for (i = 0; i < Softsubitm; i++)
        //////////                {
        //////////                    cmd = new SqlCommand("spi_inventory_software");
        //////////                    cmd.CommandType = CommandType.StoredProcedure;

        //////////                    cmd.Parameters.AddWithValue("@dc_machineName", machineName);
        //////////                    cmd.Parameters.AddWithValue("@dc_userName", userName);
        //////////                    cmd.Parameters.AddWithValue("@dg_Name", Software[0][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_Version", Software[1][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_Language", Software[2][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_Description", Software[3][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_Caption", Software[4][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_IdentifyingNumber", Software[5][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_InstallDate", Software[6][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_InstallLocation", Software[7][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_ProductID", Software[8][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_PackageName", Software[9][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_InstallSource", Software[10][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_RegOwner", Software[11][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_RegCompany", Software[12][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@dg_SKUNumber", Software[13][i].ToString());
        //////////                    cmd.Parameters.AddWithValue("@df_creationDate", ""); //fecha de creacion.                             

        //////////                    cmd.Connection = Connex;
        //////////                    cmd.ExecuteNonQuery();
        //////////                }
        //////////                Connex.Close();
        //////////                //int result = cmd.ExecuteNonQuery();
        //////////            }
        //////////            else
        //////////            {
        //////////                SaveLogEvent(1, "Faltan Datos, deberian venir: " + SoftCount.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());
        //////////            }

        //////////            SaveLogEvent(0, "Complete", System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()); ///log
        //////////            return true;
        //////////        } ////end using sql---


        //////////        //return true;
        //////////    }
        //////////    catch (Exception e)
        //////////    {
        //////////        SaveLogEvent(1, e.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());
        //////////        return false;
        //////////    }

        //////////}




    }
}