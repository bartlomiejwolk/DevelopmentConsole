using System.Collections.Generic;
using UnityEngine;

namespace DevelopmentConsoleTool {

    public struct Match {
        public string TextValue;
        public List<int> Positions;
    }

    public class FuzzySearch {

        public List<Match> MatchSearchSet(List<string> searchSet, string query) {
            if (query == string.Empty) {
                return null;
            }

            var tokens = query.ToCharArray();
            List<Match> matches = new List<Match>();

            foreach (var searchSetElement in searchSet) {
                var tokenIndex = 0;
                var stringIndex = 0;
                List<int> matchedPositions = new List<int>();

                var searchSetElementLower = searchSetElement.ToLower();

                while (stringIndex < searchSetElement.Length) {
                    if (searchSetElement[stringIndex] == tokens[tokenIndex]) {
                        matchedPositions.Add(stringIndex);
                        tokenIndex++;

                        if (tokenIndex >= tokens.Length) {
                            var match = new Match() {
                                TextValue = searchSetElementLower,
                                Positions = matchedPositions
                            };
                            matches.Add(match);

                            break;
                        }
                    }
                    stringIndex++;
                }
            }

            return matches;
        }
    }
}