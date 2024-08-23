namespace Generate
{
    public class CEnvironment
    {
        private Dictionary<string, CVariable> variables = new Dictionary<string, CVariable>();
        private CEnvironment? enclosing;

        public int offset { get; private set; } = 0;
        
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
            if (variables.ContainsKey(varName))
            {
                throw new Exception($"Error: variable {varName} is already declared in this scope.");
            }

            //Note: we don't check enclosing environments for conflicts

            offset -= 4;
            string strAddress = offset.ToString() + "(%rbp)";

            CVariable defined = new CVariable(strAddress, value);

            variables[varName] = defined;
        } 

        public CVariable Get(string varName)
        {
            if (variables.ContainsKey(varName)) return variables[varName];
            if (enclosing != null) return enclosing.Get(varName);
            
            //TODO make this a parsing instead of compiling error
            throw new Exception($"Error: variable {varName} does not exist");
        }
    }

    public class CVariable
    {
        public string address { get; private set; }
        public string value;

        public CVariable(string _address, string _value)
        {
            address = _address;
            value = _value;
        }
    }
}