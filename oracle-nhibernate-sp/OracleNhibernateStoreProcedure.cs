using NHibernate;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;


public class OracleNhibernateStoreProcedure
{
    /// <summary>
    /// This function calls the SP_NAME store procedure and return the result output value from it
    /// </summary>
    /// <param name="nHibernateSession">The nhibernate session</param>
    /// <returns></returns>
    public string GetResultFromStoreProcedure(ISession nHibernateSession)
    {
        try
        {
            IDbCommand command = new OracleCommand();
            command.Connection = nHibernateSession.Connection;
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "SP_NAME";

            command.Parameters.Add(new OracleParameter("@i_userFirstName", "Luciano"));
            command.Parameters.Add(new OracleParameter("@i_userLastName", "Ybanez"));


            OracleParameter outputParameter =
                new OracleParameter("@o_result", OracleDbType.Varchar2, 255)
                {
                    Direction = ParameterDirection.Output
                };
            command.Parameters.Add(outputParameter);

            nHibernateSession.Transaction.Enlist((DbCommand)command);

            command.ExecuteNonQuery();

            var result = ((OracleParameter)command.Parameters["@o_result"]).Value;

            if (result != null)
            {
                return result.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        catch (Exception ex) when (ex.GetType() == typeof(OracleException))
        {
            throw new Exception(ex.Message, ex);
        }

    }
}
