using System.Data.OracleClient;
using NHibernate.Driver;
using NHibernate.SqlTypes;

namespace N2.Persistence.NH
{
    public class CustomOracleClientDriver : OracleClientDriver
    {
        /// <summary>
        /// Initializes the parameter.
        /// </summary>
        /// <param name="dbParam">The db param.
        /// <param name="name">The name.
        /// <param name="sqlType">Type of the SQL.
        protected override void InitializeParameter(System.Data.IDbDataParameter dbParam, string name,
            global::NHibernate.SqlTypes.SqlType sqlType)
        {
            base.InitializeParameter(dbParam, name, sqlType);

            //System.Data.OracleClient.dll driver generates an exception
            //we set the IDbDataParameter.Value = (string whose length: 4000 > length > 2000 )
            //when we set the IDbDataParameter.DbType = DbType.String
            //when DB Column is of type NCLOB/CLOB
            //The Above is the default behavior for NHibernate.OracleClientDriver
            //So we use the built-in StringClobSqlType to tell the driver to use the NClob Oracle type
            //This will work for both NCLOB/CLOBs without issues.
            //Mapping file will need to be update to use StringClob as the property type
            if ((sqlType is StringClobSqlType))
            {
                ((OracleParameter) dbParam).OracleType = OracleType.NClob;
            }
        }
    }
}

