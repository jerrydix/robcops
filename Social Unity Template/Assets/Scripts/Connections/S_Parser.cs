using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Connections
{

    public static class S_Parser
    {

        public static List<string> ParseResponse(string contents)
        {
            var substrings = contents.Split('|');
            if (substrings[0] == "0")
            {
                throw new HttpListenerException(0, substrings[1]);
            }

            var result = new List<string>();
            for (int i = 0; i < substrings.Length; i++)
            {
                result.Add(substrings[i]);
            }

            return result;
        }
    }
}
