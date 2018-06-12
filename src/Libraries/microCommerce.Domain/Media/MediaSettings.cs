using microCommerce.Domain.Settings;

namespace microCommerce.Domain.Media
{
    public class MediaSettings : ISettings
    {
        public int MaximumPictureSize { get; set; }
    }
}