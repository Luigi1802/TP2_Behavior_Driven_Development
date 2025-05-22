using FluentAssertions;
using VotingSystem;

namespace TP2_BDD_User_story.Steps;

[Binding]
public sealed class VotingSystemStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private List<CandidateResult> _results;
    private string _winner;
    private Ballot _ballot;

    [Given(@"the ballot has ended")]
    public void GivenTheBallotHasEnded()
    {
        _ballot.EndBallot();
    }

    [Given(@"the votes are :")]
    public void GivenTheVotesAre(Table table)
    {
        foreach (var row in table.Rows)
        {
            var candidate = row["Candidate"];
            var votes = int.Parse(row["Votes"]);
            _ballot.AddCandidateVote(candidate, votes);
        }
    }

    [When(@"we calculate the ballot results")]
    public void WhenWeCalculateTheBallotResults()
    {
        _results = _ballot.CalculateResults();
        _winner = _ballot.GetWinner();
    }

    [Then(@"the winner is (.*)")]
    public void ThenTheWinnerIs(string expectedWinner)
    {
        _winner.Should().Be(expectedWinner);
    }

    [Then(@"the results are :")]
    public void ThenTheResultsAre(Table table)
    {
        foreach (var row in table.Rows)
        {
            var expectedVotes = int.Parse(row["Votes"]);
            var expectedPercentage = int.Parse(row["Percentage"]);

            var candidateResult = _results.Find(r => r.Name == row["Candidate"]);
            
            candidateResult.Should().NotBeNull("The candidate result should not be null");
            candidateResult?.VoteCount.Should().Be(expectedVotes);
            candidateResult?.Percentage.Should().Be(expectedPercentage);
        }
    }

    [Then(@"the results are not given")]
    public void ThenTheResultsAreNotGiven()
    {
        _results.Should().BeNull();
        _winner.Should().BeNull();
    }

    [Given(@"a ballot starts")]
    public void GivenABallotStarts()
    {
        _ballot = new Ballot();
    }

    [Then(@"the ballot is on second round")]
    public void ThenTheBallotIsOnSecondRound()
    {
        _ballot.isSecondRound.Should().Be(true);
    }

    [Then(@"the ballot is on first round")]
    public void ThenTheBallotIsOnFirstRound()
    {
        _ballot.isSecondRound.Should().Be(false);
    }

    [Then(@"there is no winner")]
    public void ThenThereIsNoWinner()
    {
        _winner.Should().Be(null);
    }

    [Then(@"no more round starts")]
    public void ThenNoMoreRoundStarts()
    {
        _ballot.IsClosed.Should().Be(true);
    }

    [Then(@"the three candidates are qualified")]
    public void ThenTheThreeCandidatesAreQualified()
    {
        _ballot.Votes.Count.Should().Be(3);
    }
}