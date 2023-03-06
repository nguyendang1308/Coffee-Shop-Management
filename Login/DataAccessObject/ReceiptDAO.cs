﻿using Login.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DataAccessObject
{
    public class ReceiptDAO
    {
        private static ReceiptDAO instance;

        public static ReceiptDAO Instance
        {
            get { if (instance == null) instance = new ReceiptDAO(); return ReceiptDAO.instance; }
            private set { ReceiptDAO.instance = value; }
        }

        private ReceiptDAO() { }

        /// <summary>
        /// Return receipt.ID if query is success or return -1
        /// </summary>
        /// <param name="idTable"></param>
        /// <returns></returns>
        public int GetUncheckReceiptIDByTableID(int idTable)
        {
            string query = "SELECT * FROM dbo.Receipt WHERE idTable = " + idTable + " AND STATUS = 0";
            //string query = "USERPROC_GetUncheckReceiptIDByTableID @idTable";
            DataTable data = DataAccess.Instance.ExecuteQuery(query, new object[] { idTable });
            if(data.Rows.Count > 0)
            {
                Receipt receipt = new Receipt(data.Rows[0]);
                return receipt.ID;
            }
            return -1;
        }

        public void CheckOut(int id, int discount, float totalPrice)
        {
            //string query = "UPDATE dbo.Receipt SET status = 1, " + "discount = " + discount + " WHERE id = " + id;
            string query = "UPDATE dbo.Receipt SET departDate = GETDATE(), status = 1, " + "discount = " + discount + ", totalPrice = " + totalPrice + " WHERE id =  " + id;
            DataAccess.Instance.ExecuteNonQuery(query);

        }
        public void InsertReceipt(int id)
        {
            DataAccess.Instance.ExecuteNonQuery("EXEC USERPROC_InsertReceipt @idTable", new object[]{id});
        }

        public DataTable GetReceiptListByDate(DateTime arrivalDate, DateTime departDate)
        {
            return DataAccess.Instance.ExecuteQuery("EXEC USERPROC_GetListReceiptByDate @arrivalDate , @departDate", new object[] { arrivalDate, departDate });
        }

        public int GetMaxIDReceipt()
        {
            try
            {
                return (int)DataAccess.Instance.ExecuteScalar("SELECT MAX(id) FROM dbo.Receipt");
            }
            catch 
            { 
                return 1; 
            }
        }
    }
}
