namespace DotTja.Types;

using Enums;

public sealed partial record Course
{
    internal sealed class Builder
    {
        public Difficulty? Difficulty { get; set; }
        public int? Stars { get; set; }
        public CourseVariant.Builder SingleCourse { get; set; } = new();
        public CourseVariant.Builder DoubleCourse { get; set; } = new();

        public Course ToCourse() => new()
        {
            Difficulty = this.Difficulty,
            Stars = this.Stars,
            SingleCourse = this.SingleCourse.ToCourseVariant(),
            DoubleCourse = this.DoubleCourse.ToCourseVariant()
        };
    }
};
