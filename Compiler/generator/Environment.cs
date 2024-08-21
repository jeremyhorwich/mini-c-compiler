namespace Generator
{
    public class Environment
    {
        private Dictionary<string, string> values = new Dictionary<string, string>();

        public void Define(string varName, string value)
        {
            if (values.ContainsKey(varName))
            {
                throw new Exception($"Error: variable {varName} is already declared elsewhere.");
            }
            values[varName] = value;
        } 

        public string Get(string varName)
        {
            if (values.ContainsKey(varName)) return values[varName];
            //TODO make this a parsing instead of compiling error
            throw new Exception($"Error: variable {varName} does not exist");
        }
    }
}