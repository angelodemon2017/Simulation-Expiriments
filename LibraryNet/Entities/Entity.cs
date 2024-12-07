namespace LibraryNet
{
    [System.Serializable]
    public class Entity
    {
        private const char separator = '^';

        public int Id { get; private set; }
        public float FieldValue { get; set; }
        public string Message => $"{Id}{separator}{FieldValue.ToString("F2")}";
        public string PresentText => $"Id:{Id}, val={FieldValue.ToString("F2")}";

        public Entity(string message)
        {
            var args = message.Split(separator);
            Id = int.Parse(args[0]);
            FieldValue = float.Parse(args[1]);
        }

        public Entity(int id)
        {
            Id = id;
            FieldValue = 0;
        }
    }
}