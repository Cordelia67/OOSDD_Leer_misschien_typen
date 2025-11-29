using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typotrainer.Services
{
    public class TypControle
    {
        public bool IsCorrectLetter(string correctZin, int Index, char typedChar)
        {
            if (string.IsNullOrEmpty(correctZin))
            {
                return false;
            }

            if (Index < 0 || Index >= correctZin.Length)
            {
                return false;
            }

            return correctZin[Index] == typedChar;
        }
    }
}