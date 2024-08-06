namespace Generate
{
    public class Instruction
    {
        private string operation;
        private string operand;

        public Instruction(string _operation, string _operand)
        {
            operation = _operation;
            operand = _operand;
        }

        public string Format()
        {
            return $"\n\t{operation}\t{operand}";
        }
    }
}