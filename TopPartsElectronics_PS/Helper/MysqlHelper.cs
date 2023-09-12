// ===============================================================================
// Microsoft Data Access Application Block for .NET
// http://msdn.microsoft.com/library/en-us/dnbda/html/daab-rm.asp
//
// cs
//
// This file contains the implementations of the SqlHelper and SqlHelperParameterCache
// classes.
//
// For more information see the Data Access Application Block Implementation Overview. 
// ===============================================================================
// Release history
// VERSION	DESCRIPTION
//   2.0	Added support for FillDataset, UpdateDataset and "Param" helper methods
//
// ===============================================================================
// Copyright (C) 2000-2001 Microsoft Corporation
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;
using MySql.Data.MySqlClient;

using static TopPartsElectronics_PS.Helper.GeneralModelClass;

namespace YourApp.Data
{
    /// <summary>
    /// The SqlHelper class is intended to encapsulate high performance, scalable best practices for 
    /// common uses of SqlClient
    /// </summary>
    public class MysqlHelper
    {
      
        public static bool call_from_search_bom = false;
        public static bool call_from_search_material=false;
        public static bool call_from_search_client = false;
        public static bool call_from_productionInput_to_client = false;
        public static bool call_from_productionInput_to_item = false;
        public static bool call_from_shipping_to_client = false;
        public static bool call_from_shipping_to_item = false;
        public static bool call_from_ProductionStatus_to_client = false;
        public static bool call_from_ProductionStatus_to_item = false;
        public static bool call_from_lotinfomation_status_to_client = false;
        public static bool call_from_lotinfomation_status_to_item = false;
        public string get_user_roll = string.Empty;    
     
        private string mstr_ConnectionString;
        private MySqlConnection mobj_SqlConnection;
        private MySqlCommand mobj_SqlCommand;
        private int mint_CommandTimeout = 30;
        public static MySqlConnection mobj_SqlConnection_pub;
        public enum ExpectedType
        {

            StringType = 0,
            NumberType = 1,
            DateType = 2,
            BooleanType = 3,
            ImageType = 4
        }

        public MysqlHelper()
        {
            try
            {

                mstr_ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myDbconnect"].ToString();

                mobj_SqlConnection = new MySqlConnection(mstr_ConnectionString);
                mobj_SqlCommand = new MySqlCommand();
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.Connection = mobj_SqlConnection;
                mobj_SqlConnection_pub= mobj_SqlConnection;
               
                //ParseConnectionString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error initializing data class." + Environment.NewLine + ex.Message);
            }
        }

        public void Dispose()
        {
            try
            {
                //Clean Up Connection Object
                if (mobj_SqlConnection != null)
                {
                    if (mobj_SqlConnection.State != ConnectionState.Closed)
                    {
                        mobj_SqlConnection.Close();
                    }
                    mobj_SqlConnection.Dispose();
                }

                //Clean Up Command Object
                if (mobj_SqlCommand != null)
                {
                    mobj_SqlCommand.Dispose();
                }

            }

            catch (Exception ex)
            {
                throw new Exception("Error disposing data class." + Environment.NewLine + ex.Message);
            }

        }

        public void CloseConnection()
        {
            if (mobj_SqlConnection.State != ConnectionState.Closed) mobj_SqlConnection.Close();
        }
        public int GetExecuteScalarByCommand(string Command)
        {

            object identity = 0;
            try
            {
                mobj_SqlCommand.CommandText = Command;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                mobj_SqlConnection.Open();

                mobj_SqlCommand.Connection = mobj_SqlConnection;
                identity = mobj_SqlCommand.ExecuteScalar();
                CloseConnection();
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
            }
            return Convert.ToInt32(identity);
        }

        public void GetExecuteNonQueryByCommand(string Command, string Parametergp, string Parametereid, object gpval, object gpemail)
        {
            try
            {
                mobj_SqlCommand.CommandText = Command;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                mobj_SqlConnection.Open();
                AddParameterToSQLCommand(Parametergp, gpval);
                AddParameterToSQLCommand(Parametereid, gpemail);
                mobj_SqlCommand.Connection = mobj_SqlConnection;
                mobj_SqlCommand.ExecuteNonQuery();

                CloseConnection();
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
            }
        }
        //----------------------------------------------------------------------------------------------------------------
        public int ExecuteScalar(string Command, string[] Parametername, object[] Parameterval)
        {
            int r = 0;
            try
            {
                int param = Parametername.Length;

                mobj_SqlCommand.CommandText = Command;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                mobj_SqlConnection.Open();
                for (int i = 0; i < param; i++)
                {
                    AddParameterToSQLCommand(Parametername[i], Parameterval[i]);
                }
                mobj_SqlCommand.Connection = mobj_SqlConnection;
                r = Convert.ToInt32(mobj_SqlCommand.ExecuteScalar());
                return r;
                //CloseConnection();
                //CloseConnection();
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
                //return r;
            }
        }

        //__________________________________________________________________________________________________________________
        public void GetExecuteNonQueryByCommand(string Command, string[] Parametername, object[] Parameterval)
        {
            try
            {
                int param = Parametername.Length;

                mobj_SqlCommand.CommandText = Command;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                mobj_SqlConnection.Open();
                for (int i = 0; i < param; i++)
                {
                    AddParameterToSQLCommand(Parametername[i], Parameterval[i]);
                }
                mobj_SqlCommand.Connection = mobj_SqlConnection;
                mobj_SqlCommand.ExecuteNonQuery();

                CloseConnection();
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
            }
        }

        //----------------------------------------------------------------------------------------------------------------

        public DataSet GetDatasetByCommand(string Command)
        {
            try
            {
                mobj_SqlCommand.CommandText = Command;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                mobj_SqlConnection.Open();

                MySqlDataAdapter adpt = new MySqlDataAdapter(mobj_SqlCommand);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }


        public DataSet GetDatasetByCommand(string Command, string Parameter, object value)
        {
            try
            {
                mobj_SqlCommand.CommandText = Command;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;
                mobj_SqlCommand.Parameters.Clear();
                mobj_SqlConnection.Open();
                AddParameterToSQLCommand(Parameter, value);
                MySqlDataAdapter adpt = new MySqlDataAdapter(mobj_SqlCommand);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataSet GetDatasetByCommandString(string Command, string[] Parameter, object[] value)
        {
            try
            {
                int param = Parameter.Length;

                mobj_SqlCommand.CommandText = Command;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;
                mobj_SqlCommand.Parameters.Clear();
                mobj_SqlConnection.Open();
                for (int i = 0; i < param; i++)
                {
                    AddParameterToSQLCommand(Parameter[i], value[i]);
                }
                //AddParameterToSQLCommand(Parameter, value);
                MySqlDataAdapter adpt = new MySqlDataAdapter(mobj_SqlCommand);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public MySqlDataReader GetReaderBySQL(string strSQL)
        {
            mobj_SqlConnection.Open();
            try
            {
                MySqlCommand myCommand = new MySqlCommand(strSQL, mobj_SqlConnection);
                return myCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
            }
        }

        public MySqlDataReader GetReaderByCmd(string Command)
        {
            MySqlDataReader objSqlDataReader = null;
            try
            {
                mobj_SqlCommand.CommandText = Command;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;

                mobj_SqlConnection.Open();
                mobj_SqlCommand.Connection = mobj_SqlConnection;


                objSqlDataReader = mobj_SqlCommand.ExecuteReader();
                return objSqlDataReader;

            }
            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
            }

        }


        //----------------------------------------------------------------------------------


        public MySqlDataReader GetReaderByCmd(string Command, string[] Parametername, object[] Parameterval)
        {
            MySqlDataReader objSqlDataReader = null;
            int param = Parametername.Length;
            try
            {
                mobj_SqlCommand.CommandText = Command;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.Parameters.Clear();
                mobj_SqlConnection.Open();
                for (int i = 0; i < param; i++)
                {
                    AddParameterToSQLCommand(Parametername[i], Parameterval[i]);
                }
                mobj_SqlCommand.Connection = mobj_SqlConnection;


                objSqlDataReader = mobj_SqlCommand.ExecuteReader();
                return objSqlDataReader;
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
            }
        }
        //----------------------------------------------------------------------------------
        public MySqlDataReader GetReaderByCmd(string Command, string parametername, object value)
        {
            MySqlDataReader objSqlDataReader = null;
            try
            {
                mobj_SqlCommand.CommandText = Command;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;

                mobj_SqlConnection.Open();
                mobj_SqlCommand.Connection = mobj_SqlConnection;
                mobj_SqlCommand.Parameters.Clear();
                AddParameterToSQLCommand(parametername, value);
                objSqlDataReader = mobj_SqlCommand.ExecuteReader();
                //CloseConnection();
                return objSqlDataReader;
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw ex;
            }

        }

        //------------------------------------------------------------------------------------

        public void AddParameterToSQLCommand(string ParameterName, SqlDbType ParameterType)
        {
            try
            {
                mobj_SqlCommand.Parameters.Add(new MySqlParameter(ParameterName, ParameterType));
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddParameterToSQLCommand(string ParameterName, MySqlDbType ParameterType, int ParameterSize)
        {
            try
            {
                mobj_SqlCommand.Parameters.Add(new MySqlParameter(ParameterName, ParameterType, ParameterSize));
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        //------------------------------------


        public void AddParameterToSQLCommand(string ParameterName, object Value)
        {
            try
            {
                mobj_SqlCommand.Parameters.Add(new MySqlParameter(ParameterName, Value));
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        //-----------------------------------

        public void SetSQLCommandParameterValue(string ParameterName, object Value)
        {
            try
            {
                mobj_SqlCommand.Parameters[ParameterName].Value = Value;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }     
        public DataTable FetchDetails(string sp_name,string parameter_name,string[] paremeter_value)
        {
            if (mobj_SqlConnection_pub.State == ConnectionState.Closed)
            {
                mobj_SqlConnection_pub.Open();
            }
            DataTable dtData = new DataTable();
            mobj_SqlCommand = new MySqlCommand(sp_name, mobj_SqlConnection_pub);
            mobj_SqlCommand.CommandType = CommandType.StoredProcedure;
            mobj_SqlCommand.Parameters.AddWithValue(parameter_name, paremeter_value);
            MySqlDataAdapter sqlSda = new MySqlDataAdapter(mobj_SqlCommand);
            sqlSda.Fill(dtData);
            return dtData;
        }

        public DataSet GetDatasetByCommand(string p, string[] strarray1, string[] objarray1)
        {
            throw new NotImplementedException();
        }

        internal MySqlDataAdapter ExecuteQuery(string p, string[] str, string[] obj)
        {
            throw new NotImplementedException();
        }
        public DataTable GetDatasetByCommandString_dt(string Command, string[] Parameter, object[] value)
        {
            try
            {
                int param = Parameter.Length;

                mobj_SqlCommand.CommandText = Command;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;
                mobj_SqlCommand.Parameters.Clear();
                mobj_SqlConnection.Open();
                for (int i = 0; i < param; i++)
                {
                    AddParameterToSQLCommand(Parameter[i], value[i]);
                }
                //AddParameterToSQLCommand(Parameter, value);
                MySqlDataAdapter adpt = new MySqlDataAdapter(mobj_SqlCommand);
                DataTable ds = new DataTable();
                adpt.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
        //////////////// MGR Start
        public DataSet GetDatasetByClientcodeNames(string customercode, string customershortname)
        {
            DataSet ds = new DataSet();
            string ActionType = "GetDataSingle";
            string[] str = { "@idcust", "@customercd", "@fname", "@sname", "@created_at", "@updated_at", "@ActionType" };
            string[] obj = { "0", customercode, "", customershortname, "", "", ActionType };

            ds = GetDatasetByCommandString("customer_crud", str, obj);
            return ds;
        }
        public DataSet GetDatasetByMaterialcodeNames(string materialcode, string makercode)
        {
            DataSet ds = new DataSet();
            string ActionType = "GetDataSingle";
            string[] str = { "@idmat", "@materialcd", "@makercd", "@clasfy", "@price", "@fname",  "@created_at", "@updated_at", "@ActionType" };
            string[] obj = { "0", materialcode, makercode,"", "", "", "", "", ActionType };

            ds = GetDatasetByCommandString("material_crud", str, obj);
            return ds;
        }
        public DataSet GetDatasetByBOMView(string customercode,string shortname)
        {
            DataSet ds = new DataSet();
            string ActionType = "GetData";
            string[] str = { "@custcd", "@sname", "@ActionType" };
            string[] obj = { customercode, shortname, ActionType };

            ds = GetDatasetByCommandString("bom_view", str, obj);
            return ds;
        }
        public DataSet GetDatasetByBOMView_Item(string customercode, string shortname)
        {
            DataSet ds = new DataSet();
            string ActionType = "GetDataSingleitem";
            string[] str = { "@custcd", "@sname", "@ActionType" };
            string[] obj = { customercode, shortname, ActionType };

            ds = GetDatasetByCommandString("bom_view", str, obj);
            return ds;
        }
        public DataSet GetDatasetByBOMView_Pro_input(string customercode, string itemcode)
        {
            DataSet ds = new DataSet();
            string ActionType = "GetData";
            string[] str = { "@custcd", "@itemcd", "@ActionType" };
            string[] obj = { customercode, itemcode, ActionType };

            ds = GetDatasetByCommandString("Fetch_bom_view_cuscd_itemcd", str, obj);
            return ds;
        }
        public DataSet GetDatasetByBOMView_Pro_input_shipment(string customercode, string itemcode, string ActionType)
        {
            DataSet ds = new DataSet();           
            string[] str = { "@custcd", "@itemcd", "@ActionType" };
            string[] obj = { customercode, itemcode, ActionType };

            ds = GetDatasetByCommandString("Fetch_bom_view_cuscd_itemcd", str, obj);
            return ds;
        }
        public DataTable ProcessList()
        {
            string ActionType = "GetData";
            string[] str = { "@idproc", "@processcd", "@fname", "@sname", "@showord", "@inpscrtyp", "@created_at", "@updated_at", "@ActionType", "@inpscrtyp_id" };
            string[] obj = { "0", "", "", "", "", "", "", "", ActionType,"" };
            DataTable sdr = GetDatasetByCommandString_dt("process_crud", str, obj);
            return sdr;
        }
        public string Process_pattern_id(string processId)
        {
            string get_pattern_type = string.Empty;            
            string[] str = { "@processcd"};
            string[] obj = { processId };
            MySqlDataReader getPatternType = GetReaderByCmd("get_patterntype_processidvs", str, obj);
            if (getPatternType.Read())
            {
                get_pattern_type = getPatternType["inputscreentyp_id"].ToString();               
            }
            getPatternType.Close();
            CloseConnection();
            return get_pattern_type;
        }
       

    }
}
