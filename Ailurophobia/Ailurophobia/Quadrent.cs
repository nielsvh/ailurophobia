using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ailurophobia
{
    class Quadrent
    {
        List<Character> chars;
        Quadrent[] children;
        public Quadrent()
        {
            chars = new List<Character>();
            children = new Quadrent[4];
        }

        public void addChar(Character newChar)
        {
            chars.Add(newChar);
        }

        public void buildTree()
        {
        }
    }
}
