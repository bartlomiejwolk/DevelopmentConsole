using System.Collections.Generic;
using DevelopmentConsole.FuzzySearch;

namespace DevelopmentConsoleTool.FuzzySearchTool {
	public class FuzzySearch {

        public List<Match> MatchResultSet(List<string> resultSet, string query) {
            if (query == string.Empty) {
                return null;
            }

            var tokens = query.ToCharArray();
            var matches = new List<Match>();

            foreach (var result in resultSet) {
                var tokenIndex = 0;
                var resultCharIndex = 0;
                var matchedPositions = new List<int>();

                while (resultCharIndex < result.Length) {
                    if (result[resultCharIndex] == tokens[tokenIndex]) {
                        matchedPositions.Add(resultCharIndex);
                        tokenIndex++;

                        if (tokenIndex >= tokens.Length) {
                            var match = new Match() {
                                TextValue = result,
                                Positions = matchedPositions
                            };
                            matches.Add(match);
                            break;
                        }
                    }
                    resultCharIndex++;
                }
            }
            return matches;
        }
    }
}