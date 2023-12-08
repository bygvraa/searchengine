namespace Shared
{
    public class Config
    {
        public static string FOLDER { get; } = @"D:\Mikkel\code\data\small";

        public static string DATA { get; } = @"D:\Mikkel\code\data\emails";

        public static string DATABASE { get; } = @"D:\Mikkel\code\data\searchDB.db";

        public static string API_ADDRESS { get; } = @"http://localhost:5171";

        public static string LOADBALANCER_ADDRESS { get; } = @"http://localhost:5238";

        public static string DATABASE_ADDRESS { get; } = @"http://localhost:5097";
    }
}
