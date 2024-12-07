namespace LibraryNet
{
    public class NetMessage
    {
        private const char SEPARATOR = '&';
        public const char ENDMES = '~';

        public TypeMessage _typeMessage;
        public string Message;

        public string FullMessage => $"{(int)_typeMessage}{SEPARATOR}{Message}{ENDMES}";

        public NetMessage(TypeMessage typeMessage, string mes)
        {
            _typeMessage = typeMessage;
            Message = mes;
        }

        public NetMessage(Entity entity)
        {
            _typeMessage = TypeMessage.Entity;
            Message = entity.Message;
        }

        public static TypeMessage GetTypeFromFullMessage(string fullMessage)
        {
            var tempType = fullMessage.Split(SEPARATOR);

            return (TypeMessage)int.Parse(tempType[0]);
        }

        public static string GetMessageFromFullMessage(string fullMessage)
        {
            var tempType = fullMessage.Split(SEPARATOR);

            return tempType[1];
        }
    }
}