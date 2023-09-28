using System.Runtime.Serialization;

namespace AspNetCore.Api.Enums;

public enum OrderStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "PaymentRecieved")]
    PaymentRecieved,
    [EnumMember(Value = "PaymentFailed")]
    PaymentFailed

}
