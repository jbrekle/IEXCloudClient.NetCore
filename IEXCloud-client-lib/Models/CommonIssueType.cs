using System.Runtime.Serialization;

namespace IEXCloudClient.NetCore
{
    public enum CommonIssueType
    {
        [EnumMember(Value = "ad")]
        ADR,

        [EnumMember(Value = "re")]
        REIT,

        [EnumMember(Value = "ce")]
        ClosedEndFund,

        [EnumMember(Value = "si")]
        SecondaryIssue,

        [EnumMember(Value = "lp")]
        LimitedPartnerships,

        [EnumMember(Value = "cs")]
        CommonStock,

        [EnumMember(Value = "et")]
        ETF,

        [EnumMember(Value = "wt")]
        Warrant,

        [EnumMember(Value = "oef")]
        OpenEndedFund,

        [EnumMember(Value = "cef")]
        ClosedEndedFund,

        [EnumMember(Value = "ps")]
        PreferredStock,

        [EnumMember(Value = "ut")]
        ///undocumented
        UT,

        [EnumMember(Value = "rt")]
        ///undocumented
        RT,
        
        [EnumMember(Value = "struct")]
        ///undocumented, bug?!
        Struct
    }
}