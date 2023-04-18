namespace DotTja.Types;

public sealed record TaikoWebSkin(
    DirectoryInfo Dir,
    string Name,
    string? Song,
    string? Stage,
    string? Don
);
