namespace DotTja.Types;

using Enums;

public record Course
{
    public Difficulty? Difficulty { get; init; }
    public int? Stars { get; init; }
    public CourseVariant SingleCourse { get; init; } = new();
    public CourseVariant DoubleCourse { get; init; } = new();
};
