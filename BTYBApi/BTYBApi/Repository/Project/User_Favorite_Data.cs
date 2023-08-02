﻿using BTYBApi.Repository.Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using BTYBApi.Repository.Lib.Security;
using BTYBApi.IRepository.Avigma;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.SqlClient;
using BTYBApi.Models.Avigma;
using BTYBApi.Data;
using System.Security.Claims;
using BTYBApi.Models.Project;
using BTYBApi.IRepository.Project;

namespace BTYBApi.Repository.Avigma
{
    public class User_Favorite_Data : IUser_Favorite_Data
    {
        Log log = new Log();
        SecurityHelper securityHelper = new SecurityHelper();
        ObjectConvert obj = new ObjectConvert();
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; }
        public User_Favorite_Data()
        {
        }
        public User_Favorite_Data(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("Conn_dBcon");
        }


        public IDbConnection Connection
        {
            get { return new SqlConnection(ConnectionString); }
        }


        public List<dynamic> AddUpdateUser_Favorite_Data(User_Favorite_DTO model)
        {
            string msg = string.Empty;

            List<dynamic> objData = new List<dynamic>();

            using (IDbConnection con = Connection)
            {
                if (Connection.State == ConnectionState.Closed) con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("CreateUpdate_User_Favorite", (SqlConnection)con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UF_Pkey", model.UF_Pkey);
                    cmd.Parameters.AddWithValue("@UF_User_PkeyID", model.UF_User_PkeyID);
                    cmd.Parameters.AddWithValue("@UF_UP_PKeyID", model.UF_UP_PKeyID);
                    cmd.Parameters.AddWithValue("@UF_Closet_Spotlight", model.UF_Closet_Spotlight);


                    cmd.Parameters.AddWithValue("@UF_IsActive", model.UF_IsActive);
                    cmd.Parameters.AddWithValue("@UF_IsDelete", model.UF_IsDelete);
                    cmd.Parameters.AddWithValue("@Type", model.Type);
                    cmd.Parameters.AddWithValue("@UserID", model.UserID);
                    //cmd.Parameters.AddWithValue("@UF_PkeyID_Out", 0).Direction = ParameterDirection.Output;
                    //cmd.Parameters.AddWithValue("@ReturnValue", 0).Direction = ParameterDirection.Output;

                    SqlParameter UF_PkeyID_Out = cmd.Parameters.AddWithValue("@UF_PkeyID_Out", 0);
                    UF_PkeyID_Out.Direction = ParameterDirection.Output;
                    SqlParameter ReturnValue = cmd.Parameters.AddWithValue("@ReturnValue", 0);
                    ReturnValue.Direction = ParameterDirection.Output;


                    cmd.ExecuteNonQuery();
                    objData.Add(UF_PkeyID_Out.Value);
                    objData.Add(ReturnValue.Value);

                }
                catch (Exception ex)
                {
                    log.logErrorMessage(ex.StackTrace);
                    log.logErrorMessage(ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return objData;
        }

        private DataSet Get_UserMaster(User_Favorite_DTO_Input model)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Get_User_Favorite", (SqlConnection)Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UF_PkeyID", model.UF_PkeyID);
                cmd.Parameters.AddWithValue("@UF_User_PkeyID", model.UF_User_PkeyID);
                cmd.Parameters.AddWithValue("@UF_UP_PKeyID", model.UF_UP_PKeyID);
                cmd.Parameters.AddWithValue("@UF_Closet_Spotlight", model.UF_Closet_Spotlight);

                cmd.Parameters.AddWithValue("@Type", model.Type);
                cmd.Parameters.AddWithValue("@UserID", model.UserID);
                cmd.Parameters.AddWithValue("@WhereClause", model.WhereClause);
                cmd.Parameters.AddWithValue("@PageNumber", model.PageNumber);
                cmd.Parameters.AddWithValue("@NoofRows", model.NoofRows);
                cmd.Parameters.AddWithValue("@Orderby", model.Orderby);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                log.logErrorMessage(ex.Message);
                log.logErrorMessage(ex.StackTrace);
            }
            return ds;

        }


        public List<dynamic> Get_User_FavoriteDetailsDTO(User_Favorite_DTO_Input model)
        {
            List<dynamic> objDynamic = new List<dynamic>(); try
            {


                DataSet ds = Get_UserMaster(model);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        objDynamic.Add(obj.AsDynamicEnumerable(ds.Tables[i]));
                    }

                }

            }
            catch (Exception ex)
            {
                log.logErrorMessage(ex.StackTrace);
                log.logErrorMessage(ex.Message);
            }

            return objDynamic;
        }

    }
}