using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ServiceProcess;
using System.Windows.Forms;
using VMSAPP;

namespace VMSAPP_Service
{
    partial class frmReadData : ServiceBase
    {
        List<ReadBiometric> lBiometric = new List<ReadBiometric>();
        public frmReadData()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) { }
        
            void frmReadData_Load(object sender, EventArgs e)
            {


                //lBiometric.Add(new ReadBiometric(3, "10.50.50.60"));
                SqlCommand sqlCmd = new SqlCommand("[dbo].[Get_Plants]");
                sqlCmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = SQLHelper.ExecuteReader(ref sqlCmd);
                while (dr.Read())
                {
                    try
                    {
                        int _PlantId = Convert.ToInt32(dr["Id"]);
                        string _IpAddress = dr["IpAddress"].ToString();
                        lBiometric.Add(new ReadBiometric(_PlantId, _IpAddress));

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.InnerException.Message);
                    }

                }
                dr.Close();

            }
        

        protected override void OnStop()
        {
           
                foreach (ReadBiometric item in lBiometric)
                {
                    item.Disconnect_Device();

                }


            

        }
    }
}
