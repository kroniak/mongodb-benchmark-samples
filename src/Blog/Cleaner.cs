using Npgsql;

namespace MongodbTransactions.Blog
{
    public class Cleaner
    {
        private const string DeleteSQLDataCommand = @"
                DELETE FROM comments;
                DELETE FROM articles;
                DELETE FROM articles_comments;
                DELETE FROM users;
                DELETE FROM packages;
                ";
        
        public static void CleanPostgresDb()
        {
            using (var conn = new NpgsqlConnection(Utils.GeneralUtils.PgConnectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText =
                        @"drop index if exists users_name_index;
                        drop index if exists articles_comments_userid_index;
                        drop index if exists articles_comments_comments_1_index;
                        drop index if exists articles_comments_comments_index;
                        drop index if exists articles_userid_index;
                        drop index if exists comments_userid_index;
                        drop index if exists comments_articleid_index";
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = DeleteSQLDataCommand;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}