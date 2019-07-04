using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Npgsql;
using Npgsql.TypeHandling;

namespace TestDapper
{
    public class TelephonesTypeHandler : SqlMapper.TypeHandler<Telephones>
    {
        public override Telephones Parse(object value)
        {
            return Telephones.FromString(value.ToString());
        }

        public override void SetValue(IDbDataParameter parameter, Telephones value)
        {
            parameter.Value = value.ToString();
        }
    }
}
