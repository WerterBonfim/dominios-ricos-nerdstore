namespace NerdStore.WebApi.Extencoes
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpiracaoEmHoras { get; set; }
        public string Emissor { get; set; }
        public string ValidoEm { get; set; }
    }
}