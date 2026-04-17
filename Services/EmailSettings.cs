namespace LoginApp.Services
{
    public class EmailSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 25;
        public bool EnableSsl { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string From { get; set; } = "no-reply@loginapp.local";
        public bool UsePickupDirectory { get; set; } = true;
        public string PickupDirectoryLocation { get; set; } = "Emails";
    }
}
