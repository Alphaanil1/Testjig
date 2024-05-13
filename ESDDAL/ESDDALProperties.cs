using ESD_Testjig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESDBE;

namespace ESD_Testjig
{
   public class ESD_TestjigProperties
    {
        DBAccess ObjDbClass = new DBAccess();
        public string strMessage;
        public Boolean saved;
        

        public DataTable GetPCBType(int ProductTypeID)
        {
            DataTable dtPCB = new DataTable();
             SqlConnection con = new SqlConnection(DBAccess.getconstring());
           // SqlConnection con = new SqlConnection(GlobalInformation.ConnectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetPCBType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductTypeID", ProductTypeID);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtPCB);
                con.Close();                
            }
            catch (Exception)
            { }
            finally
            { }
            return dtPCB;
        }

        public DataTable GetProductTypes()
        {
            DataTable dtProducts = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetProductType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtProducts);
               
            }
            catch (Exception)
            { }
            finally
            { con.Close(); }
            return dtProducts;
        }

        public DataTable GetUserRole(int loginUserId)
        {
            DataTable dtUserRole = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetUserRoleByUserId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", loginUserId);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtUserRole);
                con.Close();
            }
            catch (Exception)
            { }
            finally
            { }
            return dtUserRole;
        }

        public DataTable Login(string UserName, string Password)
        {
            DataTable dtLogin = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetLoginDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", UserName);
                cmd.Parameters.AddWithValue("@Password", Password);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtLogin);
                con.Close();
            }
            catch 
            { }
            finally
            { }
            return dtLogin;
        }

        public DataTable GetUser()
        {
            DataTable dtRole = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetAllUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtRole);
                con.Close();
            }
            catch (Exception)
            { }
            finally
            { }
            return dtRole;
        }

        public DataTable GetUserRole()
        {
            DataTable dtRole = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetAlluserRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtRole);
                con.Close();
            }
            catch (Exception)
            { }
            finally
            { }
            return dtRole;
        }

        public DataTable RptGetPCBTypeWise(int pcbtypeid, string status, string Fromdate, string Todate, int LoginUserId)
        {
            DataTable dtrpt = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PCBTypeId", pcbtypeid);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@Todate", Todate);
                cmd.Parameters.AddWithValue("@CreatedBy", LoginUserId);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtrpt);
                con.Close();
            }
            catch (Exception)
            { }
            return dtrpt;
        }

        public DataTable GetPCBTestCases(int pcbTypeID, string TestType)
        {
            DataTable dtManual = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetPCBTestCases", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PCBTypeid", pcbTypeID);
                cmd.Parameters.AddWithValue("@TestType", TestType);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtManual);
                con.Close();
            }
            catch (Exception)
            { }
            finally
            { }
            return dtManual;
        }
        public DataTable GetRepeatTest(string repeatTest, int LoginUserId, string Fromdate, string Todate)
        {
            DataTable dtManual = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetRepeatTest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Teststatus", repeatTest);
                cmd.Parameters.AddWithValue("@Createdby", LoginUserId);
                cmd.Parameters.AddWithValue("@Fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@Todate", Todate);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtManual);
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { }
            return dtManual;
        }

        public DataTable RptGetPCBTypeWiseForExcel(int pcbtypeid, string status, string repeatTest, string fromdate, string todate, int loginUserId)
        {
            DataTable dtrpt = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PCBTypeId", pcbtypeid);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@FromDate", fromdate);
                cmd.Parameters.AddWithValue("@Todate", todate);
                cmd.Parameters.AddWithValue("@CreatedBy", loginUserId);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtrpt);
                con.Close();
            }
            catch (Exception)
            { }
            return dtrpt;
        }

        public DataSet GetRepeatTest1()
        {
            DataSet dtManual = new DataSet();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetRepeatTest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtManual);
                con.Close();
            }
            catch (Exception)
            { }
            finally
            { }
            return dtManual;
        }

        public DataSet RptGetPCBTypeWise1(int pcbtypeid, string status, string repeatTest, string fromdate, string todate, int loginUserId)
        {
            DataSet dtrpt = new DataSet();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PCBTypeId", pcbtypeid);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@FromDate", fromdate);
                cmd.Parameters.AddWithValue("@Todate", todate);
                cmd.Parameters.AddWithValue("@CreatedBy", loginUserId);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtrpt);
                con.Close();
            }
            catch (Exception)
            { }
            return dtrpt;
        }
        public DataTable GetSerialWiseData(int loginUserId, string serialNo, int pcbTypeId, string Fromdate, string Todate)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetDetailsBySerialNoWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CreatedBy", loginUserId);
                cmd.Parameters.AddWithValue("@SerialNo", serialNo);
                cmd.Parameters.AddWithValue("@PcbTypeID", pcbTypeId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@Todate", Todate);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dt);
                con.Close();
            }
            catch (Exception)
            { }
            finally
            { }
            return dt;
        }
        public DataTable CompareMinMaxValue(string current, int pcbtypeid, int testCaseId)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_CompareMinMaxValue", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Current", current);
                cmd.Parameters.AddWithValue("@PCBTypeId", pcbtypeid);
                cmd.Parameters.AddWithValue("@TestcaseId", testCaseId);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dt);
                con.Close();
            }
            catch (Exception)
            { }
            finally
            { }
            return dt;
        }

        public DataTable GetKeyBoardStart(int pcbtypeid)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetKeyboardStart", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Handler", "Keypad_Start");
                cmd.Parameters.AddWithValue("@PcbtypeID", pcbtypeid);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dt);
                con.Close();
            }
            catch (Exception)
            { }
            finally
            { }
            return dt;
        }

        //public string SaveSKHKeyboardTest(ESDBEProperties _ObjBE)
        //{
        //    try
        //    {
        //        int Status = 0;
        //        DataTable dtManual = new DataTable();
        //        SqlConnection con = new SqlConnection(DBAccess.getconstring());
        //        SqlCommand cmd = new SqlCommand();
        //        con.Open();
        //        cmd = new SqlCommand("usp_SaveSKHKeyboardTest", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@UserID", _ObjBE.PropUserID);
        //        cmd.Parameters.AddWithValue("@SerialNo", _ObjBE.PropSerialNo);
        //        cmd.Parameters.AddWithValue("@PCBTypeID", _ObjBE.PropPCBTypeID);
        //        cmd.Parameters.AddWithValue("@PCBType", _ObjBE.PropPCBType);
        //        cmd.Parameters.AddWithValue("@TestCaseID", _ObjBE.PropTestCaseID);
        //        cmd.Parameters.AddWithValue("@Status", _ObjBE.PropStatus);
        //        cmd.Parameters.AddWithValue("@FrameToSend", _ObjBE.PropFrameToSend);
        //        cmd.Parameters.AddWithValue("@ResponseFrame", _ObjBE.PropResponseFrame);
        //        cmd.Parameters.AddWithValue("@CreatedBy", _ObjBE.PropCreatedBy);
        //        //cmd.Parameters.AddWithValue("@Comment", _ObjBE.PropComment);
        //        cmd.Parameters.AddWithValue("@CurrentDate", _ObjBE.PropCurrentDateTime);
        //        Status = cmd.ExecuteNonQuery();
        //        con.Close();
        //        saved = true;
        //    }
        //    catch (Exception)
        //    {
        //        strMessage = "Data_save_error";
        //    }
        //    finally
        //    {
        //        if (saved)
        //        {
        //            strMessage = "Data_save_success";
        //        }
        //        else
        //        {
        //            strMessage = "Duplicate Record";
        //        }
        //    }
        //    return strMessage;
        //}

        public DataTable RptGetPCBTestStatus(string fromdate, string todate, int loginUserId)
        {
            DataTable dtrpt = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetReportTestStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", fromdate);
                cmd.Parameters.AddWithValue("@Todate", todate);
                cmd.Parameters.AddWithValue("@CreatedBy", loginUserId);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dtrpt);
                con.Close();
            }
            catch (Exception)
            { }
            return dtrpt;
        }

        public DataTable CompareADCMinMaxValue(string getcurrentvalue, int pcbtypeid, int testCaseId)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_CompareADCMinMaxValue", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Current", getcurrentvalue);
                cmd.Parameters.AddWithValue("@PCBTypeId", pcbtypeid);
                cmd.Parameters.AddWithValue("@TestcaseId", testCaseId);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dt);
                con.Close();
            }
            catch (Exception)
            { }
            finally
            { }
            return dt;
        }

        public string InsertTestCaseResult(ESDBEProperties _ObjBE)
        {
            try
            {
                int Status = 0;
                DataTable dtManual = new DataTable();
                SqlConnection con = new SqlConnection(DBAccess.getconstring());
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd = new SqlCommand("usp_SaveTestCaseResult", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", _ObjBE.PropUserID);
                cmd.Parameters.AddWithValue("@SerialNo", _ObjBE.PropSerialNo);
                cmd.Parameters.AddWithValue("@PCBTypeID", _ObjBE.PropPCBTypeID);
                cmd.Parameters.AddWithValue("@PCBType", _ObjBE.PropPCBType);
                cmd.Parameters.AddWithValue("@TestCaseID", _ObjBE.PropTestCaseID);
                cmd.Parameters.AddWithValue("@TestType", _ObjBE.PropTestType);
                cmd.Parameters.AddWithValue("@Status", _ObjBE.PropStatus);
                cmd.Parameters.AddWithValue("@FrameToSend", _ObjBE.PropFrameToSend);
                cmd.Parameters.AddWithValue("@ResponseFrame", _ObjBE.PropResponseFrame);
                cmd.Parameters.AddWithValue("@CreatedBy", _ObjBE.PropCreatedBy);
                cmd.Parameters.AddWithValue("@CurrentDate", _ObjBE.PropCurrentDateTime);
                cmd.Parameters.AddWithValue("@OutputValue", _ObjBE.PropComment);
                cmd.Parameters.AddWithValue("@ProductTypeID", _ObjBE.ProductTypeID);
                Status = cmd.ExecuteNonQuery();
                con.Close();
                saved = true;
            }
            catch (Exception ex)
            {
                strMessage = "Data_save_error";
            }
            finally
            {
                if (saved)
                {
                    strMessage = "Data_save_success";
                }
                else
                {
                    strMessage = "Duplicate Record";
                }
            }
            return strMessage;
        }

        public DataTable GetErrorDetails(string errorstr, int testCaseID)  //BP 10112023
       // public DataTable GetErrorDetails(string errorstr)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetErrorCode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ErrorCode", errorstr);
                cmd.Parameters.AddWithValue("@TestCaseId", testCaseID);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dt);
                con.Close();
            }
            catch (Exception)
            { }
            finally
            { }
            return dt;
        }

        public DataTable GetErrorMsg(int testCaseID)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("GetErrorMsg", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TestCaseId", testCaseID);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dt);
                con.Close();
            }
            catch (Exception)
            { }
            finally
            { }
            return dt;
        }

        public DataTable GetPCBTestStatus(int PcbTypeID, string SerialNo)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_GetPCBStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PCBTypeId", PcbTypeID);
                cmd.Parameters.AddWithValue("@SerialNo", SerialNo);
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DA.Fill(dt);
                con.Close();
            }
            catch (Exception)
            { }
            finally
            { }
            return dt;
        }


        public int AddUSer(string Username, string Password, int roleid, int UserId, string email = null, string mobileno = null)
        {
            int rowsAffected = default(int);
            // int records= default(int);
            SqlConnection con = new SqlConnection(DBAccess.getconstring());
            SqlCommand cmd = new SqlCommand();
            try
            {
                // Checking car exist in Database
                string s = @"SELECT COUNT(*) FROM tbl_User WHERE( Username = @Username and Password=@Password and IsActive=@IsActive)";
                cmd = new SqlCommand(s, con);
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@IsActive", "Y");
                con.Open();
                //int records=0;
                int records = (int)cmd.ExecuteScalar();

                if (records == 0)
                {
                    using (SqlCommand cmd1 = new SqlCommand("INSERT INTO tbl_User (Name, Username,Password,email,Mobile1,roleid,CreatedBy,CreatedDate,IsActive) VALUES (@Username, @Username,@Password,@email,@mobileno,@roleid,@CreatedBy,@CreatedDate,@IsActive)", con))
                    {
                        cmd1.CommandType = CommandType.Text;
                        //cmd1.Parameters.AddWithValue("@UserId", UserId);
                        cmd1.Parameters.AddWithValue("@Username", Username);
                        cmd1.Parameters.AddWithValue("@Password", Password);
                        cmd1.Parameters.AddWithValue("@email", email);
                        cmd1.Parameters.AddWithValue("@mobileno", mobileno);
                        cmd1.Parameters.AddWithValue("@roleid", roleid);
                        cmd1.Parameters.AddWithValue("@CreatedBy", UserId);
                        cmd1.Parameters.AddWithValue("@CreatedDate", System.DateTime.Now);
                        cmd1.Parameters.AddWithValue("@IsActive", "Y");
                        //con.Open();
                        rowsAffected = cmd1.ExecuteNonQuery();
                        con.Close();
                    }
                }
                else if (records > 0)
                {
                    rowsAffected = 2;
                    con.Close();
                }
                else
                { }
            }
            catch 
            { }
            return rowsAffected;
        }

        public string SaveKeyboardTest(ESDBEProperties _ObjBE)
        {
            try
            {
                int Status = 0;
                DataTable dtManual = new DataTable();
                SqlConnection con = new SqlConnection(DBAccess.getconstring());
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd = new SqlCommand("usp_SaveKeyPadTest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", _ObjBE.PropUserID);
                cmd.Parameters.AddWithValue("@SerialNo", _ObjBE.PropSerialNo);
                cmd.Parameters.AddWithValue("@PCBTypeID", _ObjBE.PropPCBTypeID);
                cmd.Parameters.AddWithValue("@PCBType", _ObjBE.PropPCBType);
                cmd.Parameters.AddWithValue("@TestCaseID", _ObjBE.PropTestCaseID);
                cmd.Parameters.AddWithValue("@Status", _ObjBE.PropStatus);
                cmd.Parameters.AddWithValue("@FrameToSend", _ObjBE.PropFrameToSend);
                cmd.Parameters.AddWithValue("@ResponseFrame", _ObjBE.PropResponseFrame);
                cmd.Parameters.AddWithValue("@CreatedBy", _ObjBE.PropCreatedBy);               
                cmd.Parameters.AddWithValue("@CurrentDate", _ObjBE.PropCurrentDateTime);
                Status = cmd.ExecuteNonQuery();
                con.Close();
                saved = true;
            }
            catch (Exception)
            {
                strMessage = "Data_save_error";
            }
            finally
            {
                if (saved)
                {
                    strMessage = "Data_save_success";
                }
                else
                {
                    strMessage = "Duplicate Record";
                }
            }
            return strMessage;
        }
    }
}
