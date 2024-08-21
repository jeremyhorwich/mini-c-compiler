namespace Generate
{
    public class CEnvironment
    {
        private Dictionary<string, string> values = new Dictionary<string, string>();
        private CEnvironment? enclosing;

        public CEnvironment()
        {
            enclosing = null;
        }

        public CEnvironment(CEnvironment _enclosing)
        {
            enclosing = _enclosing;
        }

        public void Define(string varName, string value)
        {
            if (values.ContainsKey(varName))
            {
                throw new Exception($"Error: variable {varName} is already declared in this scope.");
            }

            //Note: we don't check enclosing environments for conflicts

            values[varName] = value;
        } 

        public string Get(string varName)
        {
            if (values.ContainsKey(varName)) return values[varName];
            if (enclosing != null) return enclosing.Get(varName);
            
            //TODO make this a parsing instead of compiling error
            throw new Exception($"Error: variable {varName} does not exist");
        }
    }
}