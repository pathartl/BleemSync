namespace BleemSync.Services.ViewModels
{
    public class CentralDisc
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }

        public virtual CentralGame Game { get; set; }
    }
}
