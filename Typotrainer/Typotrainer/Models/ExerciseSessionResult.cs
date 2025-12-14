namespace Typotrainer.Models;

public class ExerciseSessionResult
{
    public DateTime Date { get; set; }
    public double Wpm { get; set; }
    public double Accuracy { get; set; }
    public TimeSpan TotalTime { get; set; }
    public int Mistakes { get; set; }

    public override string ToString()
    {
        return $"{Date:yyyy-MM-dd HH:mm};{Wpm:0};{Accuracy:0.0};{TotalTime.TotalSeconds:0};{Mistakes}";
    }

    public static ExerciseSessionResult FromString(string line)
    {
        var parts = line.Split(';');
        return new ExerciseSessionResult
        {
            Date = DateTime.Parse(parts[0]),
            Wpm = double.Parse(parts[1]),
            Accuracy = double.Parse(parts[2]),
            TotalTime = TimeSpan.FromSeconds(double.Parse(parts[3])),
            Mistakes = int.Parse(parts[4])
        };
    }
}
