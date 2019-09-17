using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public class SQLHelper
{
    protected static string Connstring = "";
    static SqlConnection sqlconn;

    public static string ConString()
    {
        Connstring = ConfigurationManager.ConnectionStrings["VMSDB"].ToString();
        return Connstring;
    }

    protected static void ConnOpen()
    {


        sqlconn = new SqlConnection(ConString());
        try
        {
            sqlconn.Open();
        }
        catch
        {
        }
    }
    protected static void ConnClose()
    { sqlconn.Close(); }

    public static void ExecuteNonQuery(ref SqlCommand cmd)
    {
        ConnOpen();
        cmd.Connection = sqlconn;
        cmd.ExecuteNonQuery();
        sqlconn.Close();
    }
    //************************Configuration Section*************************************
    public static bool ExecuteNonQuery(ref SqlCommand cmd, out string msg)
    {
        ConnOpen();
        SqlTransaction tran;
        tran = sqlconn.BeginTransaction();
        try
        {
            tran.Commit();
            sqlconn.Close();
            msg = "Success";
            return true;
        }
        catch (Exception e)
        {
            tran.Rollback();
            sqlconn.Close();
            msg = e.Message;
            return false;
        }



        //try
        //    {
        //        ConnOpen();
        //        cmd.Connection = sqlconn;
        //        cmd.ExecuteNonQuery();
        //        sqlconn.Close();
        //        msg = "Success";
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        //SqlCommand cmdrb = new SqlCommand();
        //        //cmdrb.CommandType = CommandType.StoredProcedure;
        //        //cmdrb.CommandText = "RBCDHDR";
        //        //SQLHelper.ExcuteSingle(ref cmdrb);

        //        sqlconn.Close();
        //        msg = e.Message;
        //        return false;
        //    }
    }

    //***************************************************************
    public static void ExecuteNonQuery(ref SqlCommand[] cmd)
    {
        ConnOpen();
        SqlCommand sqlCmd = new SqlCommand();

        for (Int16 i = 0; i < cmd.Length; i++)
        {
            sqlCmd = cmd[i];
            sqlCmd.Connection = sqlconn;
            sqlCmd.ExecuteNonQuery();
        }
        sqlconn.Close();
    }

    public static DataSet ExecuteDataset(ref SqlCommand cmd)
    {
        ConnOpen();

        cmd.Connection = sqlconn;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        sqlconn.Close();
        return ds;
    }

    public static string ExecuteScalar(ref SqlCommand cmd)
    {
        string retval = "";
        ConnOpen();
        cmd.Connection = sqlconn;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        retval = cmd.ExecuteScalar().ToString();

        //SqlCommand cmdrb = new SqlCommand();
        //cmdrb.CommandType = CommandType.StoredProcedure;
        //cmdrb.CommandText = "RBCDHDR";
        //SQLHelper.ExcuteSingle(ref cmdrb);

        sqlconn.Close();
        return retval;
    }

    public static Int64 AUFNR(ref SqlCommand cmd)
    {
        ConnOpen();

        cmd.Connection = sqlconn;
        SqlDataAdapter da = new SqlDataAdapter(cmd);

        return (Int64)cmd.ExecuteScalar();
        sqlconn.Close();
    }

    public static String Scalar(ref SqlCommand cmd)
    {
        ConnOpen();

        cmd.Connection = sqlconn;
        SqlDataAdapter da = new SqlDataAdapter(cmd);

        return (String)cmd.ExecuteScalar();
        sqlconn.Close();
    }

    public static SqlDataReader ExecuteReader(ref SqlCommand cmd)
    {
        ConnOpen();
        cmd.Connection = sqlconn;
        return cmd.ExecuteReader();

    }

    public static SqlDataReader ExcuteSingle(ref SqlCommand cmd)
    {
        ConnOpen();

        cmd.Connection = sqlconn;
        return cmd.ExecuteReader(CommandBehavior.SingleRow);


    }

    public static bool ExecuteNonQueryTrans(ref SqlCommand[] cmd, out string msg)
    {
        ConnOpen();
        SqlTransaction tran;
        tran = sqlconn.BeginTransaction();
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            for (Int16 i = 0; i < cmd.Length; i++)
            {
                sqlCmd = cmd[i];
                sqlCmd.Connection = sqlconn;
                sqlCmd.Transaction = tran;
                sqlCmd.ExecuteNonQuery();
            }
            tran.Commit();
            sqlconn.Close();
            msg = "Success";
            return true;
        }
        catch (Exception e)
        {

            tran.Rollback();

            //SqlCommand cmdrb = new SqlCommand();
            //cmdrb.CommandType = CommandType.StoredProcedure;
            //cmdrb.CommandText = "RBCDHDR";
            //SQLHelper.ExcuteSingle(ref cmdrb);

            sqlconn.Close();
            msg = e.Message;
            return false;
        }

    }

    public static DataTable ExecuteDataTable(ref SqlCommand cmd)
    {
        ConnOpen();
        cmd.Connection = sqlconn;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        sqlconn.Close();
        return dt;

    }

    public static DataSet UpdateDataset(ref SqlCommand cmd, out SqlDataAdapter oda)
    {

        ConnOpen();
        DataSet ds = new DataSet("WHMS");
        cmd.Connection = sqlconn;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.FillSchema(ds, SchemaType.Source);
        da.Fill(ds);
        sqlconn.Close();
        oda = da;
        return ds;


    }

    public static void Update_EKPO(ref DataSet ds)
    {
        ConnOpen();
        sqlconn.Close();

    }





}
