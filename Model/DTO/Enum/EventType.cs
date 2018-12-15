using Tools.Attributes;

namespace Model.DTO.Enum
{
    public enum EventType
    {
        [Display("EventType_Organisational")]
        ORGANIZATIONAL,
        [Display("EventType_Plenary")]
        PLENARY,
        [Display("EventType_Section")]
        SECTION
    }
}
