// 
// NHibernateConfiguration.cs
//  
// Author:
//       Bryan Allred <bryan.allred@gmail.com>
// 
// Copyright (c) 2011 Bryan Allred
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace WhiteNoise.Domain.NHibernate
{
	/// <summary>
	/// NHibernate configuration.
	/// </summary>
    public class NHibernateConfiguration
    {
		/// <summary>
		/// The connection string.
		/// </summary>
        private readonly string connectionString;
		
		/// <summary>
		/// The provider.
		/// </summary>
        private readonly string provider;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Wombat.Domain.NHibernate.NHibernateConfiguration"/> class.
		/// </summary>
		/// <param name='connectionString'>
		/// Connection string.
		/// </param>
		/// <param name='provider'>
		/// Provider.
		/// </param>
        public NHibernateConfiguration(string connectionString, string provider)
        {
            this.connectionString = connectionString;
            this.provider = provider;
        }
		
		/// <summary>
		/// Creates the database configuration.
		/// </summary>
		/// <returns>
		/// The database configuration.
		/// </returns>
		/// <param name='connectionString'>
		/// Connection string.
		/// </param>
		/// <param name='provider'>
		/// Provider.
		/// </param>
        public static IPersistenceConfigurer CreateDatabaseConfiguration(string connectionString, string provider)
        {
            switch (provider)
            {
                case "MsSql2008":
                case "System.Data.SqlClient":
                    return MsSqlConfiguration.MsSql2008.ConnectionString(connectionString).ShowSql();
                case "Firebird":
                    return new FirebirdConfiguration().ConnectionString(connectionString);
                case "MySql":
                    return MySQLConfiguration.Standard.ConnectionString(connectionString);
                case "MsSqlCe":
                    return MsSqlCeConfiguration.Standard.ConnectionString(connectionString);
                case "SQLite":
                    return SQLiteConfiguration.Standard.ConnectionString(connectionString);
                case "JetDriver":
                    return JetDriverConfiguration.Standard.ConnectionString(connectionString);
                default:
                    return MsSqlConfiguration.MsSql2008.ConnectionString(connectionString);
            }
        }
		
		/// <summary>
		/// Creates the session factory.
		/// </summary>
		/// <returns>
		/// The session factory.
		/// </returns>
        public ISessionFactory CreateSessionFactory()
        {
            var rawConfig = new Configuration();
			
			// NOTE: Removed but left in place as a reminder.
            //rawConfig.SetNamingStrategy(new MsSqlNamingStrategy());

            Configuration configuration = Fluently.Configure(rawConfig)
                .Database(CreateDatabaseConfiguration(this.connectionString, this.provider))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateConfiguration>())
                .BuildConfiguration();

            new SchemaUpdate(configuration).Execute(true, true);
            return configuration.BuildSessionFactory();
        }
    }
}