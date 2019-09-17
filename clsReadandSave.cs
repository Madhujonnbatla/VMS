using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zkemkeeper;

namespace VMSAPP
{
    public class ReadBiometric
    {

        CZKEMClass axKem = new CZKEMClass();
        static bool isConnected = false;
        static string conStr = ConfigurationManager.ConnectionStrings["VMSDB"].ToString();
        int _plant;
        static int VisitorCode;
        public ReadBiometric(int Plant, string IPAddress)
        {
            this._plant = Plant;
            Connect_Device(IPAddress);
        }
        private void Connect_Device(string IPAddress)
        {

            isConnected = this.axKem.Connect_Net(IPAddress, 4370);
            if (isConnected)
            {
                if (this.axKem.RegEvent(1, 65535))//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                {
                    this.axKem.OnAttTransactionEx += new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(onFingerPrintPass);
                }



            }
        }
        public void Disconnect_Device()
        {
            this.axKem.OnAttTransactionEx -= new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(onFingerPrintPass);
            this.axKem.Disconnect();
        }

        private void onFingerPrintPass(string sEnrollNumber, int iIsInValid, int iAttState, int iVerifyMethod, int iYear, int iMonth, int iDay, int iHour, int iMinute, int iSecond, int iWorkCode)
        {
            if (int.TryParse(sEnrollNumber, out VisitorCode))
            {
                if (VisitorCode > 999000000)
                {
                    DateTime SwipeTime = new DateTime(iYear, iMonth, iDay, iHour, iMinute, iSecond);
                    SqlCommand sqlCmd = new SqlCommand("[dbo].[ins_swipe]");
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@VisitorId", int.Parse(sEnrollNumber));
                    sqlCmd.Parameters.AddWithValue("@Plant", _plant);
                    sqlCmd.Parameters.AddWithValue("@SwipeTime", SwipeTime);
                    SQLHelper.ExecuteNonQuery(ref sqlCmd);
                }
            }





        }


    }
}
