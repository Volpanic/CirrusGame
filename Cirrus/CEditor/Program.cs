namespace CEditor
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using (var game = new EditorGame()) game.Run();
        }
    }
}