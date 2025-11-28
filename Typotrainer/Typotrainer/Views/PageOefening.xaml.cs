namespace Typotrainer.Views;

public partial class PageOefening : ContentView
{
    private List<string> allSentences = new List<string>();
    private List<string> remainingSentences = new List<string>();
    private Random random;

    public PageOefening()
    {
        InitializeComponent(); 
        random = new Random(); 
        LoadSentences(); 
    }

    private async void LoadSentences()
    {
        {
            // Open makkelijk.txt en lees het als één string
            using var stream = await FileSystem.OpenAppPackageFileAsync("makkelijk.txt");
            using var reader = new StreamReader(stream);
            var content = await reader.ReadToEndAsync();
            
            // split de string tot de losse regels en voeg ze toe aan allSentences
            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var sentence = line.Trim();
                allSentences.Add(sentence);
            }
            // Maakt een kopie om te gebruiken voor de oefening, de andere lijst blijft compleet zodat we niet
            // elke keer de .txt hoeven te lezen.
            remainingSentences = new List<string>(allSentences);
        }
    }

    private void OnVolgendeButtonClicked(object sender, EventArgs e)
    {
        // Kijk naar het aantal zinnen, kies random een, toon die, en verwijder die uit de lijst
        int index = random.Next(remainingSentences.Count);
        string selectedSentence = remainingSentences[index];
        SentenceLabel.Text = selectedSentence;
        remainingSentences.RemoveAt(index);
    }

    private void OnResetButtonClicked(object sender, EventArgs e)
    {
        remainingSentences = new List<string>(allSentences);
        SentenceLabel.Text = "Klik op 'Volgende zin' om te beginnen";
    }
}