namespace VotingSystem;

public class CandidateResult
{
    public string Name { get; set; }
    public int VoteCount { get; set; }
    public double Percentage { get; set; }
}

public class Ballot
{
    public bool IsClosed { get; private set; }
    public bool isSecondRound { get; private set; }
    public Dictionary<string, int> Votes { get; private set; } = new();
    public int BlankVotes { get; private set; } = 0;

    private int _totalVotes => Votes.Values.Sum() + BlankVotes;

    public void AddCandidateVote(string candidateName, int voteCount)
    {
        if (!Votes.ContainsKey(candidateName))
        {
            Votes[candidateName] = 0;
        }

        Votes[candidateName] += voteCount;
    }
    
    public void AddBlankVote(int count = 1)
    {
        BlankVotes += count;
    }

    public void EndBallot()
    {
        IsClosed = true;
    }

    public List<CandidateResult> CalculateResults()
    {
        if (!IsClosed)
            return null;

        var results = Votes
            .Select(v => new CandidateResult
            {
                Name = v.Key,
                VoteCount = v.Value,
                Percentage = _totalVotes == 0 ? 0 : Math.Round((double)v.Value / _totalVotes * 100)
            })
            .OrderByDescending(r => r.VoteCount)
            .ToList();
            
        if (BlankVotes > 0)
        {
            results.Add(new CandidateResult
            {
                Name = "Blank",
                VoteCount = BlankVotes,
                Percentage = Math.Round((double)BlankVotes / _totalVotes * 100)
            });
        }

        if (!isSecondRound)
        {
            // Premier tour
            if (results.Count > 0 && results[0].Percentage > 50)
            {
                IsClosed = true;
                return results;
            }

            // Pas de gagnant, on prépare le second tour
            isSecondRound = true;
            IsClosed = false;

            var qualified = new List<string>();

            var top1 = results[0];
            var top2 = results[1];
            qualified.Add(top1.Name);
            qualified.Add(top2.Name);

            // Vérifie égalité entre 2e et 3e
            if (results.Count > 2 && results[2].VoteCount == top2.VoteCount)
            {
                qualified.Add(results[2].Name);
            }

            // Préparer Votes pour le second tour avec les qualifiés à 0 vote
            Votes = qualified.ToDictionary(name => name, name => 0);
            return results;
        }
        else
        {
            // Second tour, on vérifie s'il y a un gagnant ou égalité
            if (results.Count >= 2 && results[0].VoteCount == results[1].VoteCount)
            {
                IsClosed = true;
                return results;
            }

            // Gagnant au second tour
            IsClosed = true;
            return results;
        }
    }

    public string GetWinner()
    {
        if (!IsClosed)
            return null;

        if (_totalVotes == 0)
            return null;

        var results = Votes.OrderByDescending(kv => kv.Value).ToList();

        if (!isSecondRound)
        {
            var percentage = (double)results[0].Value / _totalVotes * 100;
            return percentage > 50 ? results[0].Key : null;
        }
        else
        {
            if (results.Count >= 2 && results[0].Value == results[1].Value)
                return null;

            return results[0].Key;
        }
    }
}
