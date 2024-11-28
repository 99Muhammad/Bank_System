namespace Driving_License_Management_DVLD_.Helpers
{
    public class JWT
    {
        public string key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireDurationInDay { get; set; }
    }
}
