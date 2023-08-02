﻿using BTYBApi.Repository.Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using BTYBApi.Repository.Lib.Security;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.SqlClient;
using BTYBApi.Data;
using System.Security.Claims;
using BTYBApi.Models.Project;
using BTYBApi.IRepository.Project;

namespace BTYBApi.Repository.Avigma
{
    public class Order_Master_Child_Data : IOrder_Master_Child_Data
    {
        Log log = new Log();
        SecurityHelper securityHelper = new SecurityHelper();
        ObjectConvert obj = new ObjectConvert();
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; }
        public Order_Master_Child_Data()
        {
        }
        public Order_Master_Child_Data(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("Conn_dBcon");
        }


        public IDbConnection Connection
        {
            get { return new SqlConnection(ConnectionString); }
        }


        public List<dynamic> AddUpdateOrder_Master_Child_Data(Order_Master_Child_DTO model)
        {
            string msg = string.Empty;

            List<dynamic> objData = new List<dynamic>();

            using (IDbConnection con = Connection)
            {
                if (Connection.State == ConnectionState.Closed) con.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("CreateUpdate_Order_Master_Child", (SqlConnection)con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ORD_PKeyID", model.ORD_PKeyID);
                    cmd.Parameters.AddWithValue("@ORD_UP_PkeyID", model.ORD_UP_PkeyID);
                    cmd.Parameters.AddWithValue("@ORD_Pro_PkeyID", model.ORD_Pro_PkeyID);
                    //cmd.Parameters.AddWithValue("@ORD_UP_User_PkeyID", model.ORD_UP_User_PkeyID);
                    //cmd.Parameters.AddWithValue("@ORD_UP_Purchase_PkeyID", model.ORD_UP_Purchase_PkeyID);
                    cmd.Parameters.AddWithValue("@ORD_ORDM_PKeyID", model.ORD_ORDM_PKeyID);

                    cmd.Parameters.AddWithValue("@ORD_No_IP", model.ORD_No_IP);
                    cmd.Parameters.AddWithValue("@ORD_No_Stripe_ProductID", model.ORD_No_Stripe_ProductID);
                    cmd.Parameters.AddWithValue("@ORD_No_Stripe_PriceID", model.ORD_No_Stripe_PriceID);
                    cmd.Parameters.AddWithValue("@ORD_No_Stripe_UserID", model.ORD_No_Stripe_UserID);
                    cmd.Parameters.AddWithValue("@ORD_Net_Amount", model.ORD_Net_Amount);
                    cmd.Parameters.AddWithValue("@ORD_IsStatus", model.ORD_IsStatus);

                    cmd.Parameters.AddWithValue("@ORD_IsActive", model.ORD_IsActive);
                    cmd.Parameters.AddWithValue("@ORD_IsDelete", model.ORD_IsDelete);
                    cmd.Parameters.AddWithValue("@Type", model.Type);
                    cmd.Parameters.AddWithValue("@UserID", model.UserID);
                    //cmd.Parameters.AddWithValue("@ORD_Pkey_Out", 0).Direction = ParameterDirection.Output;
                    //cmd.Parameters.AddWithValue("@ReturnValue", 0).Direction = ParameterDirection.Output;

                    SqlParameter ORD_Pkey_Out = cmd.Parameters.AddWithValue("@ORD_Pkey_Out", 0);
                    ORD_Pkey_Out.Direction = ParameterDirection.Output;
                    SqlParameter ReturnValue = cmd.Parameters.AddWithValue("@ReturnValue", 0);
                    ReturnValue.Direction = ParameterDirection.Output;


                    cmd.ExecuteNonQuery();
                    objData.Add(ORD_Pkey_Out.Value);
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

        private DataSet Get_UserMaster(Order_Master_Child_DTO_Input model)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Get_Order_Master_Child", (SqlConnection)Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ORD_PkeyID", model.ORD_PkeyID);
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


        public List<dynamic> Get_Order_Master_ChildDetailsDTO(Order_Master_Child_DTO_Input model)
        {
            string msg = string.Empty;
            List<dynamic> objDynamic = new List<dynamic>();
            try
            {


                DataSet ds = Get_UserMaster(model);
                if (ds.Tables[0].Rows.Count > 0)
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