namespace DotTja.Types;

public record TaikoWebSkin(
    DirectoryInfo Dir,
    string Name,
    string? Song,
    string? Stage,
    string? Don
);
