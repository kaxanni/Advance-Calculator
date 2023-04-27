using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Variables
{
    public class Variable
    {
        //para ma store ang variables
        private Dictionary<string, double> _variables;
        //ang variables
        public Variable()
        {
            _variables = new Dictionary<string, double>();
        }
        //pag add og variable
        public void add(string name, double num)
        {
            _variables[name] = num;
        }
        //kung mali ang pag butang sa equation
        public double get(string name)
        {
            if (_variables.ContainsKey(name)) {
                return _variables[name];
            }

            throw new Exception("Variable \"" + name + "\" not found!");
        }

        
        public override string ToString()
        {
            var s = "";
            foreach(var x in _variables)
            {
                s += string.Format("{0} = {1}\n", x.Key, x.Value);
            }

            return s;
        }
    }
}
