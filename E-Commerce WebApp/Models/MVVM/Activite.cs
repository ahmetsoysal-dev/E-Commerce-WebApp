namespace E_Commerce_WebApp.Models.MVVM
{
    public class Activite
    {
        public string? Tur { get; set; }
        public string? Adi { get; set; }
        public DateTime EtkinlikBaslamaTarihi { get; set; }
        public DateTime EtkinlikBitisTarihi { get; set; }
        public string? EtkinlikMerkezi { get; set; }
        public string? KisaAciklama { get; set; }
        public string? BiletSatisLinki { get; set; }
        public string? KucukAfis { get; set; }
    }
}
