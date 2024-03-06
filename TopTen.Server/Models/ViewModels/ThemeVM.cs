namespace TopTen.Server.Models.ViewModels
{
    public class ThemeVM
    {
        public string Content { get; set; }
        public string Biggest { get; set; }
        public string Smallest { get; set; }

        public ThemeVM() { }

        public ThemeVM(Theme theme)
        {
            Content = theme.Content;
            Biggest = theme.Biggest;
            Smallest = theme.Smallest;
        }
    }
}
