using AlphaApp.Common.Entities.Dtos;
using AlphaApp.Repositories.Contracts;
using AlphaApp.Repositories.DatabaseContexts;
using AlphaApp.Repositories.Entities;
using AlphaApp.Repositories.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AlphaApp.Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AlphaAppContext _context;

        public UserRepository(AlphaAppContext context)
        {
            _context = context;
        }
        public string getSomething()
        {
            var temSTr = "";
            DataTable Ds = null;
           // var list = new List<IBase>();

            SQLHelper sqlHelper = new SQLHelper(_context);
            try
            {
                sqlHelper.StoredProcedureName = "Temp_Procedure";
                sqlHelper.Parameters.Add("@ActionType", SqlDbType.Int).Value = 3;
                sqlHelper.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = "Pakistan";
                sqlHelper.Parameters.Add("@Flag", SqlDbType.NVarChar).Value = "Pakistan";
                sqlHelper.Parameters.Add("@IsActive", SqlDbType.Bit).Value = true;
                sqlHelper.Parameters.Add("@CountryId", SqlDbType.Int).Value = 1;
                Ds = sqlHelper.ExecDataTable(sqlHelper._connectionString, sqlHelper._objCommand);
                if (Ds != null)
                {
                    foreach (DataRow dRow in Ds.Rows)
                    {
                        //CountryViewModel obj = new CountryViewModel();
                        //obj.CountryId = Convert.ToInt32(dRow["COUNTRY_ID"]);
                        //obj.CountryName = Convert.ToString(dRow["COUNTRY_NAME"]);
                        //obj.CountryCode = Convert.ToString(dRow["COUNTRY_CODE"]);
                        //obj.IsActive = Convert.ToBoolean(dRow["IS_ACTIVE"]);
                        //obj.MobilePrefix = Convert.ToString(dRow["MOBILE_PREFIX"]);

                        //list.Add(obj);
                        temSTr = Convert.ToString(dRow["Message"]);
                    }


                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return temSTr;

        }

    }
    }
