namespace src.DTO.UserDto
{
    public class NotificationSettingsDto
    {
        public bool EmailEnabled { get; set; }
        public bool PushEnabled { get; set; }
        public bool SmsEnabled { get; set; }
        public bool MarketingEnabled { get; set; }
        public bool SecurityAlertsEnabled { get; set; } = true;
    }
}
