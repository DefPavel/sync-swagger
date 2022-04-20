using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace sync_swagger
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // ����������� ��������� win 1251 (������ ��� � Net. 5 � �� ����������, ����� ������������ ��������� ������� )
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding(1252);

            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
