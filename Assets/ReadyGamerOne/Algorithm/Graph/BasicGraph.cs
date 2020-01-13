namespace ReadyGamerOne.Algorithm.Graph
{
    public class BasicGraph
    {
        protected static int IdSour = 0;
        public readonly int id;

        protected BasicGraph()
        {
            id = IdSour++;
        }
    }
}