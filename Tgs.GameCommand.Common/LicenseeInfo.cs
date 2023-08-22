namespace Tgs.GameCommand.Common;

public record LicenseeInfo(
    Guid ResellerId,
    string ResellerCode,
    Guid LicenseeId,
    string LicenseeCode);
