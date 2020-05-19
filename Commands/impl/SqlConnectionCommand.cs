using System;
using System.Data.SqlClient;
using check_elo.Configuration;
using check_elo.Parameters;
using check_elo.Response;
using check_elo.Utils;
using pcm.IXClient7;

namespace check_elo.Commands.impl
{
    public class SqlConnectionCommand : BaseCommand<SqlConnectionParameters>
    {
        public SqlConnectionCommand(IXClient client, Settings settings, SqlConnectionParameters parameters) : base(
            client, settings, parameters)
        {
        }

        public override bool Run()
        {
            CheckResult.Message = "Connecting to SQL successful.";
            CheckResult.ExitCode = ExitCode.Ok;

            try
            {
                using (var sqlConnection = new SqlConnection(CreateSqlConnectionString()))
                {
                    sqlConnection.Open();
                }
            }
            catch (Exception ex)
            {
                CheckResult.Message = ex.Message;
                CheckResult.ExitCode = ExitCode.Critical;

                Log.Error($"Unable to create a SQL connection due to: {ex.Message}", ex);
            }
            finally
            {
                CheckResult.Commit();
            }

            return CheckResult.ExitCode == ExitCode.Ok;
        }

        private string CreateSqlConnectionString()
        {
            var dataSource = Helper.IfNullOrWhitespaceUse(Parameters.HostName, Settings.Database.Host);
            var catalog = Helper.IfNullOrWhitespaceUse(Parameters.DatabaseName, Settings.Elo.ArchiveName);
            var user = Helper.IfNullOrWhitespaceUse(Parameters.DatabaseUserName, Settings.Database.User);
            var password = Helper.IfNullOrWhitespaceUse(Parameters.DatabasePassword, Settings.Database.Password);

            return
                $"Data Source={dataSource};Initial Catalog={catalog};Persist Security Info=True;User ID={user};Password={Helper.DecryptPassword(password)}";
        }
    }
}