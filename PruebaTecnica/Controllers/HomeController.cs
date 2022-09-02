using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PruebaTecnica.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public JsonResult Validar(string user, string contrasena)
        {
            if (user == "admin" && contrasena == "admin")            
                return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);            
            else
                return Json(new { result = "ERROR" }, JsonRequestBehavior.AllowGet);
            
        }
        public JsonResult getusuarios()
        {
            try
            {
                var parameters = new[] {
                                new SqlParameter("@IDBASE", SqlDbType.Int) {Value = 1 },
                                new SqlParameter("@STATUS", SqlDbType.Int) {Value = 2},
                            };

                var ds = DtSql(ConnectSql(), parameters, "DetailsEmailsSent");
                var Lista = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new
                    {
                        idLogEnvioCorreo = dataRow.Field<int>("idLogEnvioCorreo"),
                        CodEmpleado = dataRow.Field<int>("CodEmpleado"),
                        Nombre = dataRow.Field<string>("Nombre"),
                        motivo = dataRow.Field<string>("motivo"),
                        Correo = dataRow.Field<string>("Correo")
                    }).ToList();
                
                return Json(new { result = Lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {

                return Json(new { result = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public SqlConnectionStringBuilder ConnectSql(Configuration configuration = null, string Base = "")
        {
            try
            {
                //              "ConnectionStrings": {
                //                  "DBContext": "Server=10.177.0.18;Database=NominaAtento;User Id=nomina;Password=nomina01;MultipleActiveResultSets=true"

                //},                //return new SqlConnectionStringBuilder(configuration.GetConnectionString(:));

                return new SqlConnectionStringBuilder("Server=10.177.0.18;Database=NominaAtento;User Id=nomina;Password=nomina01;MultipleActiveResultSets=true");
            }
            catch (Exception ex)
            {
                return new SqlConnectionStringBuilder();                
            }

        }





        public DataSet DtSql(SqlConnectionStringBuilder builder, Array parameters, string queryName = "")
        {
            List<Dictionary<String, Object>> items = new List<Dictionary<String, Object>>();
            SqlConnection connection = new SqlConnection(builder.ConnectionString);

            try
            {
                DataSet ds = new DataSet();
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using (SqlCommand command = new SqlCommand(queryName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 0;
                    if (parameters != null)
                    {
                        foreach (SqlParameter sqlParameter in parameters)
                        {
                            command.Parameters.Add(sqlParameter);
                        }
                    }


                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        using (DataSet dataSet = new DataSet())
                        {
                            adapter.Fill(dataSet);
                            command.Dispose();
                            ds = dataSet;
                        }
                        adapter.Dispose();
                    }

                }

                return ds;
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw;
                //return new List<Dictionary<String, Object>>();
            }
            finally
            {

                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }



    }
}

public class CD_Conexion
{
    private SqlConnection Conexion = new SqlConnection("Server=DESKTOP-UEPK13H\\RONETJOHN;DataBase= Practica;Integrated Security=true");
    public SqlConnection AbrirConexion()
    {
        if (Conexion.State == ConnectionState.Closed)
            Conexion.Open();
        return Conexion;
    }
    public SqlConnection CerrarConexion()
    {
        if (Conexion.State == ConnectionState.Open)
            Conexion.Close();
        return Conexion;
    }
}