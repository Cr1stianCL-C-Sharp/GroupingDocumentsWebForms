using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Configuration;

namespace GroupingDocumentsWeb.clases
{
    public class DataAccess
    {        
            #region Variables y propiedades

            public static String StringConnex = String.Empty;
            private static String cadena = String.Empty;
            
            public SqlConnection oConnection { get; set; }

            public SqlCommand oCommand { get; set; }

            public IDataReader Lector { get; set; }

        //public SqlDataReader Lector { get; set; }

        #endregion

        #region Constructores

        public DataAccess()
            {
            //   StringConnex = ConfigurationManager.ConnectionStrings["StringConnection"].ConnectionString;
            StringConnex = "Data Source=200.29.3.142,1038;Initial Catalog=BD_COBRANZA;User ID=acfcapital;Password=acfcapital";
            //StringConnex = @"Data Source=DESKTOP-BVN4UMD\SQLEXPRESS;Initial Catalog=Inventory;User ID=user;Password=admin";
            oConnection = new SqlConnection();
                oCommand = new SqlCommand();
            }

            public void Open()
            {
                if (oConnection.State == ConnectionState.Open)
                {
                    return;
                }

                oConnection.ConnectionString = StringConnex;

                try
                {
                    oConnection.Open();

                }
                catch
                {
                    throw;
                }
            }

        public String ConexString()
        {
            try
            {
                return StringConnex;
            }
            catch
            {
                throw;
            }

       }
            
       

        public void Close()
            {
                if (oConnection.State == ConnectionState.Closed)
                {
                    return;
                }

                oConnection.Close();

            }

            #endregion

            #region Metodos
            public Int32 ExecuteNonQuery(CommandType tipoComando, String Query)
            {////sirve para ejecuatar sentencias Update y Delete
                oCommand.Connection = oConnection;
                oCommand.CommandType = tipoComando;
                oCommand.CommandText = Query;
                int retorno = 0;

                try
                {
                    retorno = oCommand.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }

                return retorno;
            }

        public Int32 ExecuteNonQuery(CommandType tipoComando)
        {////sirve para ejecuatar sentencias Update y Delete
            oCommand.Connection = oConnection;
            oCommand.CommandType = tipoComando;
            //oCommand.CommandText = Query;
            int retorno = 0;

            try
            {
                retorno = oCommand.ExecuteNonQuery();
                
            }
            catch
            {                
                throw;  
            }

            return retorno;

        }

        public IDataReader ExecuteReader(CommandType tipoComando, String Query)
            { ////Sirve para realizar sentencias Select.

                Lector = null;
                oCommand.Connection = oConnection;
                oCommand.CommandType = tipoComando;
                oCommand.CommandText = Query;

                try
                {
                    Lector = oCommand.ExecuteReader();

                //while (Lector.Read())
                //{

                //}

                }
                catch
                {
                    throw;
                }
                return Lector;
            }

            #endregion
        
    }
}
